using System.Collections.Generic;
using Engine;
using Engine.Component;

namespace Game.GameObjects {
    public class MetalChunkShape : GameObject {
        public List<MetalChunk> Chunks;

        public MetalChunkShape(List<MetalChunk> chunks) {
            Chunks = chunks;
        }

        public MetalChunkShape() {
            Chunks = new List<MetalChunk>();
        }
    }
}