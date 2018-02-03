using System.Collections.Generic;

namespace Engine.Material {
    public static class PostProcessing {
        private static readonly List<PostProcessingMaterial> EFFECTS;

        static PostProcessing() => EFFECTS = new List<PostProcessingMaterial>();

        public static void DrawMaterials() {
            foreach (var material in EFFECTS) {
                material.Draw();
            }
        }
    }
}