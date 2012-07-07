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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using PlutoEngine;
using XSIXNARuntime;

namespace BrickBreaker
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        // The IGraphicsDeviceService the engine will use
        GraphicsDeviceManager graphics;


        public Main()
        {
            // Setup graphics
            graphics = new GraphicsDeviceManager(this);

            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;

            //this.graphics.IsFullScreen = true;
            Engine.Game = this;
        }


        protected override void LoadContent()
        {
            // Setup engine. We do this in the load method
            // so that we know graphics will be ready for use
            Engine.SetupEngine(graphics);

            // Setup Inputs
            MouseDevice mouse = new MouseDevice();
            KeyboardDevice keyboard = new KeyboardDevice();

            
            Engine.Services.AddService(typeof(MouseDevice), mouse);
            Engine.Services.AddService(typeof(KeyboardDevice), keyboard);

            // Add MenuScreen
            Engine.AddScreen(new IntroScreen("IntroScreen"));
            Trace.WriteLine(Engine.GameScreens[0].Name);

        }


        protected override void Update(GameTime gameTime)
        {
            // Update the engine and game
            Engine.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);

            // Draw the engine and game
            Engine.Draw(gameTime, ComponentType.All);
        }
    }
}
