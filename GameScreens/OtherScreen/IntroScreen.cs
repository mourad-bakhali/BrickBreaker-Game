using System;
using System.Threading;
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
    public class IntroScreen : GameScreen
    {
        // Fields and Components========================================
        KeyboardDevice keyboard;

        Entity2D background;
        Entity2D fade;

        float transitionPosition = 1f;
        bool isTransitionOff = false;

        // Constructor===============================================
        public IntroScreen(string Name)
            : base(Name)
        {
            BlocksInput = true;
            BlocksUpdate = true;
        }

        // Initializor=============================================
        public override void Initialize()
        {
            // Initialize the keyboard
            keyboard = Engine.Services.GetService<KeyboardDevice>();


            fade = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\black"),
                    Vector2.Zero, this);

            // Initialize the GameOverText

            // Initialize the black texture
            background = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\intro"),
                                Vector2.Zero, this);

            base.Initialize();
        }

        // Update function============================================
        public override void Update()
        {
            // Read Inputs
            keyboard.Update();

            if (fade.Alpha >= 0f && !isTransitionOff)
            {
                transitionPosition -= 0.01f/2;
                fade.Alpha = transitionPosition;
            }


            if (keyboard.WasKeyReleased(Keys.Enter))
            {
                this.Disable();
                Engine.AddScreen(new MenuScreen("Menu"));
            }

            base.Update();
        }

        public int sizeOfText(EntityText textEntity)
        {
            // The width of each character in the spriteFont
            int charWidth = 13;

            // return the size of the text
            return charWidth * textEntity.Text.Length;
        }
    }
}
