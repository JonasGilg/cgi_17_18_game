using System;
using Engine.GUI;
using Engine.Render;
using OpenTK;

namespace Engine {
    public static class Statistics {
        private static int Score = 0;
        public static readonly HudTextElement ScoreTextElement = HUD.CreateHudTextElement("SCORE : 0", new Vector2(-1f, 0.82f));

        public static readonly HudTextElement TimeSpentTextElement = HUD.CreateHudTextElement("TIME SPENT : 0", new Vector2(-1f, 0.88f));

        public static bool updatingTime = true;
        public static bool scoringPoints = true;

        static Statistics() {
            HUD.AddHudTextElement(ScoreTextElement);
            HUD.AddHudTextElement(TimeSpentTextElement);
        }

        public static void IncreaseScore(int points = 1) {
            if (scoringPoints) {
                Score += points;
                ScoreTextElement.Text = $"SCORE : {Score}";
            }
        }

        public static void Stop() {
            updatingTime = false;
            scoringPoints = false;
        }

        public static void UpdateTimeSpent() {
            if (updatingTime) {
                TimeSpentTextElement.Text = $"TIME SPENT : {TimeSpan.FromSeconds(Time.TotalTime / 1000).ToString(@"hh\:mm\:ss")}";
            }
        }
    }
}