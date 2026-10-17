using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine.Managers {
    public static class Randomizer {
        public static Random Random;
        public static void Initialize(int? seed = null) {
            if (seed != null) {
                Random = new Random((int)seed);
            }
            else
                Random = new Random();
        }

        public static Vector3 NextVector3() {
            float x = Random.NextSingle();
            float y = Random.NextSingle();
            float z = Random.NextSingle();
            return new Vector3(x, y, z);
        }
    }
}
