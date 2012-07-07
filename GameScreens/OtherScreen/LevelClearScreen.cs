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
    public class LevelClearScreen : GameScreen
    {
        // Fields and Components========================================
        KeyboardDevice keyboard;
        MouseDevice mouseDevice;

        Entity2D background;
        Entity2D hand;
        Entity2D fade;

        EntityText titleText;
        EntityText scoreText;
        EntityText moneyText;

        Button nextBtn;

        float transitionPosition = 1f;

        // Constructor===============================================
        public LevelClearScreen(string Name)
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
            mouseDevice = Engine.Services.GetService<MouseDevice>();
            mouseDevice.ResetMouseAfterUpdate = false;

            Texture2D texture;
            Texture2D level_finish_bg=Engine.Content.Load<Texture2D>("Content\\Textures\\level_finish_bg");

            hand = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\hand"),
                    new Vector2(mouseDevice.State.X, mouseDevice.State.Y), this);

            // Initialize the Game Text
            string level="LEVEL " + GlobalVariables.level.ToString() + " COMPLETE";
            titleText = new EntityText(Engine.Content.Load<SpriteFont>("Content\\Fonts\\FontAgency24"),
                new Vector2((Engine.Viewport.Width / 2) - 20, 200), level, this);

            string score = "Score: " + GlobalVariables.score.ToString()+" points.";
            scoreText = new EntityText(Engine.Content.Load<SpriteFont>("Content\\Fonts\\FontAgency24"),
                new Vector2((Engine.Viewport.Width / 2) - 120, 270), score, this);

            string money = "Money: " + GlobalVariables.money.ToString() + " Dirhams";
            moneyText = new EntityText(Engine.Content.Load<SpriteFont>("Content\\Fonts\\FontAgency24"),
                new Vector2((Engine.Viewport.Width / 2) - 120, 330), money, this);

            // Initialize the black texture
            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\next_btn");
            nextBtn = new Button(texture, new Vector2(475, 380), this);

            background = new Entity2D(level_finish_bg,
                                Vector2.Zero, this);
            background.Alpha = 0f;
            background.Position = new Vector2((Engine.Viewport.Width / 2) - (background.Texture.Width / 2),
                                             (Engine.Viewport.Height / 2) - (background.Texture.Height / 2));


            fade = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\black"),
                    Vector2.Zero, this);
            fade.Alpha = 0.4f;

            base.Initialize();
        }

        // Update function============================================
        public override void Update()
        {
            // Read Inputs
            keyboard.Update();

            // Update hand pointer position
            hand.Position = new Vector2(mouseDevice.State.X, mouseDevice.State.Y);

            if ((background.Alpha <= 1))
            {
                transitionPosition += 0.01f / 2;
                background.Alpha = transitionPosition;
            }

            if (nextBtn.isClicked() || keyboard.WasKeyReleased(Keys.Enter))
            {
                // Increase the level and the speed of the ball
                GlobalVariables.speedFactor += 0.05f;
                GlobalVariables.level++;

                Engine.Services.RemoveService(typeof(Camera));
                Engine.GameScreens["GamePlayScreen"].Disable();
                this.Disable();
                //Engine.AddScreen(new GamePlayScreen("GamePlayScreen"));
                Engine.AddScreen(new LoadScreen("LoadScreen"));
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
