using System.Collections.Generic;

namespace Engine.Collision {
    public static class CollisionEngine {
        private static readonly List<CollisionComponent> COLLISION_COMPONENTS = new List<CollisionComponent>();
        
        public static void Register(CollisionComponent component) => COLLISION_COMPONENTS.Add(component);

        public static void Unregister(CollisionComponent component) => COLLISION_COMPONENTS.Remove(component);
        
        public static void CheckCollisions() {
            for (int i = 0; i < COLLISION_COMPONENTS.Count; i++) {
                var currObj = COLLISION_COMPONENTS[i];
                for (var j = 0; j < COLLISION_COMPONENTS.Count; j++) {
                    if (i != j) {
                        //cant collide with yourself
                        var collidedWith = COLLISION_COMPONENTS[j];
                        if (currObj.IsColliding(collidedWith)) {
                            currObj.OnCollision(new Collision {
                                otherGameObject = collidedWith.GameObject,
                                OtherCollisonComponent = collidedWith
                            });
                        }
                    }
                }
            }
        }
        
    }
}