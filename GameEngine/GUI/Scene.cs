using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine {
    public class Scene {
        public delegate void CallMethod();
        public CallMethod Update;
        public CallMethod Draw;
        public List<GUIElement> Elements;
        public Scene(CallMethod update, CallMethod draw) {
            Update = update; 
            Draw = draw;
            Elements = new List<GUIElement>();
        }
    }
}
