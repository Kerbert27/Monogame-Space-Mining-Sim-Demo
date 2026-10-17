using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class Skybox {
        public Texture2D SkyboxTexture;
        Model skyboxModel;
        Camera camera;
        Transform transform;
        Renderer renderer;

        public Skybox(Texture2D texture, Model model, Camera cam, ContentManager content) {
            this.SkyboxTexture = texture;
            this.skyboxModel = model;
            this.camera = cam;
            transform = new Transform();
            transform.Scale *= 100000;
            

            renderer = new Renderer(model, transform, camera, content, ScreenManager.GraphicsDevice, "PlanetShader", 1, 0, SkyboxTexture, Light.Default);

        }

        public void Draw() {
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            camera.farPlane = 1000000;
            renderer.Draw();
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }
    }
}
