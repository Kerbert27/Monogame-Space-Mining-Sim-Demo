using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using CPI311.GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final {
    public class Atmosphere : GameObject {
        public Planet Planet;
        
        public Atmosphere(Planet planet, float radius) {
            this.Planet = planet;
            this.transform.World = planet.transform.World;
            Vector3 origin = this.transform.Position;
            this.transform.Position = origin + this.transform.Forward;
            //this.transform.Scale = planet.transform.Scale;

            this.transform.Scale *= radius * 2.1f;
           

            Dictionary<string, Object> parameters = new Dictionary<string, object>() {
                { "cameraPosition", Game1.Camera.Transform.Position },
                { "origin" , planet.transform.Position }
            };
            ShaderRenderer shader = new ShaderRenderer(Assets.Cube, this.transform, Game1.Camera, Assets.content, "AtmosphereShader", 0, parameters);
            Add<ShaderRenderer>(shader);
        }

        public override void Draw() {
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            base.Draw();
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }
    }
}
