using System;
using System.Runtime.Remoting.Messaging;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.GUI;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Game.Components;

namespace Game.GameObjects {
	public class Planet : GameObject {
		private static readonly Model3D PLANET_MODEL = ModelLoaderObject3D.Load("data/objects/Planet.obj");
		private static readonly int NORMAL_ID = TextureManager.LoadTexture("data/textures/defaultNormalMap.png");
		private static readonly int METALNESS_ID = TextureManager.LoadTexture("data/textures/simpleBlack.png");
		private static readonly int ROUGHNESS_ID = TextureManager.LoadTexture("data/textures/simpleBlack.png");
		private static readonly int AO_ID = TextureManager.LoadTexture("data/textures/simpleWhite.png");
		private static readonly int BLUR_ID = TextureManager.LoadTexture("data/textures/planets/blur.png");
		
		public readonly RenderComponent RenderComponent;
		private readonly ShadowComponent shadowComponent;
		public readonly MoveComponent MoveComponent;
		private readonly CollisionComponent collisionComponent;

		public Planet(int textureId) {
			RenderComponent = new RenderComponent(
				PLANET_MODEL,
				MaterialManager.GetMaterial(Material.PBR),
				new MaterialSettings {
					ColorTexture = textureId,
					NormalTexture = NORMAL_ID,
					MetalnessTexture = METALNESS_ID,
					RoughnessTexture = ROUGHNESS_ID,
					AOTexture = AO_ID,
					GlowTexture = BLUR_ID
				},
				this
			);

			shadowComponent = new ShadowComponent(RenderComponent, this);

			
			MoveComponent = new MoveComponent(this);
			
			
			collisionComponent = new SphereCollider(this, RenderComponent.Model,
				message => {
				
					if (message.OtherCollisonComponent.GameObject.searchOptionalComponents(ComponentType.HEALTH_COMPONENT, out var componentList)) {
						for (int i = 0; i < componentList.Count; i++) {
							((HealthComponent) componentList[i]).instaKill();
						}	
					}
				},
				null//noActive,
				//noPhysics
			);
			
			
			
			
		}
		
		public void activateMarker() {
			HUD.AddHudObjectMarker(objectMarker);
		}

		public void deactivateMarker() {
			HUD.RemoveHudObjectMarker(objectMarker.ID);
		}

		

		public override void Awake() {
			base.Awake();
			RenderEngine.RegisterRenderComponent(RenderComponent);
			CollisionEngine.Register(collisionComponent);
			
			Radius = RenderComponent.Model.Radius(TransformComponent.Scale);
			RenderComponent.AABB = RenderComponent.AABB * TransformComponent.Scale;
			TransformComponent.UpdateWorldMatrix();
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			RenderComponent.Update();
			shadowComponent.Update();
		}
		
		protected override void OnDestroy() {
			//TODO BIIIG explosion
			RenderEngine.UnregisterRenderComponent(RenderComponent);
			CollisionEngine.Unregister(collisionComponent);
			HUD.RemoveHudObjectMarker(objectMarker.ID);
		}

		public override string ToString() {
			return TransformComponent.WorldPosition.ToString();
		}
	}
}