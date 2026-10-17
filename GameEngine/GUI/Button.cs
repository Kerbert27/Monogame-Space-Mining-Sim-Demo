using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class Button : GUIElement {
        public bool IsActive = true;
        public Button(Rectangle bounds, Texture2D texture, string text) {
            this.Bounds = bounds;
            this.Texture = texture;
            this.Text = text;
            this.Color = Color.White;
        }
        public override void Update() {
            if (Bounds.Contains(InputManager.GetMousePosition())) {
                Selected = true;
                if(InputManager.IsMouseReleased(0)) 
                    OnAction();
            }
            else Selected = false;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font) {
            if (IsActive) {
                base.Draw(spriteBatch, font);
                spriteBatch.DrawString(font, Text, new Vector2(Bounds.X, Bounds.Y), Color.Black);
            }
        }
    }
}
