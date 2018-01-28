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
		public virtual void Destroy() {
			OnDestroy();
		}
		
		
		/// <summary>
		/// hook method to implement behaviour happening at destruction time
		/// </summary>
		public virtual void OnDestroy() { }

		public override string ToString() => GetType().Name + "(#" + GetHashCode().ToString() + ")";
	}
}