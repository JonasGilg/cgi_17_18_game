using System.Collections.Generic;

namespace Engine.Material {
    public static class PostProcessing {
        public static List<PostProcessingMaterial> Effects;

        static PostProcessing() {
            Effects = new List<PostProcessingMaterial>();
            //Effects.Add(new TestEffect());
        }
        public static void DrawMaterials() {
            foreach (var material in Effects) {
                material.Draw();
            }
        }
    }
}