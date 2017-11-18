using System;
using Engine;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Game.Components;

namespace Game.GameObjects {
	public class Planet : GameObject {
		public readonly RenderComponent RenderComponent;
		public readonly MoveComponent MoveComponent;
		public readonly CollisionComponent CollisionComponent;

		public Planet(int textureId, GameObject referenceObject = null) {
			RenderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/Planet.obj", this),
				MaterialManager.GetMaterial(Material.AMBIENT_DIFFUSE_SPECULAR),
				new MaterialSettings {
					ColorTexture = textureId,
					Shininess = 0.0
				},
				this
			);
			
			RenderEngine.RegisterRenderComponent(RenderComponent);
			
			if (referenceObject != null) {
				MoveComponent = new GravityMovement(this, 0.0);
			}
			else {
				MoveComponent = new MoveComponent(this);
			}
			
			CollisionComponent = new SphereCollider(this, RenderComponent.Model,
				collision => { Console.WriteLine(ToString() + " collided with " + collision.GameObject.ToString()); });
			CollisionComponent.Register();
		}

		public override void Awake() {
			base.Awake();
			Radius = RenderComponent.Model.Radius(TransformComponent.Scale);
			RenderComponent.AABB = RenderComponent.AABB * TransformComponent.Scale;
			TransformComponent.UpdateWorldMatrix();
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			RenderComponent.Update();
		}

		public override string ToString() {
			return TransformComponent.WorldPosition.ToString();
		}
	}
}