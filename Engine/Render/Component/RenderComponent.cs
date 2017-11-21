using Engine.Material;
using Engine.Model;
using Engine.Util;

namespace Engine.Component {
	public class RenderComponent : Component, IOctreeItem {
		private static int idCounter;
		private readonly int id = idCounter++;
		
		public readonly Model3D Model;
		public AxisAlignedBoundingBox AABB;
		public BaseMaterial Material;
		public readonly MaterialSettings MaterialSettings;

		public RenderComponent(Model3D model, BaseMaterial material, MaterialSettings materialSettings, GameObject gameObject) : base(gameObject) {
			Model = model;
			AABB = Model.AABB * gameObject.TransformComponent.Scale;
			Material = material;

			MaterialSettings = materialSettings;
		}

		public override void Update() {
			/*RenderEngine.UnregisterRenderComponent(this);
			RenderEngine.RegisterRenderComponent(this);*/
		}

		public AxisAlignedBoundingBox GetAABB() => AABB + GameObject.TransformComponent.WorldPosition;
		public Sphere GetBoundingSphere() => new Sphere(GameObject.TransformComponent.WorldPosition, GameObject.Radius);

		public override int GetHashCode() => id;
	}
}