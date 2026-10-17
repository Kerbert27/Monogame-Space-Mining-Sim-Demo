using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class GUIElement {
        public delegate void EventHandler(GUIElement sender);
        public event EventHandler Action;
        protected void OnAction() {
            if (Action != null) Action(this);
        }
        public Rectangle Bounds { get; set; }
        public Texture2D Texture { get; set; }
        public String Text { get; set; }
        public bool Selected { get; set; }
        public Color Color { get; set; }
        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont) {
            if (Texture != null)
                spriteBatch.Draw(Texture, Bounds, Selected ? Color.Yellow : Color);
        }
    }
}
