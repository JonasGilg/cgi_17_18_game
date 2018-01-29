using System;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.GUI;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Game.Components;

namespace Game.GameObjects {
	public class Planet : GameObject {
		private static readonly Model3D PLANET_MODEL = ModelLoaderObject3D.Load("data/objects/Planet.obj");

		public readonly RenderComponent RenderComponent;
		public readonly MoveComponent MoveComponent;
		public readonly CollisionComponent CollisionComponent;

		public readonly HudObjectMarker objectMarker;
		
		private int _hp = 10;
		public int hp {
			get => _hp;
			set {
				_hp = value;
				if (_hp <= 0) {
					Destroy(this);
				}
			}
		}

		public Planet(int textureId, GameObject referenceObject = null) {
			RenderComponent = new RenderComponent(
				PLANET_MODEL,
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
				collision => { Console.WriteLine(ToString() + " collided with " + collision.otherGameObject.ToString()); });
			CollisionEngine.Register(CollisionComponent);
			
			objectMarker =HUD.CreateHudObjectMarker(this);
			
		}
		
		public void activateMarker() {
			HUD.AddHudObjectMarker(objectMarker);
		}

		public void deactivateMarker() {
			HUD.RemoveHudObjectMarker(objectMarker.ID);
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
		
		protected override void OnDestroy() {
			//TODO BIIIG explosion
			RenderEngine.UnregisterRenderComponent(RenderComponent);
			HUD.RemoveHudObjectMarker(objectMarker.ID);
			CollisionEngine.Unregister(CollisionComponent);
		}

		public override string ToString() {
			return TransformComponent.WorldPosition.ToString();
		}
	}
}