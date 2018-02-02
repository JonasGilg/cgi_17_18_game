using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using Player = System.Media.SoundPlayer;

namespace Engine.Sound {
	public class Sound : IDisposable {
		private readonly int buffer;
		private readonly int source;
		private bool stopped;

		internal Sound(int buffer, int source) {
			this.buffer = buffer;
			this.source = source;
		}

		public void Play() {
			stopped = false;
			AL.SourcePlay(source);
		}

		public void PlayLooping() {
			Play();
			Task.Run(() => {
				ALSourceState state;
				do {
					state = AL.GetSourceState(source);
				} while (state == ALSourceState.Playing);

				if (!stopped)
					PlayLooping();
			});
		}

		public void Stop() {
			stopped = true;
			AL.SourceStop(source);
		}

		public void Dispose() {
			AL.DeleteSource(source);
			AL.DeleteBuffer(buffer);
		}
	}
}