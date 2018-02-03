namespace Engine.Collision {
	public class CollisionMessage {
		public readonly CollisionComponent OtherCollisonComponent;

		public CollisionMessage(CollisionComponent other) {
			OtherCollisonComponent = other;
		}
	}
}