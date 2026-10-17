using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine.Managers {
    public static class CameraManager {
        public static Camera CurrentCamera { get; set; }
        private static List<Camera> cameras = new List<Camera>();
        public static void Initialize(Camera camera) {
            CurrentCamera = camera;
            cameras.Add(camera);
        }
    }
}
