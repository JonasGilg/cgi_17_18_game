using System.Collections.Generic;
using Player = System.Media.SoundPlayer;

namespace Engine.Sound {
	public static class SoundPlayer {
		private static readonly Dictionary<int, Player> SOUNDS = new Dictionary<int, Player>();
		private static int counter;

		public static int LoadSound(string path) {
			var sound = new Player(path);
			var id = counter++;
			SOUNDS[id] = sound;
			sound.Load();
			return id;
		}

		public static void DeleteSound(int id) {
			var sound = SOUNDS[id];
			SOUNDS.Remove(id);
			sound.Dispose();
		}

		public static void Play(int id) => SOUNDS[id].Play();
		public static void PlayLooping(int id) => SOUNDS[id].PlayLooping();
		public static void Stop(int id) => SOUNDS[id].Stop();
	}
}