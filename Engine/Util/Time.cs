using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.Util {
	public static class Time {
		public static double DeltaTime { get; private set; }

		public static void Update(double deltaTime) {
			DeltaTime = deltaTime;
		}
	}

	public class TimingStats {
		public readonly string Name;
		
		private readonly Queue<double> _lastFrameTimes = new Queue<double>(10);

		public TimingStats(string name) {
			Name = name;
			_lastFrameTimes.Enqueue(0);
		}

		public double GetAverage() {
			return _lastFrameTimes.Average();
		}

		public double GetLast() {
			return _lastFrameTimes.Last();
		}
		
		private readonly Stopwatch _stopwatch = new Stopwatch();

		public void Start() {
			_stopwatch.Restart();
		}

		public void Stop() {
			_stopwatch.Stop();
			_lastFrameTimes.Enqueue(_stopwatch.ElapsedMilliseconds);
			if (_lastFrameTimes.Count > 10) {
				_lastFrameTimes.Dequeue();
			}
		}

		public override string ToString() => $"\t{Name}: {GetAverage().ToString()}ms";
	}
}