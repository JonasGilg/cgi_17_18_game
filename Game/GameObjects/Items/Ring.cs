using System;
using Engine;
using Engine.Collision;
using Engine.Component;
using Engine.Material;
using Engine.Model;
using Engine.Render;
using Engine.Texture;
using Engine.Util;
using Game.GamePlay;
using OpenTK;

namespace Game.GameObjects {
    
    public abstract class Ring : Item {
        protected static readonly Model3D Model = ModelLoaderObject3D.Load("data/objects/SupplyRing.obj");
       
    }
}