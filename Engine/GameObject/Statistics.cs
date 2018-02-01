using System;
using Engine.GUI;
using Engine.Render;
using OpenTK;

namespace Engine {
    public static class Statistics {
        private static int Score = 0;
        public static readonly HudTextElement ScoreTextElement = HUD.CreateHudTextElement("[0]", new Vector2(0, 0.675f),TextAnchor.CENTER);

        public static readonly HudTextElement TimeSpentTextElement = HUD.CreateHudTextElement("", new Vector2(0, 0.775f),TextAnchor.CENTER);

        public static bool updatingTime = true;
        public static bool scoringPoints = true;

        static Statistics() {
            HUD.AddHudTextElement(ScoreTextElement);
            HUD.AddHudTextElement(TimeSpentTextElement);
        }

        public static void IncreaseScore(int points = 1) {
            if (scoringPoints) {
                Score += points;
                ScoreTextElement.Text = $"[{Score}]";
            }
        }

        public static void Stop() {
            updatingTime = false;
            scoringPoints = false;
        }

        public static void UpdateTimeSpent() {
            if (updatingTime) {
                TimeSpentTextElement.Text = $"{TimeSpan.FromSeconds(Time.TotalTime / 1000).ToString(@"hh\:mm\:ss")}";
            }
        }
    }
}