using System;

namespace Engine.Material {
	/// <summary>
	/// An Enum of all available Materials in the order of which they are drawn.
	/// </summary>
	public enum Material {
		SIMPLE = 0,
		SIMPLE_REFLECTION = 1,
		AMBIENT_DIFFUSE_SPECULAR = 2,
		NORMAL_MAPPING = 3
	}

	public static class MaterialManager {
		private static readonly BaseMaterial[] MATERIALS;

		static MaterialManager() {
			MATERIALS = new BaseMaterial[Enum.GetNames(typeof(Material)).Length];

			MATERIALS[(int) Material.SIMPLE] = new SimpleTextureMaterial();
			MATERIALS[(int) Material.SIMPLE_REFLECTION] = new SimpleReflectionMaterial();
			MATERIALS[(int) Material.AMBIENT_DIFFUSE_SPECULAR] = new AmbientDiffuseSpecularMaterial();
			MATERIALS[(int) Material.NORMAL_MAPPING] = new NormalMappingMaterial();
		}

		public static BaseMaterial GetMaterial(Material material) => MATERIALS[(int) material];

		/// <summary>
		/// Draws all Materials in succession.
		/// </summary>
		public static void DrawAll() {
			for (var i = 0; i < MATERIALS.Length; i++) {
				MATERIALS[i].DrawAll();
			}
		}
	}
}