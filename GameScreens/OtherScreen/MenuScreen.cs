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
    class MenuScreen : GameScreen
    {
        // Components
        MouseDevice mouseDevice;

        Entity2D black;
        
        Entity2D background;
        Entity2D menuTitle;
        Entity2D menuBall;
        Entity2D hand;

        // Buttons
        Button menuPlay;
        //Button menuOptions;
        //Button menuWho;
        Button menuQuit;

        KeyboardDevice keyboard;
        public MenuScreen(string Name)
            : base(Name)
        {

        }

        public override void Initialize()
        {
            // Initialize the keyboard
            keyboard = Engine.Services.GetService<KeyboardDevice>();
            mouseDevice = Engine.Services.GetService<MouseDevice>();
            mouseDevice.ResetMouseAfterUpdate=false;

            // Initialize Components
            Texture2D texture;

            black = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\black"),
                                Vector2.Zero, this);

            hand = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\hand"),
                                new Vector2(mouseDevice.State.X, mouseDevice.State.Y), this);
                
            // Initialize Buttons
            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\menuPlayBtn");
            menuPlay = new Button(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 270), this);

            //texture = Engine.Content.Load<Texture2D>("Content\\Textures\\menuOptionsBtn");
            //menuOptions = new Button(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 315), this);

            //texture = Engine.Content.Load<Texture2D>("Content\\Textures\\menuWhoBtn");
            //menuWho = new Button(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 360), this);

            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\menuQuitBtn");
            menuQuit = new Button(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 325), this);



            texture = Engine.Content.Load<Texture2D>("Content\\Textures\\MenuTitle");
            menuTitle = new Entity2D(texture, new Vector2((Engine.Viewport.Width / 2) - (texture.Width / 2), 155), this);

            texture=Engine.Content.Load<Texture2D>("Content\\Textures\\MenuBall");
            menuBall = new Entity2D(texture, new Vector2(473,81), this);

            background = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\MenuBackground"), Vector2.Zero, this);

            base.Initialize();
        }

        public override void Update()
        {
            // Read Inputs
            keyboard.Update();
            mouseDevice.Update();

            // Update black Texture
            if (black.Alpha > 0)
            {
                black.Alpha-=0.01f;
            }


            // Update hand pointer position
            hand.Position = new Vector2(mouseDevice.State.X, mouseDevice.State.Y);

            if (menuPlay.isClicked() || keyboard.WasKeyReleased(Keys.Enter))
            {
                Disable();
                Engine.AddScreen(new LoadScreen("LoadScreen"));
                //Engine.AddScreen(new GamePlayScreen("GamePlayScreen"));
            }
            if (menuQuit.isClicked())
            {
                Engine.Game.Exit();
            }
            base.Update();
        }

    }
}
