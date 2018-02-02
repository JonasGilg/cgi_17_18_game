using System.Media;

namespace Engine.Sound {
	public static class Soundtrack {
		private static SoundPlayer player = new SoundPlayer();

		public static void PlaySoundTrack(string file) {
			Stop();
			player = new SoundPlayer(file);
			player.PlayLooping();
		}

		public static void Stop() {
			player.Stop();
			player.Dispose();
		}
	}
}