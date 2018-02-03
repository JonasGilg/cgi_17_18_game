using System;
using Engine.Material.Deferred;

namespace Engine.Material {
	/// <summary>
	/// An Enum of all available Materials in the order of which they are drawn.
	/// </summary>
	public enum Material {
		PBR = 0,
		AMBIENT_DIFFUSE_SPECULAR = 1,
		NORMAL_MAPPING = 2,
		SIMPLE_TEXTURE_MATERIAL = 3,
		BLACK_HOLE = 4,
		SUN_LAVAFLOW = 5
	}

	public static class MaterialManager {
		private static readonly BaseMaterial[] MATERIALS;

		static MaterialManager() {
			MATERIALS = new BaseMaterial[Enum.GetNames(typeof(Material)).Length];

			MATERIALS[(int) Material.PBR] = new PBRMaterial();
			MATERIALS[(int) Material.AMBIENT_DIFFUSE_SPECULAR] = new AmbientDiffuseSpecularMaterial();
			MATERIALS[(int) Material.NORMAL_MAPPING] = new NormalMappingMaterial();
			MATERIALS[(int) Material.SIMPLE_TEXTURE_MATERIAL] = new SimpleTextureMaterial();
			MATERIALS[(int) Material.BLACK_HOLE] = new BlackHoleMaterial();
			MATERIALS[(int) Material.SUN_LAVAFLOW] = new SunLavaflowMaterial();
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