using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlutoEngine;

namespace BrickBreaker
{
    public class GamePlayScreen : GameScreen
    {
        // Fields and Components========================================
        ShakedCamera camera;
        KeyboardDevice keyboard;
        Entity3D gameMap;
        Ball ball;
        Paddle paddle;
        PaddleSupport paddleSupport;
        Hud hud;
        Entity2D black;
        float transitionPosition = 1f;
        bool wasPaused = false;
        
        // Sounds
        Song music;
        bool musicOn = false;

        Radar radar;
        public GaussianBlur blur;

        // Bricks
        int[,] brickMapArray;
        Brick[,] brickObjectArray;
        int brickLenghtLine;
        int brickLenghtColumn;
        int totalOfBricks;
        int totalOfBricksLeft;

        // Released Objects 
        ReleasedObject[] ReleasedObjectArray;
        int ReleasedObjectArrayIndex;

        // Constructor===============================================
        public GamePlayScreen(string Name)
            : base(Name)
        {
            BlocksInput = true;
            BlocksUpdate = true;
        }

        // Initializor=============================================
        public override void Initialize()
        {
            // Initialize the Camera
            camera = new ShakedCamera(this);
            camera.Position = new Vector3(0, 13, 17);
            camera.Target = new Vector3(0, 0, 3);
            

            if (!Engine.Services.ContainsService(typeof(Camera)))
                Engine.Services.AddService(typeof(Camera), (ShakedCamera)camera);

            // Initialize the keyboard
            keyboard = Engine.Services.GetService<KeyboardDevice>();

            // Initialize the sounds

            music = Engine.Content.Load<Song>("Content\\Sounds\\music1");
            MediaPlayer.Play(music);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;

            // Inistialize tha black Texture
            black = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\black"),
                    Vector2.Zero, this);

            // Initialize tha Radar
            radar = new Radar(Engine.Content.Load<Model>("Content\\Models\\Radar"), Vector3.One, this);
            radar.Visible = false;

            // Initialize the Ball
            ball = new Ball(Engine.Content.Load<Model>("Content\\Models\\GameBall"), 
                            new Vector3(0f,0f,5f), this);
            ball.defaultLightingEnabled = true;

            // Initialize the Bricks List
            brickLenghtLine = 3;
            brickLenghtColumn = 10;
            totalOfBricks = brickLenghtLine * brickLenghtColumn;
            totalOfBricksLeft = totalOfBricks;
            brickMapArray=new int[brickLenghtLine, brickLenghtColumn];
            brickObjectArray = new Brick[brickLenghtLine, brickLenghtColumn];

            for (int i = 0; i < brickLenghtLine; i++)
            {
                for (int j = 0; j < brickLenghtColumn; j++)
                {
                    brickMapArray[i, j] = Util.random.Next(0, 3);
                }
            }

            // Initialize the Released Object array
            ReleasedObjectArray = new ReleasedObject[totalOfBricks];
            for (int i = 0; i < totalOfBricks; i++)
                ReleasedObjectArray[i] = null;

            ReleasedObjectArrayIndex = 0;

