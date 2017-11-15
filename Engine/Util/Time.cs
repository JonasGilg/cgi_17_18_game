using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.Util {
	public static class Time {
		private const int QUEUE_SIZE = 30;

		public static double GameSpeed { get; private set; } = 1.0;

		private static double deltaTimeUpdate;

		public static double DeltaTimeUpdate {
			get => deltaTimeUpdate * GameSpeed;
			private set => deltaTimeUpdate = value;
		}

		private static double deltaTimeRender;

		public static double DeltaTimeRender {
			get => deltaTimeRender * GameSpeed;
			private set => deltaTimeRender = value;
		}

		private static readonly double START_TIME;

		private static double CurrentTime => DateTime.Now.ToUniversalTime()
			.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

		public static double TotalTime => CurrentTime - START_TIME;

		private static readonly Queue<double> LAST_UPDATES = new Queue<double>(QUEUE_SIZE);
		private static readonly Queue<double> LAST_RENDERS = new Queue<double>(QUEUE_SIZE);

		public static double AverageUpdateTime() => LAST_UPDATES.Average();
		public static double AverageRenderTime() => LAST_RENDERS.Average();

		static Time() {
			START_TIME = CurrentTime;
		}

		public static void UpdateUpdateTime(double deltaTime) {
			DeltaTimeUpdate = deltaTime;

			LAST_UPDATES.Enqueue(deltaTime);
			if (LAST_UPDATES.Count > QUEUE_SIZE) {
				LAST_UPDATES.Dequeue();
			}
		}

		public static void UpdateRenderTime(double deltaTime) {
			DeltaTimeRender = deltaTime;

			LAST_RENDERS.Enqueue(deltaTime);
			if (LAST_RENDERS.Count > QUEUE_SIZE) {
				LAST_RENDERS.Dequeue();
			}
		}

		public static void IncreaseGameSpeed() => GameSpeed *= 1.1;
		public static void DecreaseGameSpeed() => GameSpeed /= 1.1;
	}

	public static class TimingRegistry {
		private static readonly List<TimingStats> UPDATE_TIMINGS = new List<TimingStats>();
		private static readonly List<TimingStats> RENDER_TIMINGS = new List<TimingStats>();

		public static void AddUpdateTiming(TimingStats timingStats) => UPDATE_TIMINGS.Add(timingStats);
		public static void AddRenderTiming(TimingStats timingStats) => RENDER_TIMINGS.Add(timingStats);

		public static string GetStatsText() {
			var s = "\n### Update ###\n";
			s = UPDATE_TIMINGS.Aggregate(s, (current, t) => current + t.ToString() + "\n");
			s += "\n### Render ###\n";
			return RENDER_TIMINGS.Aggregate(s, (current, t) => current + t.ToString() + "\n");
		}
	}

	public class TimingStats {
		private const int QUEUE_SIZE = 10;

		public readonly string Name;

		private readonly Queue<double> lastFrameTimes = new Queue<double>(QUEUE_SIZE);

		public TimingStats(string name) {
			Name = name;
			lastFrameTimes.Enqueue(0);
		}

		public double GetAverage() => lastFrameTimes.Average();

		private readonly Stopwatch stopwatch = new Stopwatch();

		public void Start() {
#if(DEBUG)
			stopwatch.Restart();
#endif
		}

		public void Stop() {
#if(DEBUG)
			stopwatch.Stop();
			lastFrameTimes.Enqueue(stopwatch.ElapsedTicks);
			if (lastFrameTimes.Count > QueueSize) {
				lastFrameTimes.Dequeue();
			}
#endif
		}

		public override string ToString() => $"\t{Name}: {GetAverage():N2} clocks";
	}
}