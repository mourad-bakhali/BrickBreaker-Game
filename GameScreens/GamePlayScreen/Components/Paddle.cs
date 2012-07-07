using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlutoEngine;

namespace BrickBreaker
{
    public class Paddle : Entity3D
    {
        // Fields
        float velocityX;
        Entity3D squareShadow;

        public float VelocityX
        {
            get { return velocityX; }
            set { velocityX = value; }
        }
        public override BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(Position - new Vector3(1f, 1f, 0.5f),
                                        Position + new Vector3(1f, 1f, 0.25f));
            }
        }

        public Paddle(Model Model, Vector3 Position)
            : base(Model, Position)
        {
        }
        public Paddle(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
        }
        protected override void Setup(Model Model, Vector3 Position)
        {
            base.Setup(Model, Position);
            this.Scale = new Vector3(2f, 0.6f, 1f);
            this.defaultLightingEnabled = false;
            this.Position += new Vector3(0f, 0.6f, 8.8f);
            this.VelocityX = 0.2f;

            squareShadow = new Entity3D(Engine.Content.Load<Model>("Content\\Models\\SquareShadow"),
                          new Vector3(Position.X, 0.1f, Position.Z), this.Parent);
            squareShadow.Scale = new Vector3(1.2f,1f,0.6f);
            squareShadow.Alpha = 0.8f;
        }

        public override void Update()
        {
            squareShadow.Position = new Vector3(Position.X, 0.1f, Position.Z);
            velocityX *= 0.98f;
            base.Update();
        }
    }
}

