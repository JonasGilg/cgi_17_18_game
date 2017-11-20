using System.Collections.Generic;
using Engine.Material;

namespace Engine.Material {
	public enum Material {
		SIMPLE,
		SIMPLE_REFLECTION,
		AMBIENT_DIFFUSE_SPECULAR,
		NORMAL_MAPPING,
		BLACK_HOLE,
		SUN_LAVAFLOW
	}

	public static class MaterialManager {
		private static readonly Dictionary<Material, BaseMaterial> MATERIALS;

		static MaterialManager() {
			MATERIALS = new Dictionary<Material, BaseMaterial> {
				[Material.SIMPLE] = new SimpleTextureMaterial(),
				[Material.SIMPLE_REFLECTION] = new SimpleReflectionMaterial(),
				[Material.AMBIENT_DIFFUSE_SPECULAR] = new AmbientDiffuseSpecularMaterial(),
				[Material.NORMAL_MAPPING] = new NormalMappingMaterial(),
				[Material.BLACK_HOLE] = new BlackHoleMaterial(),
				[Material.SUN_LAVAFLOW] = new SunLavaflowMaterial()
			};
		}

		public static BaseMaterial GetMaterial(Material material) => MATERIALS[material];
	}
}