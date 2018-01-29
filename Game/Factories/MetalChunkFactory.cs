using System;
using System.Collections.Generic;
using Engine;
using Engine.Component;
using Engine.Render;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
    /// <summary>
    /// A Factory that can generate different configurations of metal chunks.
    /// </summary>
    public static class MetalChunkFactory {

        /// <summary>
        /// Generates a single metal chunk on a chosen location with specific type and scale.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static MetalChunk GenerateSingle(Vector3d position, MetalType type, double scale = 10.0) {
            var chunk = new MetalChunk(type) {
                TransformComponent = {
                    Scale = new Vector3d(scale),
                    Position = position,
                    Orientation = Quaterniond.Identity
                }
            };
            GameObject.Instantiate(chunk);
            return chunk;
        }
        
        /// <summary>
        /// Generates a line of MetalChunks from a start position to an end position.
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="scale"></param>
        /// <returns>
        /// Returns a list of the generated metal chunks.
        /// The returned list is empty if count is 0 oder less.
        /// </returns>
        public static List<MetalChunk> GenerateLine(Vector3d startPosition, Vector3d endPosition, MetalType type, int count, double scale = 10.0 ) {
            var chunks = new List<MetalChunk>();
            if (count < 1) return chunks; //nothing to generate if count is 0
            
            //calculate positions
            var positions = new List<Vector3d>();

            switch (count) {
                 case 1: // just use the position in the middle of the start and end position
                     positions.Add(Math3D.InMiddleOf(startPosition,endPosition));
                     break;
                 default: // use start and end positions and the positions on equal distances in between 
                     positions.Add(startPosition);

                     if (count > 2) { //this block is only useful when count is greater than 2
                         var equiDistance = (endPosition - startPosition) / (count - 1);
                         for (int i = 1 ; i < count-1; i++) {
                             positions.Add(startPosition + (equiDistance * i));
                         }
                     }
                     
                     positions.Add(endPosition);
                     break;
            }
            
            foreach (var pos in positions) {
                chunks.Add(GenerateSingle(pos, type, scale));
            }

            return chunks;
        }

        /// <summary>
        /// Generates a ring of metal chunks.
        /// The angle is measured in degrees!
        /// </summary>
        /// <param name="center"></param>
        /// <param name="eulerAngle"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="radius"></param>
        /// <param name="scale"></param>
        /// <returns>
        /// Returns a list of the generated metal chunks.
        /// The returned list is empty if count is 0 oder less.
        /// </returns>
        public static List<MetalChunk> GenerateRing(Vector3d center, Vector3d eulerAngle, MetalType type, int count, double radius, double scale = 10.0 ) {
            var chunks = new List<MetalChunk>();
            if (count < 1) return chunks; //nothing to generate if count is 0
            
            
            for (int i = 0; i < count; i++) {
                var pos = new Vector3d(radius * Math.Cos(i * Math.PI * 2 / count), radius * Math.Sin(i * Math.PI * 2 / count), center.Z);
                var rotatedPos = Quaterniond.FromEulerAngles(eulerAngle.ToRadiansVector3D()).Rotate(pos) + center;
                chunks.Add( GenerateSingle(rotatedPos, type, scale) );
            }
            
            var ring = new MetalChunkShape(chunks) {
                TransformComponent = {
                    Position = center
                }
            };

            return chunks;
        }

        /// <summary>
        /// Generates a ring of metal chunks with a chunk in its center.
        /// The angle is measured in degrees!
        /// </summary>
        /// <param name="center"></param>
        /// <param name="eulerAngle"></param>
        /// <param name="ringType"></param>
        /// <param name="ringCount"></param>
        /// <param name="radius"></param>
        /// <param name="eyeType"></param>
        /// <param name="ringScale"></param>
        /// <param name="eyeScale"></param>
        /// <returns>
        /// Returns a list of the generated metal chunks.
        /// The returned list is empty if count is 0 oder less.
        /// </returns>
        public static List<MetalChunk> GenerateEye(Vector3d center, Vector3d eulerAngle, MetalType ringType, int ringCount, double radius, MetalType eyeType, double ringScale = 10.0, double eyeScale = 10.0 ) {
            var chunks = new List<MetalChunk>();
            
            chunks.AddRange(GenerateRing(center,eulerAngle,ringType,ringCount,radius,ringScale));
            chunks.Add(GenerateSingle(center,eyeType,eyeScale));
            
            var eye = new MetalChunkShape(chunks) {
                TransformComponent = {
                    Position = center
                }
            };

            return chunks;
        }
        
        /// <summary>
        /// Generates metal chunks based on the position-MetalType pairs of the dictionary.
        /// </summary>
        /// <param name="positionsAndTypes"></param>
        /// <param name="scale"></param>
        /// <returns>
        /// Returns a list of the generated metal chunks.
        /// The returned list is empty if count is 0 oder less.
        /// </returns>
        public static List<MetalChunk> GenerateOnPositions(Dictionary<Vector3d,MetalType> positionsAndTypes, double scale = 10.0 ) {
            var chunks = new List<MetalChunk>();
            if (positionsAndTypes.Count < 1) return chunks;

            foreach (var positionAndType in positionsAndTypes) {
                chunks.Add(GenerateSingle(positionAndType.Key,positionAndType.Value,scale));
            }
            
            return chunks;
        }
    }
}