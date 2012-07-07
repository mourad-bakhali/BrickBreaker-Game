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
    public enum BrickReleaseType
    {
        Normal,
        Danger,
        Money,
    }

    public class Brick : Entity3D
    {
        // Fields
        float brickLenght;
        BrickReleaseType brickReleaseType;
        Entity3D squareShadow;
        int angle;
        public float forward = 0f;
        bool die = false;
        // Properties
        public float BrickLenght
        {
            get { return brickLenght; }
            set { brickLenght = value; }
        }
        public BrickReleaseType BrickReleaseType
        {
            get { return brickReleaseType; }
            set { brickReleaseType = value; }
        }
        public override BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(Position - (new Vector3(brickLenght) / 2),
                    Position + (new Vector3(brickLenght) / 2));
            }
        }

        public Brick(Model Model, Vector3 Position)
            : base(Model, Position)
        {
        }
        public Brick(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
        }
        protected override void Setup(Model Model, Vector3 Position)
        {
            base.Setup(Model, Position);
            this.Scale = new Vector3(0.125f);
            this.Position += new Vector3(0f, 1.2f, 0f);
            this.brickLenght = 1;
            brickReleaseType = BrickReleaseType.Normal;
            squareShadow = new Entity3D(Engine.Content.Load<Model>("Content\\Models\\SquareShadow"),
                                      new Vector3(Position.X, 0.2f, Position.Z), this.Parent);
            squareShadow.Scale = new Vector3(0.8f);
            squareShadow.Alpha = 0.5f;
            angle = Util.random.Next(0, 360);
        }
        public override void Update()
        {
            if (!die)
            {
                angle++;
                float upDownMov = (float)Math.Sin((double)MathHelper.ToRadians((float)angle * 5));
                Position += new Vector3(0f, upDownMov * (0.01f), 0);

                squareShadow.Position = new Vector3(Position.X, 0.2f, Position.Z);
            }
            else
            {
                this.Alpha -= 0.03f;
                this.Rotation *= Matrix.CreateRotationY(0.02f);
                this.Position += new Vector3(0f, 0.08f, 0f);
                squareShadow.Alpha -= 0.08f;
                if (Alpha <= 0f)
                {
                    DisableComponent();
                }
            }
            base.Update();
        }
        public override void DisableComponent()
        {
            squareShadow.DisableComponent();
            base.DisableComponent();
        }
        public void Die()
        {
            die = true;
        }
    }
}

