using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using CPI311.GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final {
    public class Planet : GameObject{
        public List<Mineral> minerals;
        public Ship Ship;
        Atmosphere atmosphere;
        Texture2D texture;
        Model model;
        float capacity;
        float rotationSpeed;
        public float radius;

        public Planet(Ship ship, float mass, float radius, Transform transform, Texture2D tex, Model mod, float capacity) {
            this.Ship = ship;
            this.transform = transform;
            transform.Scale *= radius;

            RigidBody rigidbody = new RigidBody();
            rigidbody.Transform = transform;
            rigidbody.Mass = mass;
            Add<RigidBody>(rigidbody);

            SphereCollider collider = new SphereCollider();
            collider.Radius = radius;
            collider.Transform = transform;
            Add<Collider>(collider);


            texture = tex;
            model = mod;

            rotationSpeed = 0.01f;
            this.capacity = capacity;
            this.radius = radius;

            Renderer renderer = new Renderer(model, transform, Game1.Camera, Assets.content, ScreenManager.GraphicsDevice, "PlanetShader", 0, 20f, texture, Light.Default);
            renderer.Material.Ambient = new Vector3(0.03f, 0.01f, 0.03f);
            renderer.Material.SetParameter("NormalMap", texture);
            Add<Renderer>(renderer);

            atmosphere = new Atmosphere(this, radius);

            GenerateMinerals();
        }

        public override void Update() {
            foreach(Mineral m in minerals) {
                m.Update();
            }
            //this.transform.Rotate(Vector3.Up, rotationSpeed * Time.ElapsedGameTime);
            base.Update();
        }

        public override void Draw() {
            foreach(Mineral m in minerals) {
                m.Draw();
            }
            base.Draw();
            //atmosphere.Draw();
        }

        private void GenerateMinerals() {
            minerals = new List<Mineral>();

            for(int i = 0; i < capacity; i++) {
                float theta = Randomizer.Random.NextSingle() * 2 * (float)Math.PI;
                float phi = Randomizer.Random.NextSingle() * 2 * (float)Math.PI;
                float x = transform.Position.X + (float)(radius * Math.Sin(phi) * Math.Cos(theta));
                float y = transform.Position.Y + (float)(radius * Math.Sin(phi) * Math.Sin(theta));
                float z = transform.Position.Z + (float)(radius * Math.Cos(phi));
                Vector3 pointOnSphere = new Vector3(x, y, z);
                MineralType type = Mineral.GetRandomMineral(Randomizer.Random);
                Mineral newMineral = new Mineral(type, new Transform(pointOnSphere), this, Ship, Assets.Cube, Assets.Square, Assets.content);
                newMineral.transform.Scale *= 2f;
                minerals.Add(newMineral);          
            }
        }
    }
}
