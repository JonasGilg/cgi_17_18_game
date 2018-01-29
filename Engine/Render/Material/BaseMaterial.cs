using System.Collections.Generic;
using Engine.Component;
using Engine.Model;
using Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Material {
	public struct MaterialSettings {
		public int ColorTexture;
		public int NormalTexture;
		public int CubeTexture;
		public double Shininess;

		public int MetalnessTexture;
		public int RoughnessTexture;
		public int AOTexture;
		public int GlowTexture;

		public Vector3 Color;
		
		public BlendingFactorSrc SrcBlendFactor;
		public BlendingFactorDest DestBlendFactor;
	}

	public abstract class BaseMaterial {
		protected int Program;

		private readonly List<RenderComponent> objectsToDraw = new List<RenderComponent>();

		protected void CreateShaderProgram(string pathVs, string pathFs) {
			Program = ShaderLoader.LoadShader(pathVs, pathFs);
		}

		protected abstract void PreDraw();
		protected abstract void Draw(Model3D model, MaterialSettings materialSettings);
		protected abstract void PostDraw();
		
		/// <summary>
		/// Adds an entity to be drawn this frame.
		/// </summary>
		/// <param name="entity">The entity to be drawn this frame.</param>
		public void RegisterForDraw(RenderComponent entity) {
			objectsToDraw.Add(entity);
		}

		/// <summary>
		/// Draws all objects, that are registered to be drawn this frame.
		/// </summary>
		public void DrawAll() {
			PreDraw();
			for (var i = 0; i < objectsToDraw.Count; i++) {
				var entity = objectsToDraw[i];
				entity.Model.Transformation = entity.GameObject.TransformComponent.WorldMatrix;
				Draw(entity.Model, entity.MaterialSettings);
			}
			PostDraw();
			objectsToDraw.Clear();
		}
	}
}