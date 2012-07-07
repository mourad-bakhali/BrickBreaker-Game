using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PlutoEngine;

namespace BrickBreaker
{
    public class Ball : Entity3D
    {
        // Fields
        float velocityX;
        float velocityZ;
        float ballRaduis;
        float speedFactor;
        Entity3D radialShadow;
        SoundEffect sound1;
        // Properties
        public float VelocityX
        {
            get { return velocityX; }
            set { velocityX = value; }
        }
        public float VelocityZ
        {
            get { return velocityZ; }
            set { velocityZ = value; }
        }
        public float BallRaduis
        {
            get { return ballRaduis; }
            set { ballRaduis = value; }
        }

        public override BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(Position - (new Vector3(ballRaduis) / 2),
                    Position + (new Vector3(ballRaduis) / 2));
            }
        }

        public Ball(Model Model, Vector3 Position)
            : base(Model, Position)
        {
        }
        public Ball(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
        }
        protected override void Setup(Model Model, Vector3 Position)
        {
            base.Setup(Model, Position);
            sound1 = Engine.Content.Load<SoundEffect>("Content\\Sounds\\ballHit");
            this.Scale = new Vector3(0.1f);
            this.Position += new Vector3(0f, 0.5f, 0f);
            speedFactor = GlobalVariables.speedFactor;
            this.velocityZ = speedFactor;
            this.velocityX = velocityZ / 2;
            this.ballRaduis = 1f;
            radialShadow = new Entity3D(Engine.Content.Load<Model>("Content\\Models\\RadialShadow"),
                        new Vector3(this.Position.X, 0.1f, this.Position.Z), this.Parent);
            radialShadow.Scale = new Vector3(0.5f);
        }
        public override void Update()
        {
            this.Position += new Vector3(velocityX, 0, velocityZ);
            radialShadow.Position = new Vector3(this.Position.X, 0.1f, this.Position.Z);
            base.Update();
        }
        public void playSound()
        {
            sound1.Play();
        }
    }
}

