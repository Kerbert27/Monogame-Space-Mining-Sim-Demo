using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class CheckBox : GUIElement{

        public CheckBox(Rectangle bounds, Texture2D texture, string text) {
            this.Bounds = bounds;
            this.Texture = texture;
            this.Text = text;
        }

        public override void Update() {
            if (Bounds.Contains(InputManager.GetMousePosition())) {
                if (InputManager.IsMouseReleased(0))
                    OnAction();
            }
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont) {
            base.Draw(spriteBatch, spriteFont);
            int width = Math.Min(Bounds.Width, Bounds.Height);
            spriteBatch.Draw(Texture, new Rectangle(Bounds.X, Bounds.Y, width, width), Selected ? Color.Red : Color.White);
            spriteBatch.DrawString(spriteFont, Text, new Vector2(Bounds.X + width, Bounds.Y), Color.Black);
        }
    }
}
