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
    public class PaddleSupport : Entity3D
    {
        // Fields
        bool isGettingHit;
        float hitDuration; // In millisecondes
        float hitTimer;
        Entity3D ss;

        public override BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(Position - new Vector3(8f, 2f, 0.25f),
                                        Position + new Vector3(8f, 2f, 0.25f));
            }
        }

        public PaddleSupport(Model Model, Vector3 Position)
            : base(Model, Position)
        {
        }
        public PaddleSupport(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
        }
        protected override void Setup(Model Model, Vector3 Position)
        {
            base.Setup(Model, Position);
            this.defaultLightingEnabled = false;
            Alpha = 1f;
            //this.Rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(90f));
            this.isGettingHit = false;
            this.hitDuration = 1000f;
            this.Visible = true;

            ss = new Entity3D(Engine.Content.Load<Model>("Content\\Models\\SquareShadow"),
                          new Vector3(Position.X, 0.2f, Position.Z), this.Parent);
            ss.Scale = new Vector3(16f, 1f, 0.2f);
        }

        public override void Update()
        {
            if (isGettingHit)
            {
                hitTimer += (float)Engine.GameTime.ElapsedGameTime.TotalMilliseconds;

                Alpha = (float)Util.random.NextDouble();

                if (hitTimer > hitDuration)
                {
                    isGettingHit = false;
                    Alpha = 1f;
                }
            }
            base.Update();
        }

        public void getHit()
        {
            isGettingHit = true;
            hitTimer = 0f;
        }
    }
}

