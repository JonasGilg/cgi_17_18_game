using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Component;
using Engine.GUI;
using Engine.Render;

namespace Engine {
	public abstract class GameObject {
		public TransformComponent TransformComponent;
		public double Radius;
		public Sphere BoundingSphere() => new Sphere(TransformComponent.WorldPosition, Radius);

		public Dictionary<ComponentType, List<Component.Component>> optionalComponents = new Dictionary<ComponentType, List<Component.Component>>();
		
		public readonly HudObjectMarker objectMarker;
		
		protected GameObject() {
			TransformComponent = new TransformComponent(this);
			Radius = 0;
			
			objectMarker =HUD.CreateHudObjectMarker(this);
			
		}
		
		public virtual void Awake() { }

		public virtual void Update() => TransformComponent.Update();

		public static void Instantiate(GameObject obj) {
			obj.Awake();
			World.AddToWorld(obj);
		}

		public static async void Destroy(GameObject obj,int millisDelay = 0) {
			await Task.Delay(millisDelay);
			obj.OnDestroy();
			World.RemoveFromWorld(obj);
		}

		public bool searchOptionalComponents(ComponentType type, out List<Component.Component> resultList) {
			if (!optionalComponents.TryGetValue(type, out resultList)) {
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