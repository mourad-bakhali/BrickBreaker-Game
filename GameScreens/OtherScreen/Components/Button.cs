using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlutoEngine;

namespace BrickBreaker
{
    public class Button : Entity2D
    {
        MouseDevice mouseDevice;
        MouseState cmouse;
        MouseState lmouse;

        int angle = 0;
        Vector2 buttonPosition;
        bool clicked = false;

        public Button(Texture2D texture, Vector2 Position, GameScreen Parent)
            : base(texture, Position, Parent)
        {
            Setup(texture, Position);
        }
        protected override void Setup(Texture2D texture, Vector2 Position)
        {
            base.Setup(texture, Position);
            mouseDevice = Engine.Services.GetService<MouseDevice>();
            mouseDevice.ResetMouseAfterUpdate = false;
            buttonPosition = Position;
            cmouse = Mouse.GetState();
            
        }
        public override void Update()
        {
            mouseDevice.Update();

            // Update mouse state
            lmouse = cmouse;
            cmouse = Mouse.GetState();

            if ((mouseDevice.State.X >= Position.X) && (mouseDevice.State.X <= Position.X + Texture.Width) &&
                (mouseDevice.State.Y >= Position.Y) && (mouseDevice.State.Y <= Position.Y + Texture.Height))
            {
                if (angle >= 360) angle = 0;
                float sinValue = (float)Math.Cos((double)MathHelper.ToRadians((float)angle));
                Position -= new Vector2(sinValue * 0.8f, 0f);
                angle+=10;

                // Test if the Player click on the Button
                if (cmouse.LeftButton==ButtonState.Released && lmouse.LeftButton==ButtonState.Pressed)
                {
                    clicked = true;
                }
            }
            else
            {
                angle = 0;
                Position=new Vector2(MathHelper.Lerp(Position.X, buttonPosition.X, 0.2f),MathHelper.Lerp(Position.Y, buttonPosition.Y, 0.2f));
            }
            base.Update();
        }

        public bool isClicked()
        {
            return clicked;
        }
    }
}
