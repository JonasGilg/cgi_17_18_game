using OpenTK;

namespace Engine.Collision {
	public class CollisionMessage {
		public CollisionComponent OtherCollisonComponent;

		
		public CollisionMessage(CollisionComponent other) {
			OtherCollisonComponent = other;
		}
		
		
	}
}