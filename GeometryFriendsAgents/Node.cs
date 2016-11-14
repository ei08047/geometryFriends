using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{
    public class Node
    {
        private ArrayList _edges;
        private Node _parent;
        private State _state;

        public Node()
        {
            this._edges = new ArrayList();
            this._parent = null;
            this._state = null;
        }

        public Node(State s)
        {
            this._edges = new ArrayList();
            this._parent = null;
            this._state = s;
        }



        public void addEdge(Node child, int action)
        {
            Edge newEdge = new Edge(this, child, action);
            this._edges.Add(newEdge);
        }

        public void addEdge(Node child)
        {
            Edge newEdge = new Edge(this, child);
            this._edges.Add(newEdge);
        }

        public Boolean isGoalNode(Node goal,float thresh)
        {
            float xDistance = this.getState().getX() - goal.getState().getX();
            float yDistance = this.getState().getY() - goal.getState().getY();
            float total = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2);
            if (total <= thresh)
                return true;
            else
                return false;
        }

        public State getState()
        {
            return _state;
        }


    }
}
