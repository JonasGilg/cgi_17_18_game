using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;

namespace Engine.Util {
	public static class Time {
		private const int QueueSize = 30;
		
		public static double GameSpeed { get; private set; } = 1.0;

		private static double _deltaTimeUpdate;
		public static double DeltaTimeUpdate {
			get => _deltaTimeUpdate * GameSpeed;
			private set => _deltaTimeUpdate = value;
		}

		private static double _deltaTimeRender;
		public static double DeltaTimeRender {
			get => _deltaTimeRender * GameSpeed;
			private set => _deltaTimeRender = value;
		}

		private static readonly double StartTime;
		private static double CurrentTime => DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		
		public static double TotalTime => CurrentTime - StartTime;

		private static readonly Queue<double> LastUpdates = new Queue<double>(QueueSize);
		private static readonly Queue<double> LastRenders = new Queue<double>(QueueSize);

		public static double AverageUpdateTime() => LastUpdates.Average();
		public static double AverageRenderTime() => LastRenders.Average();

		static Time() {
			StartTime = CurrentTime;
		}

		public static void UpdateUpdateTime(double deltaTime) {
			DeltaTimeUpdate = deltaTime;
			
			LastUpdates.Enqueue(deltaTime);
			if (LastUpdates.Count > QueueSize) {
				LastUpdates.Dequeue();
			}
		}

		public static void UpdateRenderTime(double deltaTime) {
			DeltaTimeRender = deltaTime;
			
			LastRenders.Enqueue(deltaTime);
			if (LastRenders.Count > QueueSize) {
				LastRenders.Dequeue();
			}
		}

		public static void IncreaseGameSpeed() => GameSpeed *= 1.1;
		public static void DecreaseGameSpeed() => GameSpeed /= 1.1;
	}

	public static class TimingRegistry {
		private static readonly List<TimingStats> UpdateTimings = new List<TimingStats>();
		private static readonly List<TimingStats> RenderTimings = new List<TimingStats>();

		public static void AddUpdateTiming(TimingStats timingStats) => UpdateTimings.Add(timingStats);
		public static void AddRenderTiming(TimingStats timingStats) => RenderTimings.Add(timingStats);

		public static string GetStatsText() {
			var s = "\n### Update ###\n";
			s = UpdateTimings.Aggregate(s, (current, t) => current + t.ToString() + "\n");
			s += "\n### Render ###\n";
			return RenderTimings.Aggregate(s, (current, t) => current + t.ToString() + "\n");
		}
	}

	public class TimingStats {
		private const int QueueSize = 10;
		
		public readonly string Name;

		private readonly Queue<double> _lastFrameTimes = new Queue<double>(QueueSize);

		public TimingStats(string name) {
			Name = name;
			_lastFrameTimes.Enqueue(0);
		}

		public double GetAverage() => _lastFrameTimes.Average();

		private readonly Stopwatch _stopwatch = new Stopwatch();

		public void Start() {
#if(DEBUG)
			_stopwatch.Restart();
#endif
		}

		public void Stop() {
#if(DEBUG)
			_stopwatch.Stop();
			_lastFrameTimes.Enqueue(_stopwatch.ElapsedTicks);
			if (_lastFrameTimes.Count > QueueSize) {
				_lastFrameTimes.Dequeue();
			}
#endif
		}

		public override string ToString() => $"\t{Name}: {GetAverage():N2} clocks";
	}
}