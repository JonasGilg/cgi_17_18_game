using System.Threading.Tasks;
using Engine.Component;
using Engine.GUI;
using Engine.Render;

namespace Engine {
	public abstract class GameObject {
		public readonly TransformComponent TransformComponent;
		public double Radius;
		public Sphere BoundingSphere() => new Sphere(TransformComponent.WorldPosition, Radius);
		
		
		protected GameObject() {
			TransformComponent = new TransformComponent(this);
			Radius = 0;
		}
		
		public virtual void Awake() { }

		public virtual void Update() => TransformComponent.Update();
		
		

		/// <summary>
		/// used to clean up the object (aka Destructor)
		/// </summary>
		/*public static void Destroy(GameObject obj) {
			obj.OnDestroy();
			World.RemoveFromWorld(this);
		}*/

		public static async void Destroy(GameObject obj,int millisDelay = 0) {
			await Task.Delay(millisDelay);
			obj.OnDestroy();
			World.RemoveFromWorld(obj);
		}
		
		
		/// <summary>
		/// hook method to implement behaviour happening at destruction time
		/// </summary>
		protected virtual void OnDestroy() { }

		public override string ToString() => GetType().Name + "(#" + GetHashCode().ToString() + ")";
	}
}