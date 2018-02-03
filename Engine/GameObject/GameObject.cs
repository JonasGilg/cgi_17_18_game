using System.Collections.Generic;
using Engine.Component;
using Engine.GUI;
using Engine.Render;

namespace Engine {
	public abstract class GameObject {
		public readonly TransformComponent TransformComponent;
		public double Radius;
		public Sphere BoundingSphere() => new Sphere(TransformComponent.WorldPosition, Radius);

		public readonly Dictionary<ComponentType, List<Component.Component>> OptionalComponents = new Dictionary<ComponentType, List<Component.Component>>();

		public readonly HudObjectMarker ObjectMarker;

		protected GameObject() {
			TransformComponent = new TransformComponent(this);
			Radius = 0;

			ObjectMarker = HUD.CreateHudObjectMarker(this);
		}

		public virtual void Awake() { }

		public virtual void Update() => TransformComponent.Update();

		public void Instantiate() {
			Awake();
			World.AddToWorld(this);
		}

		public void Destroy() {
			OnDestroy();
			World.RemoveFromWorld(this);
		}

		public bool SearchOptionalComponents(ComponentType type, out List<Component.Component> resultList) {
			if (!OptionalComponents.TryGetValue(type, out resultList)) {
				return false;
			}

			return resultList.Count > 0;
		}

		/// <summary>
		/// hook method to implement behaviour happening at destruction time
		/// </summary>
		protected virtual void OnDestroy() { }

		public override string ToString() => GetType().Name + "(#" + GetHashCode().ToString() + ")";
	}
}