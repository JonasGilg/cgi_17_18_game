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

	public static class TimingRegistry {
		private static readonly List<TimingStats> UpdateTimings = new List<TimingStats>();
		private static readonly List<TimingStats> RenderTimings = new List<TimingStats>();

		public static void AddUpdateTiming(TimingStats timingStats) {
			UpdateTimings.Add(timingStats);
		}

		public static void AddRenderTiming(TimingStats timingStats) {
			RenderTimings.Add(timingStats);
		}

		public static string GetStatsText() {
			var s = "\n### Update ###\n";
			s = UpdateTimings.Aggregate(s, (current, t) => current + t.ToString() + "\n");
			s += "\n### Render ###\n";
			return RenderTimings.Aggregate(s, (current, t) => current + t.ToString() + "\n");
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