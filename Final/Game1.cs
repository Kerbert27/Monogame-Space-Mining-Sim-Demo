using System.Diagnostics;
using CPI311.GameEngine;
using CPI311.GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



/**
 * TO-DO:
 * -COCKPIT MODEL
 * -GUI FOR SPEED, ALTITUDE, CARGO, GOAL, YAW, PITCH, ROLL
 * -IMPROVED SPEED CALCULATION
 * -MINERAL DEPOSITS
 * -MINING MECHANIC
 * -ATMOSPHERE SHADING
 * -UPGRADE/TRADE MECHANICS
 * 
 */

namespace Final {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        public static SpriteBatch spriteBatch;
        

        public static Camera Camera;
        Planet planet1, planet2, planet3, planet4;
        Ship ship;
        Skybox skybox;
        public static Scene currentScene, mainScene, shopScene;
        int speedTier, laserTier;
        Popup moneyLabel;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            InputManager.Initialize();
            Time.Initialize();
            ScreenManager.Initialize(_graphics);
            Randomizer.Initialize();
            //ScreenManager.Setup(1920, 1200);
            
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            Camera = new Camera();

            Assets.Load(Content);
            mainScene = new Scene(sceneUpdate, sceneDraw);

            speedTier = 1;
            laserTier = 1;

            
            Light.Default.Transform = new Transform(new Vector3(5000, 400, 0));
            ship = new Ship(Camera, Content);
            ship.transform.Position = new Vector3(0, 0, 700);
            planet1 = new Planet(ship, 1, 500, new Transform(Vector3.Zero), Assets.Planet, Assets.SphereHiRes, 500);
            planet2 = new Planet(ship, 1, 300, new Transform(new Vector3(2000, 2000, 50)), Assets.Planet, Assets.SphereHiRes, 400);
            planet3 = new Planet(ship, 1, 1000, new Transform(new Vector3(-10000, -400, -100)), Assets.Planet, Assets.SphereHiRes, 2000);
            planet4 = new Planet(ship, 1, 500, new Transform(new Vector3(200, -4000, -800)), Assets.Planet, Assets.SphereHiRes, 800);
            ship.Planets.Add(planet1);
            ship.Planets.Add(planet2);
            ship.Planets.Add(planet3);
            ship.Planets.Add(planet4);

            skybox = new Skybox(Assets.Skybox, Assets.Sphere, Camera, Content);

            shopScene = new Scene(shopUpdate, shopDraw);
            Button speedUpgrade = new Button(new Rectangle(ScreenManager.Width - 200, 0, 200, 50), Assets.Square, "Upgrade Ship Speed: " + speedTier * 1000);
            Button laserUpgrade = new Button(new Rectangle(ScreenManager.Width - 200, 60, 200, 50), Assets.Square, "Upgrade Laser: " + laserTier * 1000);
            Button sellMinerals = new Button(new Rectangle(ScreenManager.Width - 200, 120, 200, 50), Assets.Square, "Sell Collected Minerals");
            moneyLabel = new Popup(new Rectangle(0, 0, 75, 20), Assets.Square, "$" + ship.Money, float.MaxValue);
            speedUpgrade.Action += UpgradeSpeed;
            laserUpgrade.Action += UpgradeLaser;
            sellMinerals.Action += SellMinerals;
            shopScene.Elements.Add(speedUpgrade);
            shopScene.Elements.Add(laserUpgrade);
            shopScene.Elements.Add(sellMinerals);
            shopScene.Elements.Add(moneyLabel);
            mainScene.Elements.Add(moneyLabel);

            currentScene = mainScene;

        }

        protected override void Update(GameTime gameTime) {
            InputManager.Update();
            Time.Update(gameTime);


            ship.Update();
            planet1.Update();
            planet2.Update();
            planet3.Update();
            planet4.Update();
            currentScene.Update();

            Light.Default.Transform.RotateAround(planet1.transform.Position, Vector3.Up, 0.01f * Time.ElapsedGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            

            skybox.Draw();
            planet1.Draw();
            planet2.Draw();
            planet3.Draw();
            planet4.Draw();
            ship.Draw();

            currentScene.Draw();

            base.Draw(gameTime);
        }

        private void sceneUpdate() {
            foreach(GUIElement e in mainScene.Elements) {
                e.Update();
            }

            if(InputManager.IsKeyPressed(Keys.Tab)) {
                currentScene = shopScene;
            }
        }

        private void sceneDraw() {
            spriteBatch.Begin(depthStencilState:DepthStencilState.Default);
            foreach (GUIElement e in mainScene.Elements) {
                e.Draw(spriteBatch, Assets.Font);
            }
            spriteBatch.End();
        }

        private void shopUpdate() {
            foreach (GUIElement e in shopScene.Elements) {
                e.Update();
            }

            if (InputManager.IsKeyPressed(Keys.Tab)) {
                currentScene = mainScene;
            }
        }

        private void shopDraw() {
            spriteBatch.Begin(depthStencilState: DepthStencilState.Default);
            foreach (GUIElement e in shopScene.Elements) {
                e.Draw(spriteBatch, Assets.Font);
            }
            spriteBatch.End();
        }

        private void UpgradeSpeed(GUIElement e) {
            if (ship.Money >= speedTier * 1000) {
                ship.Money -= speedTier * 1000;
                ship.maxSpeed += speedTier * 10;
                ship.minSpeed += speedTier * 5;
                speedTier++;
                e.Text = "Upgrade Speed: " + speedTier * 1000;
                moneyLabel.Text = "$" + ship.Money;
            }
        }

        private void UpgradeLaser(GUIElement e) {
            if (ship.Money >= speedTier * 1000) {
                ship.Money -= speedTier * 1000;
                ship.laserLeft.Distance += laserTier * 5;
                ship.laserLeft.Strength += laserTier * ship.laserLeft.Strength;
                laserTier++;
                e.Text = "Upgrade Laser: " + laserTier * 1000;
                moneyLabel.Text = "$" + ship.Money;
            }
        }

        private void SellMinerals(GUIElement e) {
            foreach(string name in ship.MineralsCollected.Keys) {
                ship.Money += MineralType.mineralLookup[name].Value * ship.MineralsCollected[name];
                ship.MineralsCollected[name] = 0;
                ship.ProgressBars[name].value = 0;
            }
            moneyLabel.Text = "$" + ship.Money;
        }

    }
}
