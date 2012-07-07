using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlutoEngine;
namespace BrickBreaker
{
    public class ShakedCamera : Camera
    {
        // Fields
        private bool isShaking;
        private float shakeMagnitude;
        private float shakeDuration;
        private float shakeFactor;
        private float shakeTimer;
        private Vector3 shakeOffset;

        // Constractors
        public ShakedCamera()
            : base()
        {
            Initialize();
        }
        public ShakedCamera(GameScreen Parent)
            : base(Parent)
        {
            Initialize();
        }
        void Initialize()
        {
            isShaking = false;
            shakeFactor = 1f;
            
        }
        // Functions
        public override void Update()
        {
            base.Update();
            
            // Shoud we shake the camera
            if (isShaking)
            {
                shakeFactor *= -1f;
                shakeTimer += (float)Engine.GameTime.ElapsedGameTime.TotalSeconds;
                
                if (shakeTimer >= shakeDuration)
                {
                    isShaking = false;
                    shakeTimer = shakeDuration;
                    Position = Vector3.Lerp(Position, new Vector3(0, 13, 17), 1f);
                    Target = Vector3.Lerp(Target, new Vector3(0, 0, 3), 0.8f);
                }

                // Compute the progress on a [0,1] range
                float progress = shakeTimer / shakeDuration;

                float magnitude = shakeMagnitude * (1f - (progress * progress));

                shakeOffset = new Vector3(1f , 1f, 0f) * magnitude;

                // Add the offset to Position and Target
                Position += shakeOffset * shakeFactor;
                Target += shakeOffset * shakeFactor;
            }
            
        }

        private float NextFloat()
        {
            return (float)Util.random.NextDouble() * 2f - 1f;
        }

        public void shake(float magnitude, float duration)
        {
            // is shaking
            isShaking = true;

            // Store the Magnitude and duration
            shakeMagnitude = magnitude;
            shakeDuration = duration;

            // Reset the timer
            shakeTimer = 0f;
        }
    }
}
