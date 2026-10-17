using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CPI311.GameEngine {
    public class ProgressBar : Sprite{
        public Color FillColor {  get; set; }
        public float value { get; set; }
        public float minVal, maxVal;
        public float speed {  get; set; }

        public ProgressBar(Texture2D texture, float minVal, float maxVal, float speed = 1) : base(texture) {
            this.minVal = minVal;
            this.maxVal = maxVal;
            this.value = minVal;
            this.speed = speed;
        }

        public void Increment() {
            value += speed * Time.ElapsedGameTime;
            if(value >= maxVal) value = maxVal;
            if(value <= minVal) value = minVal;
        }

        public void Decrement() {
            value -= speed * Time.ElapsedGameTime;
            if (value >= maxVal) value = maxVal;
            if (value <= minVal) value = minVal;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            float percentage = value / (maxVal - minVal);
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, (int)(Source.Width * percentage), Source.Height), FillColor, Rotation, Origin, Scale, Effects, Layer);

        }

    }
}
