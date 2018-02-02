using System;
using System.IO;
using OpenTK.Audio.OpenAL;

namespace Engine.Sound {
	public static class SoundLoader {
		public static Sound LoadSound(string file) {
			var buffer = AL.GenBuffer();
			var source = AL.GenSource();

			var soundData = LoadWave(File.Open(file, FileMode.Open), out var channels, out var bitsPerSample, out var sampleRate);
			AL.BufferData(buffer, GetSoundFormat(channels, bitsPerSample), soundData, soundData.Length, sampleRate);

			AL.Source(source, ALSourcei.Buffer, buffer);
			
			return new Sound(buffer, source);
		}
		
		private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			using (var reader = new BinaryReader(stream)) {
				var signature = new string(reader.ReadChars(4));
				if (signature != "RIFF")
					throw new NotSupportedException("Specified stream is not a wave file.");

				var riffChunckSize = reader.ReadInt32();

				var format = new string(reader.ReadChars(4));
				if (format != "WAVE")
					throw new NotSupportedException("Specified stream is not a wave file.");

				var formatSignature = new string(reader.ReadChars(4));
				if (formatSignature != "fmt ")
					throw new NotSupportedException("Specified wave file is not supported.");

				var formatChunkSize = reader.ReadInt32();
				var audioFormat = reader.ReadInt16();
				var numChannels = reader.ReadInt16();
				var sampleRate = reader.ReadInt32();
				var byteRate = reader.ReadInt32();
				var blockAlign = reader.ReadInt16();
				var bitsPerSample = reader.ReadInt16();

				var dataSignature = new string(reader.ReadChars(4));
				if (dataSignature != "data")
					throw new NotSupportedException("Specified wave file is not supported.");

				var dataChunkSize = reader.ReadInt32();

				channels = numChannels;
				bits = bitsPerSample;
				rate = sampleRate;

				return reader.ReadBytes((int) reader.BaseStream.Length);
			}
		}

		private static ALFormat GetSoundFormat(int channels, int bits) {
			switch (channels) {
				case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
				case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
				default: throw new NotSupportedException("The specified sound format is not supported.");
			}
		}
	}
}