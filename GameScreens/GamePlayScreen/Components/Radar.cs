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
    public class Radar : Entity3D
    {
        // Fields
        public bool isPlaying = false;

        public Radar(Model Model, Vector3 Position)
            : base(Model, Position)
        {
            Setup(Model, Position);
        }

        public Radar(Model Model, Vector3 Position, GameScreen Parent)
            : base(Model, Position, Parent)
        {
            Setup(Model, Position);
        }
        protected override void Setup(Model Model, Vector3 Position)
        {
            base.Setup(Model, Position);
            Scale = new Vector3(0.125f);
        }
        public override void Update()
        {

            if (isPlaying)
            {
                Alpha -= 0.01f;
                Scale += new Vector3(0.05f);
                if (Alpha < 0.1f)
                {
                    Alpha = 1f;
                    isPlaying = false;
                    Scale = new Vector3(0.125f);
                    Visible = false;
                }
            }
            base.Update();
        }
        public void play(Vector3 pos)
        {
            Position = new Vector3(pos.X, 0.2f, pos.Z);
            isPlaying = true;
        }
    }
}
