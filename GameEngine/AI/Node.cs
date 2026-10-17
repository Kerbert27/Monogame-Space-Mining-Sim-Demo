using Microsoft.Xna.Framework;

namespace CPI311.GameEngine {
    public class Node {
        public Node parent;
        public Vector3 position;
        public bool passable;
        public bool closed;
        public float cost;
        public float distance;
        public int row, col;

        public Node(int r, int c, Vector3 pos) {
            col = c;
            row = r;
            position = pos;

            passable = true;
            cost = 0;
            distance = 0;
            parent = null;
            closed = false;
        }
    }
}
