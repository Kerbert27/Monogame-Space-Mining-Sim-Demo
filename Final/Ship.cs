
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml.Linq;
using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using SharpDX.XAudio2;

namespace Final {
    public class Ship : GameObject {
        public float maxSpeed, minSpeed;
        float Speed;
        public List<Planet> Planets;
        public Planet ClosestPlanet;
        public Dictionary<string, int> MineralsCollected;
        public Dictionary<string, ProgressBarGUI> ProgressBars;
        public float Money;
        public float CollectionRadius;
        public Laser laserLeft, laserRight;
        ProgressBarGUI speedometer;
        
        public Ship(Camera cam, ContentManager content) { 
            camera = cam;
            camera.Transform = transform;
            rigidbody = new RigidBody(Vector3.Zero, 1, Vector3.Zero);
            laserLeft = new Laser(content, this, new Vector3(0, -5, -5));
            //laserRight = new Laser(content, this, new Vector3(2, -5, -5));
            Money = 0;
            MineralsCollected = new Dictionary<string, int>();
            ProgressBars = new Dictionary<string, ProgressBarGUI>();

            CollectionRadius = 5f;
            maxSpeed = 30;
            minSpeed = 2;

            Planets = new List<Planet>();

            speedometer = new ProgressBarGUI(new Vector2(10, 0.5f), new Vector2(300, ScreenManager.Height - 20), Assets.Square, 0, maxSpeed, "");
            speedometer.Color = Color.White;
            Game1.mainScene.Elements.Add(speedometer);

        }

        public override void Update() {
            ClosestPlanet = GetClosestPlanet();
            Speed = minSpeed + ((maxSpeed - minSpeed) / (1 + (float)Math.Pow(Math.E, -0.2f*(Vector3.Distance(transform.Position, ClosestPlanet.transform.Position) - ClosestPlanet.radius - 20))));

            speedometer.value = 0;
            speedometer.Text = "     0" + " km/s";

            if (InputManager.IsKeyDown(Keys.W)) {
                transform.Position += Speed * transform.Forward * Time.ElapsedGameTime;
                speedometer.value = Speed;
                speedometer.Text = "     " + Speed.ToString("0.00") + " km/s";
            }
            if (InputManager.IsKeyDown(Keys.Left)) {
                transform.Rotate(transform.Up, Time.ElapsedGameTime, true);
            }
            if (InputManager.IsKeyDown(Keys.S)) {
                transform.Position += Speed * transform.Back * Time.ElapsedGameTime;
                speedometer.value = Speed;
                speedometer.Text = "     " + Speed.ToString("0.00") + " km/s";
            }
            if (InputManager.IsKeyDown(Keys.Right)) {
                transform.Rotate(transform.Down, Time.ElapsedGameTime, true);
            }
            if (InputManager.IsKeyDown(Keys.Up)) {
                transform.Rotate(transform.Right, Time.ElapsedGameTime, true);
            }
            if (InputManager.IsKeyDown(Keys.Down)) {
                transform.Rotate(transform.Left, Time.ElapsedGameTime, true);
            }
            if (InputManager.IsKeyDown(Keys.D)) {
                transform.Rotate(transform.Forward, Time.ElapsedGameTime, true);
            }
            if (InputManager.IsKeyDown(Keys.A)) {
                transform.Rotate(transform.Back, Time.ElapsedGameTime, true);
                
            }

            speedometer.maxVal = maxSpeed;
            speedometer.Color = Color.Lerp(Color.Red, Color.Blue, Speed / maxSpeed);

            if (InputManager.IsKeyDown(Keys.Space)) {
                laserLeft.Fired = true;
                //laserRight.Fired = true;
            }
            else {
                laserLeft.Fired = false;
                //laserRight.Fired = false;
            }


            laserLeft.Update();
            //laserRight.Update();
            //Debug.WriteLine(this.collider.Transform.Position);
            base.Update();
        }

        public override void Draw() {
            laserLeft.Draw();
            //laserRight.Draw();
            base.Draw();
        }

        public void Collect(MineralType mineral) {
            if(MineralsCollected.ContainsKey(mineral.Name)) {
                MineralsCollected[mineral.Name]++;
                ProgressBars[mineral.Name].value += 1;
            } else {
                MineralsCollected.Add(mineral.Name, 1);
                ProgressBarGUI progressBar = new ProgressBarGUI(new Vector2(5, 1), new Vector2(0, ScreenManager.Height - MineralsCollected.Count * 30), Assets.Square, 0, 10, mineral.Name);
                progressBar.value = 1;
                progressBar.Color = mineral.Color;
                Game1.mainScene.Elements.Add(progressBar);
                ProgressBars.Add(mineral.Name, progressBar);
            }

            this.Money += mineral.Value;

            Popup popup = new Popup(new Rectangle(ScreenManager.Width / 2, ScreenManager.Height / 2, 150, 15), Assets.Square, mineral.Name + " Collected!", 3);
            popup.Color = Color.Multiply(Color.White, 0.5f);
            Game1.mainScene.Elements.Add(popup);

        }

        private Planet GetClosestPlanet() {
            float shortestDistance = float.MaxValue;
            Planet closestPlanet = null;
            foreach(Planet p in Planets) {
                float dist = Vector3.Distance(this.transform.Position, p.transform.Position);
                if (dist < shortestDistance) {
                    shortestDistance = dist;
                    closestPlanet = p;
                }
            }

            return closestPlanet;
        }

    }
}
