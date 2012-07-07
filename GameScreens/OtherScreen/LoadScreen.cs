using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlutoEngine;

namespace BrickBreaker
{
    class LoadScreen : GameScreen
    {
        // Components
        GamePlayScreen gamePlayScreen;
        bool gameInitialized = false;
        int angle = 0;

        TimeSpan timeToWait = TimeSpan.FromMilliseconds(500);
        TimeSpan time = TimeSpan.Zero;

        KeyboardDevice keyboard;

        Entity2D black;
        Entity2D background;
        Entity2D loading;
        Entity2D pressEnter;
       

        public LoadScreen(string Name)
            : base(Name)
        {
            BlocksDraw = true;
            BlocksUpdate = true;
        }

        public override void Initialize()
        {
            // Initialize the keyboard
            keyboard = Engine.Services.GetService<KeyboardDevice>();

            // Initialize Components
            Texture2D texture;

            black = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\black"),
                                Vector2.Zero, this);


            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\loadingText");
            loading = new Entity2D(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 493), this);

            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\Press_ENTER");
            pressEnter = new Entity2D(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 493), this);
            pressEnter.Visible = false;

            background = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\LoadScreen"), Vector2.Zero, this);

            base.Initialize();
        }

        public override void Update()
        {
            // Read Inputs
            keyboard.Update();

            // Update black Texture
            if (black.Alpha > 0)
            {
                black.Alpha -= 0.01f;
            }
            else
            {
                // Update the time
                if (!gameInitialized)
                {
                    time += Engine.GameTime.ElapsedGameTime;
                    if (time >= timeToWait)
                    {
                        Trace.WriteLine("time >= timeToWait");
                        // Load the game play elements
                        gamePlayScreen = new GamePlayScreen("GamePlayScreen");
                        gameInitialized = true;
                        loading.Visible = false;
                        pressEnter.Visible = true;
                    }
                }
                else
                {
                    if (angle < 180)
                        angle++;
                    else
                        angle += 10;

                    if(angle>360)
                    {
                        angle=0;
                    }
                    float alpha = (float)Math.Sin(MathHelper.ToRadians(angle));
                    pressEnter.Alpha = alpha;
                    if (keyboard.WasKeyReleased(Keys.Enter))
                    {
                        this.Disable();
                        Engine.AddScreen(gamePlayScreen);
                    }
                }
            }

            base.Update();
        }

    }
}
