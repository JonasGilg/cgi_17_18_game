using System.Collections.Generic;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Texture;
using Engine.Util;

namespace Game.GameObjects {
    
    public enum PointType {
        Copper, Silver, Gold
    }
    

    internal static class PointRingTextureRegistry {
        private const string PATH = "data/textures/pointring/";
        public static Dictionary<PointType, MaterialSettings> MATERIAL_SETTINGS;

        static PointRingTextureRegistry() {
            MATERIAL_SETTINGS = new Dictionary<PointType, MaterialSettings>();
            foreach (PointType ptype in EnumUtil.GetValues<PointType>()) {
                MATERIAL_SETTINGS.Add(ptype, new MaterialSettings {
                    ColorTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/color.png"),
                    AOTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/ao.png"),
                    NormalTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/normal.png"),
                    MetalnessTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/metalness.png"),
                    RoughnessTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/roughness.png"),
                    GlowTexture = TextureManager.LoadTexture(PATH + ptype.ToString().ToLower() + "/blur.png")
                });
            }
        }
    }

    public class PointRing : Ring {
        public int points;
        public PointType pointType;
        

        public PointRing(PointType type) {
            pointType = type;
            
            switch (type) {
                case PointType.Copper:
                    points = 1;
                    break;
                case PointType.Silver:
                    points = 5;
                    break;
                case PointType.Gold:
                    points = 20;
                    break;
            }
            renderComponent.MaterialSettings = PointRingTextureRegistry.MATERIAL_SETTINGS[pointType];
        }

        protected override CollisionComponent InitCollider() => new SphereCollider(this, Model, passiveMessage => {
            IO.PrintAsync("points collected");
            Statistics.IncreaseScore(points);
        });

        protected override RenderComponent InitRenderer() => new RenderComponent(
            Model,
            MaterialManager.GetMaterial(Material.PBR),
            new MaterialSettings {
                Shininess = 1.5
            },
            this
        );
    }
}