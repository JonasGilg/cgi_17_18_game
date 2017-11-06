using System;
using System.Collections.Generic;
using Engine.Texture;
using Game.GameObjects;
using OpenTK;
using OpenTK.Graphics.ES11;

namespace Game.Utils {
    public static class PlanetFactory {
        public enum PlanetTexture {
            Sun,
            Neptun,
            Jupiter
        }
        
        public static readonly Dictionary<PlanetTexture,string> PlanetTextures = new Dictionary <PlanetTexture,string> {
            {PlanetTexture.Sun,"data/textures/planets/2k_sun.jpg"},
            {PlanetTexture.Neptun,"data/textures/planets/neptunemap.jpg"},
            {PlanetTexture.Jupiter,"data/textures/planets/2k_jupiter.jpg"}
            
        };
        
        
        public static Planet GeneratePlanet(PlanetTexture planetTexture, Vector3d position, Vector3d scale, Vector3d rotation) {
            var textureId = TextureManager.LoadTexture(PlanetTextures[planetTexture]);
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