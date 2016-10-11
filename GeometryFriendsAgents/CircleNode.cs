using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{
    public class CircleNode
    {
        private ArrayList _edges;
        private CircleNode _parent;
        private State _state;

        public CircleNode()
        {
            this._edges = new ArrayList();
            this._parent = null;
            this._state = null;
        }

        public CircleNode(State s)
        {
            this._edges = new ArrayList();
            this._parent = null;
            this._state = s;
        }

        public void addEdge(CircleNode child, int action)
        {
            CircleEdge newEdge = new CircleEdge(this, child, action);
            this._edges.Add(newEdge);
        }

    }
}
