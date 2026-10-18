using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    public class Transform
    {
        private Vector3 localScale;
        private Vector3 localPosition;
        private Quaternion localRotation;
        private Matrix world;

        public Vector3 LocalPosition { get { return localPosition; } set { localPosition = value; UpdateWorld(); } }
        public Vector3 LocalScale { get { return localScale; } set { localScale = value; UpdateWorld(); } }

        public Quaternion LocalRotation { get { return localRotation; } set { localRotation = value; UpdateWorld(); } }

        public Matrix World { get { return world; } set { world = value;} }
        public Vector3 Forward { get { return world.Forward; } }
        public Vector3 Back { get { return world.Backward; } }
        public Vector3 Right { get { return world.Right; } }
        public Vector3 Left { get { return world.Left; } }
        public Vector3 Up { get { return world.Up; } }
        public Vector3 Down { get { return world.Down; } }

        private void UpdateWorld()
        {
            world = Matrix.CreateScale(localScale) *
            Matrix.CreateFromQuaternion(localRotation) *
            Matrix.CreateTranslation(localPosition);

            if (parent != null)
                world *= parent.World;
            foreach (Transform child in Children)
                child.UpdateWorld();
        }

        public Transform()
        {
            parent = null;
            Children = new List<Transform>();
            localPosition = Vector3.Zero;
            localScale = Vector3.One;
            localRotation = Quaternion.Identity;
            UpdateWorld();
        }

        public Transform(Vector3 position) {
            parent = null;
            Children = new List<Transform>();
            localPosition = position;
            localScale = Vector3.One;
            localRotation = Quaternion.Identity;
            UpdateWorld();
        }

        public Vector3 Position {
            get { return World.Translation; }
            set {
                if (Parent == null) {
                    LocalPosition = value;
                }
                else {
                    Matrix total = World;
                    total.Translation = value;
                    LocalPosition = (Matrix.Invert(Matrix.CreateScale(LocalScale) *
                        Matrix.CreateFromQuaternion(LocalRotation)) *
                        total * Matrix.Invert(Parent.World)).Translation;
                }
            }
        }

        public Vector3 Scale {
            get {
                Vector3 scale, pos;
                Quaternion q;
                world.Decompose(out scale, out q, out pos);
                return scale;
            }
            set { 
                
                if (Parent == null) LocalScale = value;
                else {
                    Vector3 scale, pos; Quaternion rot;
                    world.Decompose(out scale, out rot, out pos);
                    Matrix total = Matrix.CreateScale(value) * Matrix.CreateFromQuaternion(rot) * Matrix.CreateTranslation(pos);
                    Vector3 s, t; Quaternion r;
                    (total * Matrix.Invert(
                        Matrix.CreateFromQuaternion(LocalRotation) *
                        Matrix.CreateTranslation(LocalPosition) * Parent.world)
                    ).Decompose(out s, out r, out t);
                    LocalScale = s;
                }

            }
        }


        public Quaternion Rotation {
            get { return Quaternion.CreateFromRotationMatrix(World); }
            set {
                if (Parent == null) LocalRotation = value;
                else {
                    Vector3 scale, pos; Quaternion rot;
                    world.Decompose(out scale, out rot, out pos);
                    Matrix total = Matrix.CreateScale(scale) *
                    Matrix.CreateFromQuaternion(value) *
                    Matrix.CreateTranslation(pos);
                    LocalRotation = Quaternion.CreateFromRotationMatrix(
                    Matrix.Invert(Matrix.CreateScale(LocalScale)) * total *
                    Matrix.Invert(Matrix.CreateTranslation(LocalPosition)
                    * Parent.world));
                }
            }
        }

        public void Rotate(Vector3 axis, float angle, bool isWorldSpace = false) {
            Quaternion q = Quaternion.CreateFromAxisAngle(axis, angle);
            if (isWorldSpace) {
                LocalRotation = q * LocalRotation;
            }
            else LocalRotation = LocalRotation * q;
        }


  
        public void RotateAround(Vector3 target, Vector3 axis, float angle) {
            Vector3 pos = Position;
            Quaternion rot = Quaternion.CreateFromAxisAngle(axis, angle);
            Vector3 diff = pos - target;
            diff = Vector3.Transform(diff, rot);
            Position = target + diff;
        }

        private Transform parent;

        public void RemoveParent() {
            if (parent == null) return;
       
            parent.Children.Remove(this);
            parent = null;
        }

        public Transform Parent {
            get { return parent; }
            set {
                if (parent != null) parent.Children.Remove(this);
                parent = value;
                if (parent != null) parent.Children.Add(this);
                UpdateWorld();
            }
        }
        public List<Transform> Children {  get; set; }

    }
}
