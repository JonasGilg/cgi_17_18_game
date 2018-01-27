using System.Drawing;

namespace Engine.GUI {
	public class HudObjectMarker {
		public readonly GameObject GameObject;
		public static readonly double minSize = 50.0;
		public readonly int id;

		public HudObjectMarker(int id, GameObject gameObject) {
			this.id = id;
			GameObject = gameObject;
			
		}
	
		


	}
}