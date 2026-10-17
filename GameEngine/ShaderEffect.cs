using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class ShaderRenderer : Component, IRenderable {
        public Effect ShaderEffect;
        private Model Model;
       
        private Dictionary<string, Object> Parameters;
        private int CurrentTechnique;
        private GraphicsDevice g;
        private Camera camera;
        public ShaderRenderer(Model model, Transform transform, Camera camera, ContentManager content, String filename, int currentTechnique, Dictionary<string, Object> parameters) {
            ShaderEffect = content.Load<Effect>(filename);
            Parameters = parameters;
            Model = model;
            this.Transform = transform;
            CurrentTechnique = currentTechnique;
            this.camera = camera;
            g = ScreenManager.GraphicsDevice;
        }

        public void Draw() {
            ShaderEffect.CurrentTechnique = ShaderEffect.Techniques[CurrentTechnique];
            ShaderEffect.Parameters["World"].SetValue(this.Transform.World);
            ShaderEffect.Parameters["View"].SetValue(camera.View);
            ShaderEffect.Parameters["Projection"].SetValue(camera.Projection);
            foreach (KeyValuePair<string, Object> kv in Parameters) {
                try {
                    ShaderEffect.Parameters[kv.Key].SetValue((dynamic)kv.Value);
                }
                catch(Exception e) {
                    Debug.WriteLine("Parameter Failed: " + kv.Key);
                    Debug.WriteLine(e.Message);
                }
            }
            ShaderEffect.CurrentTechnique.Passes[0].Apply();
            foreach (ModelMesh mesh in Model.Meshes) {
                foreach (ModelMeshPart part in mesh.MeshParts) {
                    g.SetVertexBuffer(part.VertexBuffer);
                    g.Indices = part.IndexBuffer;
                    g.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, 0, part.PrimitiveCount);
                }
            }
        }
    }
}
