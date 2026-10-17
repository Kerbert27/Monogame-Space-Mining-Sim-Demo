using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class Popup : GUIElement {
        float lifespan, age;
        bool paused;
        public Popup(Rectangle bounds, Texture2D texture, string text, float lifespan) {
            this.Bounds = bounds;
            this.Texture = texture;
            this.Text = text;
            this.lifespan = lifespan;
            this.age = 0;
            this.Color = Color.White;
            paused = false;
        }

        public Popup(Rectangle bounds, Texture2D texture, string text) {
            this.Bounds = bounds;
            this.Texture = texture;
            this.Text = text;
            this.lifespan = float.MaxValue;
            this.age = 0;
            this.Color = Color.White;
            paused = true;
        }

        public override void Update() {
            if(!paused)
                age += Time.ElapsedGameTime;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font) {
            if(age <= lifespan) { 
                base.Draw(spriteBatch, font);
                spriteBatch.DrawString(font, Text, new Vector2(Bounds.X, Bounds.Y), Color.Black);
            }
        }
    }
}
