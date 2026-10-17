using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using CPI311.GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final {
    //public enum MineralType {
    //    Diamond = 500, Gold = 200, Silicon = 100, Platinum = 600, Silver = 300, Iron = 400
    //}
    public struct MineralType {
        public static readonly Dictionary<string, MineralType> mineralLookup = new Dictionary<string, MineralType>();
        public string Name;
        public int Durability;
        public int Value;
        public Color Color;
        public MineralType(string name, int durability, int value, Color col) {
            Name = name; Durability = durability; Value = value; Color = col;
            mineralLookup.Add(name, this);
        }


        public static readonly MineralType Diamond = new MineralType("Diamond", 500, 100, Color.LightSteelBlue);
        public static readonly MineralType Silicon = new MineralType("Silicon", 50, 10, Color.SlateGray);
        public static readonly MineralType Platinum = new MineralType("Platinum", 700, 200, Color.LightPink);
        public static readonly MineralType Iron = new MineralType("Iron", 200, 25, Color.OrangeRed);
        public static readonly MineralType Gold = new MineralType("Gold", 150, 100, Color.Gold);
        public static readonly MineralType Silver = new MineralType("Silver", 300, 75, Color.Silver);

    };


    public class Mineral : GameObject {
        public MineralType Type;
        public Planet Planet;
        public Ship Ship;
        Model model;
        Texture2D texture;
        public ProgressBarGUI mineBar;
        public bool mining;
        bool released = false;
        bool collected = false;
        public Mineral(MineralType type, Transform transform, Planet planet, Ship ship, Model model, Texture2D texture, ContentManager content) {
            this.Planet = planet;
            this.Ship = ship;
            this.transform.Parent = Planet.transform;
            this.transform = transform;
            Type = type;
            this.model = model;
            this.texture = texture;
            mining = false;

            rigidbody = new RigidBody();
            rigidbody.Mass = 1;
            Add<RigidBody>(rigidbody);

            BoxCollider collider = new BoxCollider();
            collider.Transform = this.transform;
            Add<Collider>(collider);

            mineBar = new ProgressBarGUI(new Vector2(5, 1), new Vector2(ScreenManager.Width / 2, 0), Assets.Square, 0, Type.Durability, Type.Name, 50);
            mineBar.Color = Type.Color;
            Game1.mainScene.Elements.Add(mineBar);
            renderer = new Renderer(model, transform, Game1.Camera, content, ScreenManager.GraphicsDevice, "PlanetShader", 0, 20f, texture, Light.Default);
            renderer.Material.Diffuse = Type.Color.ToVector3();
        }

        public override void Update() {
            if (collected) return;

            if(mining && !released) {
                mineBar.IsActive = true;
                mineBar.Decrement();
                if(mineBar.value <= 0) {
                    released = true;
                    this.rigidbody.Impulse = 0.01f * (transform.Position - Planet.transform.Position);
                }
            } else {
                mineBar.IsActive = false;
            }

            if(released) {
                //Debug.WriteLine(this.collider.Transform.Position);
                if (Vector3.Distance(this.transform.Position, Ship.transform.Position) <= Ship.CollectionRadius) {
                    Ship.Collect(Type);
                    collected = true;
                }
            }

                
            base.Update();
        }

        public override void Draw() {
            if (collected) return;
            if(Vector3.Distance(this.transform.Position, Ship.transform.Position) <= 1000)
                base.Draw();
        }


        public static MineralType GetRandomMineral(Random random) {
            int val = (int)random.NextInt64(0, 70);
            if(val < 20) {
                return MineralType.Silicon;
            }
            if(val < 40) {
                return MineralType.Iron;
            }
            if(val < 50) {
                return MineralType.Silver;
            }
            if(val < 60) {
                return MineralType.Gold;
            }
            if(val < 65) {
                return MineralType.Diamond;
            }
            if(val < 70) {
                return MineralType.Platinum;
            }
            return MineralType.Silicon;
        }

        
    }
}
