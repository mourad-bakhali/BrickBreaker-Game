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
    public class NormalObject : ReleasedObject
    {
        // Fields
        EntityText text;
        bool hitWidthPaddle = false;
        // Properties



        public NormalObject(Model Model, Vector3 Position)
            : base(Model, Position)
        {
        }

        public NormalObject(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
        }

        protected override void Setup(Model Model, Vector3 Position)
        {
            base.Setup(Model, Position);
            this.Scale = new Vector3(0.125f / 1.3f);
            this.defaultLightingEnabled = true;
            this.Position += new Vector3(0f, 2f, 0f);
            releaseType = ReleaseType.Normal;

            icon = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\scoresIcon"),
                    GraphicsUtil.GetProjectPoint(Position), this.Parent);
            icon.Visible = false;

            text = new EntityText(Engine.Content.Load<SpriteFont>("Content\\Fonts\\FontAgency24Or"),
                                GraphicsUtil.GetProjectPoint(Position), "+100", this.Parent);
            text.Visible = false;
        }

        public override void Update()
        {
            if (release)
            {
                Rotation *= Matrix.CreateRotationY(0.1f);
                if (hitWidthPaddle||hitWithPaddleSupport)
                {
                    text.Position -= new Vector2(0f, 1f);
                    text.Alpha -= 0.02f;
                }
                base.Update();
            }
        }

        public override void DisableComponent()
        {
            text.DisableComponent();
            icon.DisableComponent();
            base.DisableComponent();
        }

        public override void fadeAway()
        {
            base.fadeAway();
        }

        public override void hitPaddle()
        {
            base.hitPaddle();
            text.Position = GraphicsUtil.GetProjectPoint(Position);
            text.Visible = true;
            text.Text = "+200";
            hitWidthPaddle = true;
        }
        public override void hitPaddleSupport()
        {
            text.Position = GraphicsUtil.GetProjectPoint(Position);
            text.Visible = true;
            text.Text = "+10";
            base.hitPaddleSupport();
        }
    }
}

