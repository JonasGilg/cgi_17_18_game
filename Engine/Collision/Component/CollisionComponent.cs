using System;
using Engine.Collision;
using Engine.Collision.Component;
using Engine.Component;
using Engine.Model;
using Engine.Render;

namespace Engine {
	

	public delegate void ExecuteFunction(CollisionMessage collisionMessage);
	
	public abstract class CollisionComponent : Component.Component, IOctreeItem<CollisionComponent> {
		
		private readonly Model3D model3D;
		private readonly AxisAlignedBoundingBox aabb;
		public PhysicsMaterial PhysicsMaterial;

		public readonly ExecuteFunction ActiveCollisionFunctions;
		public readonly ExecuteFunction PassiveCollisionFunctions;
		
		protected CollisionComponent(
			GameObject gameObject,
			Model3D model,
			ExecuteFunction passiveCollisionFunctions = null,
			ExecuteFunction activeCollisionFunctions = null,
			PhysicsMaterial physicsMaterial = null
		) : base(gameObject) {
			model3D = model;
			aabb = model.AABB;
			
			PhysicsMaterial = physicsMaterial ?? new DefaultPhysicsMaterial();
			ActiveCollisionFunctions = activeCollisionFunctions;
			PassiveCollisionFunctions =  col =>{ };
			if (passiveCollisionFunctions != null) {
				PassiveCollisionFunctions = passiveCollisionFunctions;
			}
			
			
			
		}

		public override void Update() { }

		public abstract bool IsColliding(CollisionComponent otherCollider);

		public AxisAlignedBoundingBox GetAABB() => aabb;
		public Sphere GetBoundingSphere() => new Sphere(GameObject.TransformComponent.WorldPosition, GameObject.Radius);
	}
}