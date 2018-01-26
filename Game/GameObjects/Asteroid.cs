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

		private int _hp = 3;
		public int hp {
			get => _hp;
			set {
				_hp = value;
				if (_hp <= 0) {
					Destroy();
				}
			}
		}

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
				collision => {
					if (collision.otherGameObject is Asteroid asteroid) {
						Console.WriteLine("Asteroid " + ToString() + " collided with Asteroid " + asteroid.ToString());
					}
				});
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

		public override void Destroy() {
			base.Destroy();
			RenderEngine.UnregisterRenderComponent(renderComponent);
			CollisionComponent.Unregister();
		}
	}
}