            // initialize the ObjectArray of bricks
            for (int i = 0; i < brickLenghtLine; i++)
            {
                for (int j = 0; j < brickLenghtColumn; j++)
                {
                    Brick brick;
                    Model model;
                    // Type of Brick
                    Vector3 brikPos = new Vector3(-7f + j * 1.5f, 0f, -4f + i*1.5f);
                    switch (brickMapArray[i,j])
                    {
                        case 0:
                            // Type Normal
                            model = Engine.Content.Load<Model>("Content\\Models\\GameBrick1");
                            brick = new Brick(model, brikPos, this);
                            brick.BrickReleaseType = BrickReleaseType.Normal;
                            brickObjectArray[i, j] = brick;
                            
                            model = Engine.Content.Load<Model>("Content\\Models\\ReleasedNormObject");
                            ReleasedObjectArray[brickLenghtColumn * i + j] = new NormalObject(model, brikPos, this);
                            break;
                        case 1:
                            // Type Score
                            model = Engine.Content.Load<Model>("Content\\Models\\GameBrick1");
                            brick = new Brick(model, brikPos, this);
                            brick.BrickReleaseType = BrickReleaseType.Money;
                            brickObjectArray[i, j] = brick;

                            model = Engine.Content.Load<Model>("Content\\Models\\ReleasedMoneyObject");
                            ReleasedObjectArray[brickLenghtColumn * i + j] = new MoneyObject(model, brikPos, this);
                            break;
                        case 2:
                            // Type Life
                            model = Engine.Content.Load<Model>("Content\\Models\\GameBrick1");
                            brick = new Brick(model, brikPos, this);
                            brick.BrickReleaseType = BrickReleaseType.Danger;
                            brickObjectArray[i, j] = brick;

                            model = Engine.Content.Load<Model>("Content\\Models\\ReleasedDangerObject");
                            ReleasedObjectArray[brickLenghtColumn * i + j] = new DangerObject(model, brikPos, this);
                            break;
                        
                        default:
                            // Type Normal
                            model = Engine.Content.Load<Model>("Content\\Models\\GameBrick1");
                            brick = new Brick(model, brikPos, this);
                            brick.BrickReleaseType = BrickReleaseType.Normal;
                            brickObjectArray[i, j] = brick;

                            model = Engine.Content.Load<Model>("Content\\Models\\ReleasedNormObject");
                            ReleasedObjectArray[brickLenghtColumn * i + j] = new NormalObject(model, brikPos, this);
                            break;
                    }

                    // Update the ReleasedObjectArrayIndex
                    if (ReleasedObjectArrayIndex >= totalOfBricks)
                        ReleasedObjectArrayIndex = 0;
                    else
                        ReleasedObjectArrayIndex++;
                }
            }



            // Initialize the Paddle
            paddle = new Paddle(Engine.Content.Load<Model>("Content\\Models\\GamePaddle"), Vector3.Zero, this);
            paddle.defaultLightingEnabled = true;


            // Iniotialize the spiralRod
            paddleSupport = new PaddleSupport(Engine.Content.Load<Model>("Content\\Models\\SpiralRod"),
                                     new Vector3(0f, 0.6f, 9f), this);
            //paddleSupport.Rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(90f));
            paddleSupport.defaultLightingEnabled = true;

            // Initialize the Blur
            blur = new GaussianBlur(Engine.Viewport.Width, Engine.Viewport.Height, this);
            blur.Visible = false;

            // Initialize HUD
            hud = new Hud(this);


            // Initialize the gameMap
            gameMap = new Entity3D(Engine.Content.Load<Model>("Content\\Models\\GameMap"), Vector3.Zero, this);
            gameMap.defaultLightingEnabled = true;

            

            base.Initialize();
        }

