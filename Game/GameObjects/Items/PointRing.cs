using System;
using System.Collections.Generic;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Texture;
using Engine.Util;

namespace Game.GameObjects {
    
    public enum PointType {
        Charcoal, Copper, Silver, Gold
    }
    
    public class PointRing : Ring {
        public int points;
        public PointType pointType;

        public static Dictionary<PointType, int> colorTextures = new Dictionary<PointType, int> {
            {PointType.Copper,TextureManager.LoadTexture("data/textures/supplyRing/gold.png")},
            {PointType.Silver,TextureManager.LoadTexture("data/textures/supplyRing/gold.png")},
            {PointType.Gold,TextureManager.LoadTexture("data/textures/supplyRing/gold.png")},
            {PointType.Charcoal,TextureManager.LoadTexture("data/textures/supplyRing/gold.png")},
        };
        

        public PointRing(PointType type) {
            //TODO assign correct physical based rendering settings for each metal type
            switch (type) {
                case PointType.Charcoal:
                    points = -5;
                    break;
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
            renderComponent.MaterialSettings.ColorTexture = colorTextures[pointType];
        }

        protected override CollisionComponent InitCollider() => new SphereCollider(this, Model, passiveMessage => {
            IO.PrintAsync("points collected");
            Statistics.IncreaseScore(points);
        });

        protected override RenderComponent InitRenderer() => new RenderComponent(
            Model,
            MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
            new MaterialSettings {
                Shininess = 1.5 /*,
                    MetalnessTexture = TextureManager.LoadTexture("data/textures/pbr/" + type.ToString().ToLower() + "-metalness.png"),
                    RoughnessTexture = TextureManager.LoadTexture("data/textures/pbr/" + type.ToString().ToLower() + "-roughness.png")*/
            },
            this
        );
    }
}