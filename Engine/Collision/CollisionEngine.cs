using System.Collections.Generic;

namespace Engine.Collision {
    public static class CollisionEngine {
        
        private static readonly List<CollisionComponent> ACTIVE_COLLISION_COMPONENTS = new List<CollisionComponent>();
        
        //passive list holds EVERY Component
        private static readonly List<CollisionComponent> PASSIVE_COLLISION_COMPONENTS = new List<CollisionComponent>();
        
        

        public static void Register(CollisionComponent component) {
            
            PASSIVE_COLLISION_COMPONENTS.Add(component);

            if (component.ActiveCollisionFunctions != null) {
                ACTIVE_COLLISION_COMPONENTS.Add(component);
            }
            
            
        }

        public static void Unregister(CollisionComponent component) {
            PASSIVE_COLLISION_COMPONENTS.Remove(component);
            ACTIVE_COLLISION_COMPONENTS.Remove(component);
        }
        
        public static void CheckCollisions() {

            for (int i = 0; i < ACTIVE_COLLISION_COMPONENTS.Count; i++) {
                CollisionComponent activeComponent = ACTIVE_COLLISION_COMPONENTS[i];
                CollisionComponent passiveComponent;
                for (int j = 0; j < PASSIVE_COLLISION_COMPONENTS.Count; j++) {
                    passiveComponent = PASSIVE_COLLISION_COMPONENTS[j];
                    if (!activeComponent.Equals(passiveComponent) && activeComponent.IsColliding(passiveComponent)) {

                        //AKTIVE
                        activeComponent.ActiveCollisionFunctions(new CollisionMessage(passiveComponent));
                        //PASSIVE
                        passiveComponent.PassiveCollisionFunctions(new CollisionMessage(activeComponent));
                        
                        
                    }
                }
            }
            
            
            
            
            
        }
        
    }
}