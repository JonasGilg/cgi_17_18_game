using System.Collections.Generic;

namespace Engine.Material {
	public enum Material {
		Simple,
		SimpleReflection,
		AmbientDiffuseSpecular,
		NormalMapping
	}
	
	public static class MaterialManager {
		private static readonly Dictionary<Material, BaseMaterial> Materials;

		static MaterialManager() {
			Materials = new Dictionary<Material, BaseMaterial> {
				[Material.Simple] = new SimpleTextureMaterial(),
				[Material.SimpleReflection] = new SimpleReflectionMaterial(),
				[Material.AmbientDiffuseSpecular] = new AmbientDiffuseSpecularMaterial(),
				[Material.NormalMapping] = new NormalMappingMaterial()
			};
		}

		public static BaseMaterial GetMaterial(Material material) => Materials[material];
	}
}