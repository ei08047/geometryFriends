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
        private Cell _cell;

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

        public Node(Cell c) {
            this._edges = new ArrayList();
            this._parent = null;
            this._cell = c;
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

        public State getState()
        {
            return _state;
        }

        public Cell getCell() {
            return _cell;
        }

    }
}
