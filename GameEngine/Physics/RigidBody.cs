using Microsoft.Xna.Framework;

namespace CPI311.GameEngine {
    public class RigidBody : Component, IUpdateable {


        public Vector3 Velocity { get; set; }
        public float Mass { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 Impulse { get; set; }

        public RigidBody() { }

        public RigidBody(Vector3 vel, float m, Vector3 acc) {
            Velocity = vel;
            Mass = m;
            Acceleration = acc;
        }
        public void Update() {
            Velocity += Acceleration * Time.ElapsedGameTime + Impulse / Mass;
            Transform.LocalPosition += Velocity * Time.ElapsedGameTime;
            Impulse = Vector3.Zero;
        }


    }
}
