using System;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Game.Components;

namespace Game.GameObjects {
	public class Asteroid : GameObject {
		public readonly MoveComponent MoveComponent;
		private readonly RenderComponent renderComponent;
		public readonly SphereCollider CollisionComponent;

		public Asteroid(Model3D model, int textureId, GameObject referenceObject = null) {
			MoveComponent = referenceObject != null ? new GravityMovement(this, 0.0) : new MoveComponent(this);
			
			renderComponent = new RenderComponent(
				model,
				MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
				new MaterialSettings {
					ColorTexture = textureId,
					Shininess = 1
				},
				this
			);
			
			RenderEngine.RegisterRenderComponent(renderComponent);

			CollisionComponent = new SphereCollider(this, renderComponent.Model,
				collision => { /*Console.WriteLine("Asteroid collided with" + collision.GameObject.ToString());*/ });
			CollisionComponent.Register();
		}

		public override void Awake() {
			base.Awake();
			Radius = renderComponent.Model.Radius(TransformComponent.Scale);
			renderComponent.AABB = renderComponent.AABB * TransformComponent.Scale;
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			renderComponent.Update();
		}
	}
}