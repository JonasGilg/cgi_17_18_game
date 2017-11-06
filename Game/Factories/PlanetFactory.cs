using System;
using System.Collections.Generic;
using Engine.Texture;
using Game.GameObjects;
using OpenTK;
using OpenTK.Graphics.ES11;

namespace Game.Utils {
    public static class PlanetFactory {
        public enum PlanetTexture {
            SUN,
            NEPTUN,
            JUPITER
        }
        public static Dictionary<PlanetTexture,string> planetTextures = new Dictionary <PlanetTexture,string>() {
            {PlanetTexture.SUN,"data/textures/planets/2k_sun.jpg"},
            {PlanetTexture.NEPTUN,"data/textures/planets/neptunemap.jpg"},
            {PlanetTexture.JUPITER,"data/textures/planets/2k_jupiter.jpg"}
            
        };
        
        
        public static Planet GeneratePlanet(PlanetTexture planetTexture, Vector3 position, Vector3 scale, Vector3 rotation) {
            var textureId = TextureManager.LoadTexture(planetTextures[planetTexture]);
            var result = new Planet(textureId) {
                TransformComponent = {
                    Scale = scale,
                    Position = position
                },
                MoveComponent = { AngularVelocity = rotation }
            };

            return result;
        }
    }
}