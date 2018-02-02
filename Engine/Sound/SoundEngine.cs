using System;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Engine.Sound {
	internal class SoundEngine : IDisposable {
		private readonly IWavePlayer outputDevice;
		private readonly MixingSampleProvider mixer;

		private SoundEngine(int sampleRate = 44100, int channelCount = 2) {
			outputDevice = new WaveOutEvent();
			mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount)) {ReadFully = true};
			outputDevice.Init(mixer);
			outputDevice.Play();
		}

		private ISampleProvider ConvertToRightChannelCount(ISampleProvider input) {
			if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
				return input;

			if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
				return new MonoToStereoSampleProvider(input);
			
			throw new NotImplementedException("Not yet implemented this channel count conversion");
		}

		internal void PlaySound(CachedSound sound) => AddMixerInput(new CachedSoundSampleProvider(sound));

		private void AddMixerInput(ISampleProvider input) => mixer.AddMixerInput(ConvertToRightChannelCount(input));

		public void Dispose() => outputDevice.Dispose();

		internal static readonly SoundEngine INSTANCE = new SoundEngine(44100, 2);
	}
}