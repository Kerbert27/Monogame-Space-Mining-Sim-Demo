using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class GameObject {
        public Transform transform {  get; set; }
        public Camera camera { get { return Get<Camera>(); } set { Add<Camera>(value); } }
        public RigidBody rigidbody { get { return Get<RigidBody>(); } set { Add<RigidBody>(value); } }
        public Collider collider { get { return Get<Collider>(); } set { Add<Collider>(value); } }
 
        public Renderer renderer { get { return Get<Renderer>(); } set { Add<Renderer>(value); } }

        private Dictionary<Type, Component> Components { get; set; }
        private List<IUpdateable> Updateables { get; set; }
        private List<IRenderable> Renderables { get; set; }
        private List<IDrawable> Drawables { get; set; }

        public GameObject() {
            transform = new Transform();
            Components = new Dictionary<Type, Component>();
            Updateables = new List<IUpdateable>();
            Drawables = new List<IDrawable>();
            Renderables = new List<IRenderable>();
        
        }

        public T Add<T>() where T : Component, new() {
            Remove<T>();
            T component = new T();
            component.GameObject = this;
            component.Transform = transform;
            Components.Add(typeof(T), component);
            if (component is IUpdateable) Updateables.Add(component as IUpdateable);
            if (component is IRenderable) Renderables.Add(component as IRenderable);
            if (component is IDrawable) Drawables.Add(component as IDrawable);
            return component;
        }

        public T Add<T>(T component) where T : Component {
            Remove<T>();
            component.GameObject = this;
            component.Transform = transform;
            Components.Add(typeof(T), component);
            if (component is IUpdateable) Updateables.Add(component as IUpdateable);
            if (component is IRenderable) Renderables.Add(component as IRenderable);
            if (component is IDrawable) Drawables.Add(component as IDrawable);
            return component;
        }

        public void Remove<T>() where T : Component {
            if (Components.ContainsKey(typeof(T))) {
                Component component = Components[typeof(T)];
                Components.Remove(typeof(T));
                if (component is IUpdateable) Updateables.Remove(component as IUpdateable);
                if (component is IRenderable) Renderables.Remove(component as IRenderable);
                if (component is IDrawable) Drawables.Remove(component as IDrawable);
            }
        }


        public T Get<T>() where T : Component{
            if (Components.ContainsKey(typeof(T))) return Components[typeof(T)] as T;
            else return null;
        }

        public virtual void Update() {
            foreach (IUpdateable c in Updateables) {
                c.Update();
            }
        }

        public virtual void Draw() {
            foreach(IRenderable c in Renderables) {
                c.Draw();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            foreach(IDrawable c in Drawables) {
                c.Draw(spriteBatch);
            }
        }
    }
}
