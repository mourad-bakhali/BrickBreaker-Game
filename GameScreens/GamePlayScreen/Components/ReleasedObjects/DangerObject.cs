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
    public class DangerObject : ReleasedObject
    {
        // Fields
        int angle=0;
        EntityText text;
        bool hitWidthPaddle = false;

        // Properties

        public DangerObject(Model Model, Vector3 Position)
            : base(Model, Position)
        {
        }

        public DangerObject(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
        }

        protected override void Setup(Model Model, Vector3 Position)
        {   

            base.Setup(Model, Position);
            this.Scale = new Vector3(0.125f / 1.3f);
            this.Position += new Vector3(0f, 2f, 0f);
            this.defaultLightingEnabled = true;
            releaseType = ReleaseType.Danger;

            text = new EntityText(Engine.Content.Load<SpriteFont>("Content\\Fonts\\FontAgency24Or"),
                    GraphicsUtil.GetProjectPoint(Position), "Good !", this.Parent);
            text.Visible = false;
        }

        public override void Update()
        {
            if (release)
            {
                if (hitWidthPaddle||hitWithPaddleSupport)
                {
                    text.Position -= new Vector2(0f, 1f);
                    text.Alpha -= 0.02f;
                }
                angle += 10;
                float rotZ = (float)Math.Cos((double)MathHelper.ToRadians((float)angle));
                Rotation *= Matrix.CreateRotationY(rotZ * 0.1f);
                base.Update();
            }
        }
        public override void hitPaddle()
        {
            base.hitPaddle();
            text.Position = GraphicsUtil.GetProjectPoint(Position);
            text.Visible = true;
            text.Text = "+10";
            hitWidthPaddle = true;
        }

        public override void DisableComponent()
        {
            text.DisableComponent();
            base.DisableComponent();
        }
        public override void hitPaddleSupport()
        {
            text.Position = GraphicsUtil.GetProjectPoint(Position);
            text.Visible = true;
            text.Text = "-5";
            base.hitPaddleSupport();
        }
    }
}

