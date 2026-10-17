using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine {
    public class Axis {
        public Keys PositiveKey { get; set; }
        public Keys NegativeKey { get; set; }
        public float value;

        
        public float friction;
        public float speed;
        public float deadzone;
        public float maxSpeed;

        public Axis(Keys p, Keys n) {
            PositiveKey = p;
            NegativeKey = n;
            value = 0;
            speed = 0.5f;
            maxSpeed = 5f;
            friction = 0.2f;
            deadzone = 0.5f;
        }

        public virtual void Update() {
            if (InputManager.IsKeyDown(PositiveKey) && value < maxSpeed) {
                value += speed;
                return;
            }
            if (InputManager.IsKeyDown(NegativeKey) && value > -maxSpeed) {
                value -= speed;
                return;
            }
            if(value >= -deadzone && value <= deadzone) {
                value = 0;
                return;
            }
            if(value < 0) {
                value += friction;
            }
            if(value > 0) {
                value -= friction;
            }
        }
    }
}