        // Update function============================================
        public override void Update()
        {
            // Read Inputs
            keyboard.Update();

            if (!musicOn)
            {
                MediaPlayer.Volume = 0.5f;
                musicOn = true;
            }

            // Update black Texture
            if (black.Alpha > 0)
            {
                transitionPosition -= 0.008f;
                black.Alpha = transitionPosition;
            }
            else
            {
                black.Alpha = 0f;
                transitionPosition = 0f;     
            }

            if (wasPaused)
            {
                wasPaused = false;
                blur.Visible = false;
                MediaPlayer.Resume();
            }



            // Test collition of Paddle and the Ball
            if (ball.BoundingBox.Intersects(paddle.BoundingBox))
            {
                
                if (ball.VelocityX > 0 && ball.VelocityZ > 0)
                {
                    // The distance between the box and the Paddle
                    float distanceBallPaddle = paddle.Position.X - ball.Position.X;
                    if ((distanceBallPaddle > 1.2))
                    {
                        // Reverce direction
                        ball.VelocityX *= -1f;
                        ball.VelocityZ *= -1f;
                    }

                }
                if (ball.VelocityX < 0 && ball.VelocityZ > 0)
                {
                    // The distance between the box and the Paddle
                    float distanceBoxPaddle = ball.Position.X - paddle.Position.X;
                    if ((distanceBoxPaddle > 1))
                    {
                        // Reverce direction
                        ball.VelocityX *= -1f;
                        ball.VelocityZ *= -1f;
                    }
                }

                camera.shake(0.03f, 0.5f);
                ball.VelocityZ *= -1;
                ball.Position -= new Vector3(0f, 0f, 0.1f);
            }
            
            // Test the collision between the ball and the bricks
            bool getOutOfTheLoop = false;
            for (int i = 0; i < brickLenghtLine; i++)
            {
                for (int j = 0; j < brickLenghtColumn; j++)
                {
                    if (brickObjectArray[i, j] != null)
                    {
                        if (ball.BoundingBox.Intersects(brickObjectArray[i, j].BoundingBox))
                        {
                            if (ball.Position.Z < brickObjectArray[i, j].Position.Z - 0.5f)
                            {
                                // The ball is above the brick
                                ball.VelocityZ *= -1;
                            }
                            else if (ball.Position.Z > brickObjectArray[i, j].Position.Z + 0.5f)
                            {
                                // The ball is under the brick
                                ball.VelocityZ *= -1;

                            }
                            else if (ball.Position.X > brickObjectArray[i, j].Position.X + 0.5f)
                            {
                                // The ball is in the Right of the brick
                                ball.VelocityX *= -1;
                            }
                            else if (ball.Position.X < brickObjectArray[i, j].Position.X - 0.5f)
                            {
                                // The ball is in the Left of the brick
                                ball.VelocityX *= -1;
                            }

                            // Release the Object if the brick should release one
                            ReleasedObjectArray[brickLenghtColumn * i + j].Position=new Vector3(brickObjectArray[i, j].Position.X,
                                ReleasedObjectArray[brickLenghtColumn * i + j].Position.Y,
                                brickObjectArray[i, j].Position.Z);
                            ReleasedObjectArray[brickLenghtColumn * i + j].releaseObject();

                            // Delete the brick and decrease the number of Bricks
                            totalOfBricksLeft -= 1;
                            //brickObjectArray[i, j].DisableComponent();
                            brickObjectArray[i, j].Die();
                            brickObjectArray[i, j] = null;
                            getOutOfTheLoop = true;
                            break;
                        }
                    }
                }
                if (getOutOfTheLoop) break;
            }
            
            // Delete any released Object if it goes out of the GameMap
            for (int i = 0; i < totalOfBricks; i++)
            {
                if (ReleasedObjectArray[i] != null)
                {
                    if (paddle.BoundingBox.Intersects(ReleasedObjectArray[i].BoundingBox))
                    {
                        radar.Visible = true;
                        radar.play(paddle.Position);
                        switch (ReleasedObjectArray[i].ReleaseType)
                        {
                            case ReleaseType.Normal:
                                hud.Score += 100;
                                break;
                            case ReleaseType.Money:
                                hud.Money += 10;
                                break;
                            case ReleaseType.Danger:
                                hud.Score += 10;
                                hud.updateDamage(+10f);
                                break;
                        }
                        
                        ReleasedObjectArray[i].hitPaddle();
                        ReleasedObjectArray[i].fadeAway();
                        ReleasedObjectArray[i] = null;
                    }
                    else if (ReleasedObjectArray[i].BoundingBox.Intersects(paddleSupport.BoundingBox))
                    {
                        switch (ReleasedObjectArray[i].ReleaseType)
                        {
                            case ReleaseType.Normal:
                                hud.Score += 10;
                                break;
                            case ReleaseType.Money:
                                hud.Money += 1;
                                break;
                            case ReleaseType.Danger:
                                hud.updateDamage(-5f);
                                break;
                        }
                        ReleasedObjectArray[i].fadeAway();
                        ReleasedObjectArray[i].hitPaddleSupport();
                        ReleasedObjectArray[i] = null;
                    }
                }
            }
            
            // Test of collision between the Ball and the GameMap
            if ((ball.Position.X > 8f - ball.BallRaduis / 2) || (ball.Position.X < -8f + ball.BallRaduis / 2))
            {
                ball.playSound();
                camera.shake(0.08f, 0.5f);
                ball.VelocityX *= -1;
            }
            if ((ball.Position.Z < -10f + ball.BallRaduis / 2) || (ball.Position.Z > 10f - ball.BallRaduis / 2))
            {
                ball.playSound();
                camera.shake(0.08f, 0.5f);
                ball.VelocityZ *= -1;
            }

            // Test of collision between the Ball and the PaddleSupport
            if (ball.Position.Z>=paddleSupport.Position.Z-0.5f)
            {
                ball.playSound();
                ball.Position = new Vector3(ball.Position.X, ball.Position.Y, paddleSupport.Position.Z - 0.8f);
                paddleSupport.getHit();
                hud.updateDamage(-10f);
                ball.VelocityZ *= -1;
            }

            // Update Paddle Position
            if (keyboard.IsKeyDown(Keys.Left) && paddle.Position.X >= -6.7f)
            {
                paddle.VelocityX = 0.2f;
                paddle.Position -= new Vector3(paddle.VelocityX, 0f, 0f);
                paddleSupport.Rotation *= Matrix.CreateRotationX(0.5f);
            }
            if (keyboard.IsKeyDown(Keys.Right) && paddle.Position.X <= 6.7f)
            {
                paddle.VelocityX = 0.2f;
                paddle.Position += new Vector3(paddle.VelocityX, 0f, 0f);
                paddleSupport.Rotation *= Matrix.CreateRotationX(-0.5f);
            }
            if (keyboard.IsKeyDown(Keys.Space))
            {
                //totalOfBricksLeft = 0;
            }

            // Test to see if the level is finished
            if (totalOfBricksLeft <= 0)
            {

                // Hide the HUD
                hud.hide();
                hud.money.Position += new Vector2(0f, 0.5f);
                hud.moneyGlass.Position += new Vector2(0f, 0.5f);
                hud.score.Position += new Vector2(0f, 0.5f);
                hud.scoreGlass.Position += new Vector2(0f, 0.5f);
                hud.damageBar.Position += new Vector2(0f, -0.5f);
                hud.damageBarGlass.Position += new Vector2(0f, -0.5f);
                hud.damageText.Position += new Vector2(0f, -0.5f);
                hud.black.Position += new Vector2(0f, 0.5f);
                hud.moneyText.Position += new Vector2(0f, 0.5f);
                hud.scoreText.Position += new Vector2(0f, 0.5f);
                hud.damageBox.Position += new Vector2(0f, -0.5f);

                // Stop moving the ball and move to center
                ball.VelocityX = 0f;
                ball.VelocityZ = 0f;
                ball.Position = new Vector3(MathHelper.Lerp(ball.Position.X, 0f, 0.02f), 0.5f, MathHelper.Lerp(ball.Position.Z, 0f, 0.02f));

                if (hud.black.Position.Y>Engine.Viewport.Height)
                {
                    GlobalVariables.score = hud.Score;
                    GlobalVariables.money = hud.Money;

                    Engine.AddScreen(new LevelClearScreen("LevelClearScreen"));
                }
                else
                {
                    //camera.Position += new Vector3(0f, 0.06f, 0f);
                    //camera.Position = new Vector3(camera.Position.X, MathHelper.Lerp(camera.Position.Y, 35f, 0.0002f), camera.Position.Z);
                }
            }

            // Test if the game is Over
            if (hud.gameIsOver())
            {
                if (black.Alpha < 1f)
                {
                    transitionPosition += 0.01f;
                    black.Alpha = transitionPosition;
                    camera.Position += new Vector3(0f, 0.07f, 0f);
                    ball.VelocityX=0f;
                    ball.VelocityZ=0f;
                    ball.Position = new Vector3(MathHelper.Lerp(ball.Position.X, 0f, 0.02f), 0.5f, MathHelper.Lerp(ball.Position.Z, 0f, 0.02f));

                }
                else
                {
                    Engine.Services.RemoveService(typeof(Camera));
                    MediaPlayer.Stop();
                    Disable();
                    Engine.AddScreen(new GameOverScreen("GameOverScreen"));
                }
            }

            if (keyboard.WasKeyReleased(Keys.Escape))
            {
                blur.Visible = true;
                wasPaused = true;
                Engine.AddScreen(new PauseScreen("Pause"));
                MediaPlayer.Pause();
            }
            base.Update();
        }

    }
}
