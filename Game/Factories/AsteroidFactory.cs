using System;
using System.Collections.Generic;
using Engine;
using Engine.Model;
using Engine.Texture;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	public static class AsteroidFactory {
		
		public enum AsteroidType {
			STANDARD,
			STRAWBERRY
		}
		public static readonly Dictionary<AsteroidType, string> AsteroidModelDictionary = new Dictionary<AsteroidType, string> {
			{AsteroidType.STANDARD, "data/objects/asteroids/asteroid_0.obj"},
			{AsteroidType.STRAWBERRY, "data/objects/asteroids/strawberry.obj"}
		};

		public static readonly Dictionary<AsteroidType, string> AsteroidTexturesDictionary = new Dictionary<AsteroidType, string> {
			{AsteroidType.STANDARD,"data/textures/asteroids/asteroid_0.png"},
			{AsteroidType.STRAWBERRY,"data/textures/asteroids/strawberry.png"}
		};


		public static Asteroid GenerateAsteroid(AsteroidType type) {
			//Console.Out.WriteLine("GenerateAsteroid with type: "+type);
			
			var model = ModelLoaderObject3D.Load(AsteroidModelDictionary[type]);
			
			var textureId = TextureManager.LoadTexture(AsteroidTexturesDictionary[type]);
			
			var asteroid = new Asteroid(model,textureId);
			
			return asteroid;
		}

		
		
		
		
	}
}