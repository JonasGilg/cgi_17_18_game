using System;
using Engine.GUI;
using Engine.Render;
using OpenTK;

namespace Engine {
    public static class Statistics {
        private static int score;
        public static readonly HudTextElement SCORE_TEXT_ELEMENT = HUD.CreateHudTextElement("[0]", new Vector2(0, 0.675f),TextAnchor.CENTER);
        public static readonly HudTextElement TIME_SPENT_TEXT_ELEMENT = HUD.CreateHudTextElement("", new Vector2(0, 0.775f),TextAnchor.CENTER);

        private static bool updatingTime = true;
        private static bool scoringPoints = true;

        static Statistics() {
            HUD.AddHudTextElement(SCORE_TEXT_ELEMENT);
            HUD.AddHudTextElement(TIME_SPENT_TEXT_ELEMENT);
        }

        public static void IncreaseScore(int points = 1) {
            if (scoringPoints) {
                score += points;
                SCORE_TEXT_ELEMENT.Text = $"[{score}]";
            }
        }

        public static void Stop() {
            updatingTime = false;
            scoringPoints = false;
        }

        public static void UpdateTimeSpent() {
            if (updatingTime) {
                TIME_SPENT_TEXT_ELEMENT.Text = $"{TimeSpan.FromSeconds(Time.TotalTime / 1000).ToString(@"hh\:mm\:ss")}";
            }
        }
    }
}