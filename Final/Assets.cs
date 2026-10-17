using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final {
    public static class Assets {
        public static Model Cube, Sphere, SphereHiRes;
        public static Texture2D Square, Planet, Skybox;
        public static SpriteFont Font;
        public static ContentManager content;

        public static void Load(ContentManager content) {
            Assets.content = content;

            Cube = content.Load<Model>("cube");
            Sphere = content.Load<Model>("Sphere");
            SphereHiRes = content.Load<Model>("SphereHiRes");

            Square = content.Load<Texture2D>("Square");
            Planet = content.Load<Texture2D>("tumblr_oqipige2lS1uo5d9jo1_1280_NRM");
            Skybox = content.Load<Texture2D>("skybox1");

            Font = content.Load<SpriteFont>("font");

        }
    }
}
