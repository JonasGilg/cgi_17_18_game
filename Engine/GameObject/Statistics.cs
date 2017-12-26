using System;
using Engine.GUI;
using Engine.Render;
using OpenTK;

namespace Engine {
    public static class Statistics {
        private static int Score = 0;
        private static readonly HUDElement _score = HUD.CreateHUDElement("SCORE : 0", new Vector2(-1f, -0.82f));

        private static readonly HUDElement _timeSpent = HUD.CreateHUDElement("TIME SPENT : 0", new Vector2(-1f, 0.88f));

        static Statistics() {
            HUD.AddHUDElement(_score);
            HUD.AddHUDElement(_timeSpent);
        }

        public static void IncreaseScore(int points = 1) {
            Score += points;
            _score.Text = $"SCORE : {Score}";
        }
        
        public static void UpdateTimeSpent() => _timeSpent.Text = $"TIME SPENT : {TimeSpan.FromSeconds(Time.TotalTime /1000).ToString(@"hh\:mm\:ss")}";
    }
}