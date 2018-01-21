﻿using Engine.Collision;
using Engine.Model;
using Engine.Render;

namespace Engine {
	public delegate void Collisionhandler(Collision.Collision col);

	public abstract class CollisionComponent : Component.Component, IOctreeItem<CollisionComponent> {
		private readonly Model3D model3D;
		private readonly AxisAlignedBoundingBox aabb;
		
		public PhysicsMaterial PhysicsMaterial;
		public readonly Collisionhandler OnCollision;

		protected CollisionComponent(GameObject gameObject, Model3D model, Collisionhandler collisionFunction, PhysicsMaterial physicsMaterial = null) :
			base(gameObject) {
			model3D = model;
			aabb = model.AABB;
			
			OnCollision = collisionFunction;
			PhysicsMaterial = physicsMaterial ?? new DefaultPhysicsMaterial();
		}

		public override void Update() { }

		public void Register() {
			World.RegisterCollisionComponent(this);
		}

		public void Unregister() {
			World.UnregisterCollisionComponent(this);
		}

		public abstract bool IsColliding(CollisionComponent otherCollider);

		public AxisAlignedBoundingBox GetAABB() => aabb;
		public Sphere GetBoundingSphere() => new Sphere(GameObject.TransformComponent.WorldPosition, GameObject.Radius);
	}
}