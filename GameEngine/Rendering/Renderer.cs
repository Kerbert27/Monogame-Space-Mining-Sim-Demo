using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class Renderer : Component, IRenderable {
        public Material Material { get; set; }
        public Model ObjectModel;
        public Transform ObjectTransform;
        public Camera Camera { get; set; }
        public Light light;
        public GraphicsDevice g;
        public int CurrentTechnique;

        public Renderer(Model objModel, Transform objTransform, Camera camera, GraphicsDevice graphicsDevice, Material mat, int currentTechnique, Light light) {
            ObjectModel = objModel;
            ObjectTransform = objTransform;
            Camera = camera;
            CurrentTechnique = currentTechnique;
            this.light = light;
            g = graphicsDevice;
            Material = mat;

        }

        public Renderer(Model objModel, Transform objTransform, Camera camera, ContentManager content, GraphicsDevice graphicsDevice, String filename, int currentTechnique, float shininess, Texture2D texture, Light light) {
            ObjectModel = objModel;
            ObjectTransform = objTransform;
            CurrentTechnique = currentTechnique;
            Camera = camera;
            this.light = light;
            g = graphicsDevice;


            if (filename != null)
                Material = new Material(ObjectTransform.World, camera, content, filename, currentTechnique, shininess, texture, light);
            else
                Material = null;
        }
        public virtual void Draw() {
            if (Material != null) {
                Material.Camera = Camera; // Update Material's properties
                Material.World = ObjectTransform.World;
                Material.Light = light;
                for (int i = 0; i < Material.Passes; i++) {
                    Material.Apply(i); // Look at the Material's Apply method
                    foreach (ModelMesh mesh in ObjectModel.Meshes) {
                        foreach (ModelMeshPart part in mesh.MeshParts) {
                            g.SetVertexBuffer(part.VertexBuffer);
                            g.Indices = part.IndexBuffer;
                            g.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, 0, part.PrimitiveCount);
                        }
                    }
                }
            }
            else ObjectModel.Draw(ObjectTransform.World, Camera.View, Camera.Projection);
        }
    }
}
