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
    public class PauseScreen : GameScreen
    {
        // Fields and Components========================================
        KeyboardDevice keyboard;
        MouseDevice mouseDevice;
        Entity2D black;
        EntityText pauseText;
        Entity2D hand;

        Button returnBtn;
        Button menuBtn;
        Button quitBtn;

        // Constructor===============================================
        public PauseScreen(string Name)
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

            // Initialize buttons
            Texture2D texture;

            hand = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\hand"),
                    new Vector2(mouseDevice.State.X, mouseDevice.State.Y), this);

            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\menuReturnBtn");
            returnBtn = new Button(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 270), this);

            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\menuMenuBtn");
            menuBtn = new Button(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 320), this);

            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\menuQuitBtn");
            quitBtn = new Button(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 370), this);


            pauseText = new EntityText(Engine.Content.Load<SpriteFont>("Content\\Fonts\\FontAgency24"),
                new Vector2((Engine.Viewport.Width / 2)-40, 200), "PAUSE", this);

            // Initialize the black texture
            black = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\black"),
                    Vector2.Zero, this);
            black.Alpha = 0.4f;


            base.Initialize();
        }

        // Update function============================================
        public override void Update()
        {
            // Read Inputs
            keyboard.Update();


            // Update hand pointer position
            hand.Position = new Vector2(mouseDevice.State.X, mouseDevice.State.Y);

            if (keyboard.WasKeyReleased(Keys.Escape))
            {
                this.Disable();
            }
            if (returnBtn.isClicked())
            {
                this.Disable();
            }
            if (menuBtn.isClicked())
            {
                Engine.Services.RemoveService(typeof(Camera));
                Engine.GameScreens["GamePlayScreen"].Disable();
                this.Disable();
                Engine.AddScreen(new MenuScreen("Menu"));
            }
            if (quitBtn.isClicked())
            {
                Engine.Game.Exit();
            }
            base.Update();
        }

    }
}
