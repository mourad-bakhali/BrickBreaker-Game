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
    public class GameOverScreen : GameScreen
    {
        // Fields and Components========================================
        KeyboardDevice keyboard;

        float transitionPosition = 1f;
        bool isTransitionOff = false;

        Entity2D background;
        Entity2D fade;
        Entity2D gameOverText;


        // Constructor===============================================
        public GameOverScreen(string Name)
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
            gameOverText = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\gameOver"),
                new Vector2((Engine.Viewport.Width / 2) - 40, (Engine.Viewport.Height / 2)), this);
            gameOverText.Position = new Vector2((Engine.Viewport.Width / 2) - (gameOverText.Texture.Width/2) ,
                                                (Engine.Viewport.Height / 2) - (gameOverText.Texture.Height / 2));
            // Initialize the black texture
            background = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\MenuBackground"),
                                Vector2.Zero, this);

            GlobalVariables.speedFactor = 0.10f;
            GlobalVariables.level = 1;
            GlobalVariables.score = 0;
            GlobalVariables.resistance = 0;

            base.Initialize();
        }

        // Update function============================================
        public override void Update()
        {
            // Read Inputs
            keyboard.Update();

            if (fade.Alpha >= 0f && !isTransitionOff)
            {
                transitionPosition -= 0.01f;
                fade.Alpha = transitionPosition;
            }
            else
            {

                isTransitionOff = true;
                transitionPosition += 0.01f/2f;
                fade.Alpha = transitionPosition;
                if (fade.Alpha >= 1f)
                {
                    Disable();
                    Engine.AddScreen(new MenuScreen("Menu"));
                }
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
