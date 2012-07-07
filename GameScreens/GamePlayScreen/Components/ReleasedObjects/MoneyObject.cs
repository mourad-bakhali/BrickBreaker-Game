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
    public class MoneyObject : ReleasedObject
    {
        // Fields
        public EntityText text;
        bool hitWidthPaddle = false;
        // Properties


        public MoneyObject(Model Model, Vector3 Position)
            : base(Model, Position)
        {
        }

        public MoneyObject(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
        }

        protected override void Setup(Model Model, Vector3 Position)
        {
            base.Setup(Model, Position);
            shadow.Scale += new Vector3(0.5f, 0f, 0f);
            Alpha = 1f;
            this.Position += new Vector3(0f, 2f, 0f);
            this.defaultLightingEnabled = true;
            this.Scale = new Vector3(1f,1f,2f);
            releaseType = ReleaseType.Money;

            icon = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\moneyIcon"),
                                GraphicsUtil.GetProjectPoint(Position), this.Parent);
            icon.Visible = false;

            text = new EntityText(Engine.Content.Load<SpriteFont>("Content\\Fonts\\FontAgency24Or"),
                                GraphicsUtil.GetProjectPoint(Position), "Money !", this.Parent);
            text.Visible = false;
        }

        public override void Update()
        {
            if (release)
            {
                shadow.Rotation *= Matrix.CreateRotationY(0.1f);
                Rotation *= Matrix.CreateRotationY(0.1f);
                if (hitWidthPaddle||hitWithPaddleSupport)
                {
                    text.Position -= new Vector2(0f, 1f);
                    text.Alpha -= 0.02f;
                }
                base.Update();
            }
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
            text.Text = "Money x 10";
            hitWidthPaddle = true;
        }

        public override void DisableComponent()
        {
            text.DisableComponent();
            icon.DisableComponent();
            base.DisableComponent();
        }
        public override void hitPaddleSupport()
        {
            text.Position = GraphicsUtil.GetProjectPoint(Position);
            text.Visible = true;
            text.Text = "+1";
            base.hitPaddleSupport();
        }
    }
}

