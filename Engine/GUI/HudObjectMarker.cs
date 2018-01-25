using System.Drawing;

namespace Engine.GUI {
	public class HudObjectMarker {
		public readonly GameObject GameObject;
		public static readonly double minSize;
		public readonly int textureId;
		public readonly int id;

		public HudObjectMarker(int id, GameObject gameObject, int textureId) {
			this.id = id;
			GameObject = gameObject;
			this.textureId = textureId;
		}
	
		


	}
}