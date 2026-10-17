using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace CPI311.GameEngine {
    public class AStar {
        public int cols, rows;
        public Node[,] nodes;
        public Node start, end;
        private SortedDictionary<float, List<Node>> openList;

        public AStar(int r, int c) {
            rows = r; cols = c;
            openList = new SortedDictionary<float, List<Node>>();
            nodes = new Node[rows, cols];
            for(int i = 0; i < r; i++) {
                for(int j = 0; j < c; j++) {
                    nodes[i, j] = new Node(i, j, new Vector3(i, j, 0));
                }
            }
        }

        public void Search() {
            openList.Clear();
            foreach(Node node in nodes) {
                node.closed = false;
                node.cost = Single.MaxValue;
                node.parent = null;
                node.distance = Vector3.Distance(node.position, end.position);
            }

            Push(start);
            while(openList.Count > 0) {
                Node node = Pop();
                if(node == end) {
                    break;
                }
                if(node.row < rows - 1) {
                    Push(nodes[node.row + 1, node.col], node);
                }
                if(node.row > 0) {
                    Push(nodes[node.row - 1, node.col], node);
                }
                if (node.col < cols - 1) {
                    Push(nodes[node.row, node.col + 1], node);
                }
                if (node.col > 0) {
                    Push(nodes[node.row, node.col - 1], node);
                }
            }
        }

        private Node Pop() {
            Node node = openList.ElementAt(0).Value[0];
            openList.ElementAt(0).Value.Remove(node);
            if (openList.ElementAt(0).Value.Count == 0) {
                openList.Remove(node.cost + node.distance);
            }
            node.closed = true;
            return node;
        }

        private void Push(Node node, Node parent = null) {
            if (!node.passable || node.closed) return;
            if (parent == null) node.cost = 0;
            else {
                float cost = parent.cost + 1;
                if (node.cost > cost) {
                    Remove(node);
                    node.cost = cost;
                    node.parent = parent;
                }
                else return;
            }

            float key = node.cost + node.distance;
            if(!openList.ContainsKey(key)) {
                openList[key] = new List<Node>();
            }
            openList[key].Add(node);
        }

        private void Remove(Node node) {
            float key = node.cost + node.distance;
            if(openList.ContainsKey(key)) {
                openList[key].Remove(node);
                if (openList[key].Count == 0) {
                    openList.Remove(key);
                }
            } 

        }


    }
}
