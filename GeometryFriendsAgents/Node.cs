using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{
    public class Node : IComparable
    {
        public int value;
        public int[] vector = new int[2];
        private ArrayList _edges;
        private Node _parent;
        private State _state;
        public int cellId;
        public Dictionary<int, Action> adj = new Dictionary<int, Action>();

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
        public Node(State s,int cell)
        {
            this._edges = new ArrayList();
            this._parent = null;
            this._state = s;
            this.cellId = cell;
        }
        public void eval(Grid rep)
        {
            Cell c = rep.getCellbyId(cellId);
            this.value = c.value; //TODO:deal with STATE
        }

        public State getState()
        {
            return _state;
        }
        public ArrayList getEdges()
        {
            return _edges;
        }
        public Action getEdge(Node dest)
        {
            foreach (Edge e in _edges)
            {
                if (e.getChild().cellId == dest.cellId)
                {
                    GeometryFriends.Log.LogError("Found ONE", false);
                    return e.getAction();
                }
            }
            GeometryFriends.Log.LogError("COULD NOT FIND THAT EDGE ", false);
            return null;
        }

        public void addEdge(Node child, Action a)
        {
            Edge newEdge = new Edge(this, child, a);
            this._edges.Add(newEdge);
        }
        public Boolean isGoalNode(Node goal,float thresh)
        {
            float xDistance = this.getState().getX() - goal.getState().getX();
            float yDistance = this.getState().getY() - goal.getState().getY();
            float total = (float)Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
            if (total <= thresh)
                return true;
            else
                return false;
        }

        public int CompareTo(object obj)
        {
            return value.CompareTo(obj);
        }
    }
}
