using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace CPI311.GameEngine {
    public class Material {

        public Effect effect;
        public Texture2D DiffuseTexture;

        public float Shininess { get; set; }
        public Camera Camera { get; set; }
        public Matrix World { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Specular { get; set; }
        public Light Light { get; set; }
        public int Passes { get { return effect.CurrentTechnique.Passes.Count; } }

        public Dictionary<string, Object> Parameters { get; private set; }
        public int CurrentTechnique { get; set; }
        public Material(Matrix world, Camera camera, ContentManager content, String filename, int currentTechnique, float shininess, Texture2D texture, Light light) {
            effect = content.Load<Effect>(filename);
            World = world; 
            Camera = camera;
            DiffuseTexture = texture;
            Shininess = shininess;
            CurrentTechnique = currentTechnique;
            Diffuse = Color.Gray.ToVector3();
            Ambient = Color.Black.ToVector3();
            Specular = Color.White.ToVector3();
            Light = light;
        }

        //public Material(string filename, ContentManager content, Dictionary<string, Object> parameters, int currentTechnique) {
        //    effect = content.Load<Effect>(filename);
        //    CurrentTechnique = currentTechnique;
        //    Parameters = parameters;
        //}

        public void SetParameter(String name, Object value) {
            dynamic val = value;
            try {
                effect.Parameters[name].SetValue(val);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public virtual void Apply(int currentPass) {
            effect.CurrentTechnique = effect.Techniques[CurrentTechnique];
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(Camera.View);
            effect.Parameters["Projection"].SetValue(Camera.Projection);
            effect.Parameters["CameraPosition"].SetValue(Camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(Light.Transform.Position);
            effect.Parameters["DiffuseColor"].SetValue(Diffuse);
            effect.Parameters["AmbientColor"].SetValue(Ambient);
            effect.Parameters["SpecularColor"].SetValue(Specular);
            effect.Parameters["Shininess"].SetValue(Shininess);
            effect.Parameters["DiffuseTexture"].SetValue(DiffuseTexture);
            //effect.Parameters["TextureSize"].SetValue(TextureSize);
            effect.CurrentTechnique.Passes[currentPass].Apply();

        }
        public virtual void ApplyCustom(int currentPass) {
            effect.CurrentTechnique = effect.Techniques[CurrentTechnique];

            foreach(KeyValuePair<string, Object> p in Parameters) {
                dynamic val = p.Value;
                effect.Parameters[p.Key].SetValue(val);
            }

            effect.CurrentTechnique.Passes[currentPass].Apply();

        }
    }
}
