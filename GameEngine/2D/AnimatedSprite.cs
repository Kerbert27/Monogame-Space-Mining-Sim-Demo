using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class AnimatedSprite : Sprite {

        public int frames { get; private set; }
        public float currentFrame { get; private set; }
        public float speed { get; set; }

        public int totalAnimations;
        public int currentAnimation { get; private set; }
        public int frameWidth { get; private set; }
        public int frameHeight { get; private set; }
        public AnimatedSprite(Texture2D texture, int frames = 1, int animations = 1) : base(texture) {
            this.frames = frames;
            this.currentFrame = 0;
            this.speed = 1;
            this.totalAnimations = animations;
            this.currentAnimation = 1;

            this.frameWidth = Texture.Width / frames;
            this.frameHeight = Texture.Height / totalAnimations;

            Source = new Rectangle((int)(currentFrame * frameWidth), currentAnimation * frameHeight, frameWidth, frameHeight);
        }

        public void SelectAnimation(int animation) {
            if (animation < totalAnimations && animation >= 0)
                currentAnimation = animation;
            
        }

        public override void Update() {
            currentFrame += speed * Time.ElapsedGameTime;
            if (currentFrame >= frames)
                currentFrame = 0;
            Source = new Rectangle((int)currentFrame * frameWidth, currentAnimation * frameHeight, frameWidth, frameHeight );
        }


    }
}
