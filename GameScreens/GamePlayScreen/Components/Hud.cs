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
    public class Hud : Component
    {
        // Fields =======================================//
        float damageValue;
        float resistanceValue;
        float maxDamage;
        int damageBarWith = 260;
        int scoreValue;
        int moneyValue;
        float transitionPosition = 0f;
        bool isHiding = false;

        public Texture2D hudSprite;
        public Entity2D black;

        public Entity2D damageBox;
        public Entity2D damageBarGlass;
        public Entity2D damageBar;
        public Entity2D damageText;

        public Entity2D score;
        public Entity2D money;
        public Entity2D scoreGlass;
        public Entity2D moneyGlass;

        public EntityText scoreText;
        public EntityText moneyText;
        public EntityText levelText;

        public float Damage
        {
            get { return damageValue; }
            set { damageValue = value; }
        }
        public float Resistance
        {
            get { return resistanceValue; }
            set { resistanceValue = value; }
        }
        public int Score
        {
            get { return scoreValue; }
            set { scoreValue = value; }
        }
        public int Money
        {
            get { return moneyValue; }
            set { moneyValue = value; }
        }

        // Constructors =======================================//
        public Hud(GameScreen parent):base(parent)
        {
            // Initialize Fields
            maxDamage = 100f;
            damageValue = 100f;
            resistanceValue = 100f;
            scoreValue = GlobalVariables.score;
            moneyValue = GlobalVariables.money;

            hudSprite = Engine.Content.Load<Texture2D>("Content\\Textures\\damage");
            SpriteFont font1 = Engine.Content.Load<SpriteFont>("Content\\Fonts\\FontAgency24");

            // Initialize the Damage bars
            damageText = new Entity2D(hudSprite, new Vector2(15f, 30f), this.Parent);
            damageText.Rectangle = new Rectangle(0, 150, 260, 50);

            damageBox = new Entity2D(hudSprite, new Vector2(10f, 10f), this.Parent);
            damageBox.Rectangle = new Rectangle(0, 0, 260, 50);

            damageBarGlass = new Entity2D(hudSprite, new Vector2(10f, 20f), this.Parent);
            damageBarGlass.Rectangle = new Rectangle(0, 50, 260, 50);

            damageBar = new Entity2D(hudSprite, new Vector2(20f, 20f), this.Parent);
            damageBar.Rectangle = new Rectangle(0, 100, 238, 50);

            // Initialize the text
            Vector2 bgHudPos = new Vector2(0, Engine.Viewport.Height - 90);

            levelText = new EntityText(font1, new Vector2(10, 11), "Level:"+GlobalVariables.level.ToString(), this.Parent);
            levelText.Position = new Vector2(Engine.Viewport.Width-120, bgHudPos.Y + 15);

            scoreText = new EntityText(font1, new Vector2(10, 11), GlobalVariables.score.ToString(), this.Parent);
            scoreText.Position = new Vector2(230, bgHudPos.Y + 15);

            moneyText = new EntityText(font1, new Vector2(10, 35), GlobalVariables.money.ToString(), this.Parent);
            moneyText.Position = new Vector2(530, bgHudPos.Y + 15);

            hudSprite = Engine.Content.Load<Texture2D>("Content\\Textures\\moneyHUD");

            // Initialize the score and money fields
            scoreGlass = new Entity2D(hudSprite, new Vector2(10, bgHudPos.Y), this.Parent);
            scoreGlass.Rectangle = new Rectangle(0, 200, 300, 100);

            moneyGlass = new Entity2D(hudSprite, new Vector2(300, bgHudPos.Y), this.Parent);
            moneyGlass.Rectangle = new Rectangle(0, 200, 300, 100);

            score = new Entity2D(hudSprite, new Vector2(10, bgHudPos.Y), this.Parent);
            score.Rectangle = new Rectangle(0, 0, 300, 100);

            money = new Entity2D(hudSprite, new Vector2(300, bgHudPos.Y), this.Parent);
            money.Rectangle = new Rectangle(0, 100, 300, 100);
            

            // Initialize the black Texture
            black = new Entity2D(Engine.Content.Load<Texture2D>("Content\\Textures\\black"),
                    new Vector2(0, 400), this.Parent);
            black.Rectangle = new Rectangle(0, 0, 800, 75);
            black.Position = bgHudPos;
            black.Alpha = 0.3f;
        }

        // Other Methodes =====================================//
        public void updateDamage(float dam)
        {

            // Add damage
            damageValue += dam;
            float damPercentage = damageBarWith * (damageValue / maxDamage);
            damageBar.Rectangle = new Rectangle(0, 100, (int)damPercentage, 50);

            // Clamp the value of the resistance
            if (damageValue < 0f)
                damageValue = 0f;
            else if (damageValue > 100f)
                damageValue = 100f;

        }
        public override void Update()
        {
            if (!isHiding)
            {
                transitionPosition = 0f;

                //scoreText.Position = new Vector2(Engine.Viewport.Width - 550 - sizeOfText(scoreText) / 2, 545);
                scoreText.Text = scoreValue.ToString();

                //moneyText.Position = new Vector2(Engine.Viewport.Width - 245 - sizeOfText(moneyText) / 2, 545);
                moneyText.Text = moneyValue.ToString();
            }
            else
            {
                transitionPosition++;
                if (transitionPosition >= 100)
                {
                    transitionPosition = 100;
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

        public bool gameIsOver()
        {
            if (damageValue <= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void hide()
        {
            isHiding = true;
        }
    }
}
