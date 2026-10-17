using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final {
    public class Laser : GameObject{
        public float Distance = 40f;
        public float Strength = 25f;
        public bool Fired;
        Model model;
        Texture2D texture;
        Ship ship;

       
        public Laser(ContentManager content, Ship ship, Vector3 offset) {
            this.transform = new Transform();

            this.ship = ship;
            this.transform.Parent = ship.transform;
            this.transform.Position += offset;
            this.transform.Scale = new Vector3(1f, -1f, -Distance);

            BoxCollider collider = new BoxCollider();
            collider.Transform = transform;
            Add<Collider>(collider);

            Fired = false;
            model = content.Load<Model>("cube");
            texture = content.Load<Texture2D>("Square");

            //renderer = new Renderer(model, this.transform, Game1.Camera, Assets.content, ScreenManager.GraphicsDevice, "PlanetShader", 2, 0, texture, Light.Default);

            //Add<Renderer>(renderer);
            
        }

        public override void Update() {
            
            foreach(Mineral m in ship.ClosestPlanet.minerals) {
                float? intersection = m.collider.Intersects(new Ray(transform.Position, transform.Forward));
                if (Fired && (intersection != null && intersection <= 1)) {
                    m.mining = true;
                    m.mineBar.speed = Strength;
                } else {
                    m.mining = false;
                }
            }

            //this.transform.Rotate(this.transform.Forward, 0.01f, true);

            base.Update();
        }
        public override void Draw() {
            if (Fired) {
                //renderer.Material.SetParameter("Time", (float)Time.TotalGameTime.TotalSeconds);
                model.Draw(transform.World, Game1.Camera.View, Game1.Camera.Projection);
                base.Draw();
            }
            //ScreenManager.GraphicsDevice.DrawUserIndexedPrimitives<VertexPosition>(PrimitiveType., vertices, 0, 2, indices, 0, 2);
        }
    }
}
