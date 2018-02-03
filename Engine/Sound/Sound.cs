using Player = System.Media.SoundPlayer;

namespace Engine.Sound {
	public class Sound {
		private readonly CachedSound cachedSound;

		public Sound(string fileName) => cachedSound = new CachedSound(fileName);

		public void Play() => SoundEngine.INSTANCE.PlaySound(cachedSound);
	}
}