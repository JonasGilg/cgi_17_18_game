﻿using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;

namespace Engine.Sound {
	internal class CachedSound {
		internal readonly float[] AudioData;
		internal readonly WaveFormat WaveFormat;

		internal CachedSound(string audioFileName) {
			using (var audioFileReader = new AudioFileReader(audioFileName)) {
				WaveFormat = audioFileReader.WaveFormat;
				var wholeFile = new List<float>((int) (audioFileReader.Length / 4));
				var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
				int samplesRead;
				while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0) {
					wholeFile.AddRange(readBuffer.Take(samplesRead));
				}

				AudioData = wholeFile.ToArray();
			}
		}
	}
}