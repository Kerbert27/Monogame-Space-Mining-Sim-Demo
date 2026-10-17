using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine {
    public class Camera : Component{
        private float fieldOfView;
        public float aspectRatio { get; set; }
        public float nearPlane { get; set; }
        public float farPlane { get; set;}

        public float FieldOfView { get { return fieldOfView; } set { if (value > 0 && value < MathHelper.Pi) fieldOfView = value; } }


        public Vector2 Position { get; set; }   //Screen Left-Top Corner
        public Vector2 Size { get; set; }       //Screen Size between 0-1
        public Viewport Viewport {
            get {
                return new Viewport((int)(ScreenManager.Width * Position.X),
                (int)(ScreenManager.Height * Position.Y),
                (int)(ScreenManager.Width * Size.X),
                (int)(ScreenManager.Height * Size.Y));
            }
        }
        public Ray ScreenPointToWorldRay(Vector2 position) {
            Vector3 start = Viewport.Unproject(new Vector3(position, 0),
            Projection, View, Matrix.Identity);
            Vector3 end = Viewport.Unproject(new Vector3(position, 1),
            Projection, View, Matrix.Identity);
            return new Ray(start, end - start);
        }


        public Matrix Projection {
            get {
                return Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlane, farPlane);
            }
        }

        public Matrix View {
            get {
                return Matrix.CreateLookAt(Transform.LocalPosition, Transform.LocalPosition + Transform.Forward, Transform.Up);
            }
        }

        public Camera(Transform transform = null) {
            fieldOfView = MathHelper.PiOver2;   //90 degrees
            aspectRatio = 1.33f;
            nearPlane = 0.1f;
            farPlane = 100000f;
            Position = Vector2.Zero;
            Size = Vector2.One;

            Transform = transform;

        }


    }
}
