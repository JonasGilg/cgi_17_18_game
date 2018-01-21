using System.Collections.Generic;
using Engine;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
    public static class MetalChunkFactory {
        //TODO

        public static MetalChunk GenerateSingle(Vector3d position, MetalType type, double scale = 10.0) {
            var chunk = new MetalChunk(MetalType.Bronze) {
                TransformComponent = {
                    Scale = new Vector3d(scale),
                    Position = position,
                    Orientation = Quaterniond.Identity
                }
            };
            World.AddToWorld(chunk);
            return chunk;
        }
        
        
        public static MetalChunk[] GenerateLine(Vector3d startposition, Vector3d endPosition, MetalType type, int count, double scale = 10.0 ) {
            var chunks = new MetalChunk[count];

            return chunks;
        }
        
        
        public static MetalChunk[] GenerateRing(Vector3d center, Vector3d rotation, MetalType type, int count, double scale = 10.0 ) {
            var chunks = new MetalChunk[count];

            return chunks;
        }
        
        
        public static MetalChunk[] GenerateEye(Vector3d center, Vector3d rotation, MetalType ringType, int ringCount, MetalType eyeType, double ringScale = 10.0, double eyeScale = 10.0 ) {
            var chunks = new MetalChunk[ringCount+1];

            return chunks;
        }
        
        
        public static MetalChunk[] GenerateOnPositions(Dictionary<Vector3d,MetalType> positionsAndTypes, double scale = 10.0 ) {
            var chunks = new MetalChunk[positionsAndTypes.Count];

            return chunks;
        }
    }
}