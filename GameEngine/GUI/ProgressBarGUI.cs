using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class ProgressBarGUI : GUIElement {
        public Color BackColor { get; set; }
        public float value { get; set; }
        public float minVal, maxVal;
        public float speed { get; set; }
        public bool IsActive;

        public Vector2 scale;
        public Vector2 position;
        public Rectangle Source;
        public float rotation;


        public ProgressBarGUI(Vector2 scale, Vector2 position, Texture2D texture, float minVal, float maxVal, string text, float speed = 1){
            this.Texture = texture;
            this.minVal = minVal;
            this.maxVal = maxVal;
            this.value = maxVal;
            this.speed = speed;
            this.Text = text;

            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);

            this.position = position;
            this.scale = scale;
            rotation = 0;
           

            IsActive = true;
        }

        public void Increment() {
            value += speed * Time.ElapsedGameTime;
            if (value >= maxVal) value = maxVal;
            if (value <= minVal) value = minVal;
        }
        public void Decrement() {
            value -= speed * Time.ElapsedGameTime;
            if (value > maxVal) value = maxVal;
            if (value <= minVal) value = minVal;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font) {
            if (IsActive) {
                spriteBatch.Draw(Texture, position, Source, BackColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                float percentage = value / (maxVal - minVal);
                spriteBatch.Draw(Texture, position, new Rectangle(0, 0, (int)(Source.Height * percentage), Source.Width), this.Color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Text, position, Color.Black);
            }
        }

    }
}
