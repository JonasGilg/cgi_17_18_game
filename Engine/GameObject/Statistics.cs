using System;
using Engine.GUI;
using Engine.Render;
using OpenTK;

namespace Engine {
    public static class Statistics {
        private static int Score = 0;
        private static readonly HudTextElement _score = HUD.CreateHudTextElement("SCORE : 0", new Vector2(-1f, -0.82f));

        private static readonly HudTextElement _timeSpent = HUD.CreateHudTextElement("TIME SPENT : 0", new Vector2(-1f, 0.88f));

        static Statistics() {
            HUD.AddHudTextElement(_score);
            HUD.AddHudTextElement(_timeSpent);
        }

        public static void IncreaseScore(int points = 1) {
            Score += points;
            _score.Text = $"SCORE : {Score}";
        }
        
        public static void UpdateTimeSpent() => _timeSpent.Text = $"TIME SPENT : {TimeSpan.FromSeconds(Time.TotalTime /1000).ToString(@"hh\:mm\:ss")}";
    }
}