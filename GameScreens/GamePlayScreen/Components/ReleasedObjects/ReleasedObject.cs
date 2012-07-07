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
        public enum ReleaseType
        {
            Normal,
            Money,
            Danger,
        }

    public class ReleasedObject : Entity3D
    {


        // Fields
        protected ReleaseType releaseType;
        protected float releasedObjectLenght;
        protected float releaseObjSpeed;
        protected Entity3D shadow;
        protected float arrowPosY;
        protected EntityText rewardText;
        protected bool release;
        protected bool isFadingAway;
        protected Entity2D icon;
        protected bool hitWithPaddleSupport;
        protected SoundEffect sound1;
        protected SoundEffect sound2;
        bool soundOn = false;
        // Properties
        public float ReleasedObjectLenght
        {
            get { return releasedObjectLenght; }
            set { releasedObjectLenght = value; }
        }
        public ReleaseType ReleaseType
        {
            get { return releaseType; }
            set { releaseType = value; }
        }
        public override BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(Position - (new Vector3(releasedObjectLenght / 2, releasedObjectLenght / 2, 0.25f)),
                    Position + (new Vector3(releasedObjectLenght / 2, releasedObjectLenght / 2, 0.25f)));
            }
        }

        public ReleasedObject(Model Model, Vector3 Position)
            : base(Model, Position)
        {
        }

        public ReleasedObject(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
        }

        protected override void Setup(Model Model, Vector3 Position)
        {
            base.Setup(Model, Position);
            sound1 = Engine.Content.Load<SoundEffect>("Content\\Sounds\\button-9");
            sound2 = Engine.Content.Load<SoundEffect>("Content\\Sounds\\releaseObject");

            this.releasedObjectLenght = 1f;
            isFadingAway = false;
            this.Alpha = 1f;
            this.releaseObjSpeed = GlobalVariables.releaseObjSpeed;
            Visible = false;
            // Setup the image with the arrow
            arrowPosY = (float)Util.random.NextDouble();
            shadow = new Entity3D(Engine.Content.Load<Model>("Content\\Models\\RadialShadow"),
                                      new Vector3(Position.X, 0.2f + (0.1f * arrowPosY), Position.Z), this.Parent);
            shadow.Scale = new Vector3(0.5f);
            shadow.Alpha = 0.6f;
            shadow.Visible = false;
            icon = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\danger"),
                                GraphicsUtil.GetProjectPoint(Position), this.Parent);
            icon.Visible = false;
        }

        public override void Update()
        {
            if (!isFadingAway)
            {
                icon.Position = GraphicsUtil.GetProjectPoint(Position+new Vector3(0f,1f,0f))
                                -new Vector2(0f,(icon.Texture.Width));
                Position += new Vector3(0f, 0f, releaseObjSpeed);
                shadow.Position = new Vector3(Position.X, 0.2f + (0.1f * arrowPosY), Position.Z);
            }
            else
            {
                if (!soundOn)
                {
                    
                    soundOn = true;
                }
                icon.Position += new Vector2(0f, 1f);
                Position += new Vector3(0f, 0.02f, 0f);
                icon.Alpha -= 0.02f;
                Alpha -= 0.08f;
                shadow.Alpha -= 0.08f;
                if (Alpha == 0f)
                {
                    this.DisableComponent();
                }
            }
            base.Update();
        }

        public override void DisableComponent()
        {
            this.shadow.DisableComponent();
            this.icon.DisableComponent();
            base.DisableComponent();
        }
        public virtual void fadeAway()
        {
            isFadingAway = true;
            icon.Visible = true;
        }

        public void releaseObject()
        {
            release = true;
            Visible = true;
            icon.Visible = true;
            shadow.Visible = true;
        }
        public virtual void hitPaddle()
        {
            sound2.Play();
        }
        public virtual void hitPaddleSupport()
        {
            sound1.Play();
            hitWithPaddleSupport = true;
        }
    }
}

