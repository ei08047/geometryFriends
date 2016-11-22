using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{
    public class Greedy
    {
        Graph graph;
        Node goal, initial;
        Path path = new Path();
        Grid grid;
        Boolean solutionFound = false;
        public Greedy(Graph gr , Node g, Node init, Grid rep) {
            this.graph = gr;
            this.initial = init;
            this.goal = g;
            this.grid = rep;
        }
        public int search() {
            Node current,nextNode;
            current = initial;
            path.path.Add(initial);
            while (!solutionFound)
            {
                ArrayList s = sucessors(current);
                solutionFound = solutionFoundinSucc(s);
                GeometryFriends.Log.LogError("current node value" + grid.locate(current.getState()).value);
                GeometryFriends.Log.LogError("current node number of succ" + s.Count);
                nextNode = getMin(s, grid.locate(current.getState()).value);
                path.path.Add(nextNode);
                current = nextNode;
            }
            return path._path.Count;
        }

        public ArrayList sucessors(Node n)
        {
            ArrayList succ = new ArrayList();
            foreach (Edge s in n.getEdges())
            {
                succ.Add(s.getChild());
            }
            return succ;
        }

        public Node getMin(ArrayList succ,int curr)
        {
            int min = curr;
            Node minNode = new Node();
            foreach (Node n in succ)
            {
               int v = grid.getCurrentCell(n.getState()).value;
                if (v < min)
                {
                    minNode = n;
                    min = v;
                }
            }
            return minNode;
        }

        public Boolean solutionFoundinSucc(ArrayList succ)
        {
            foreach (Node n in succ)
            {
                if (this.grid.locate(n.getState()).value == 0)
                    return true;
            }
            return false;
        }

    }
}
