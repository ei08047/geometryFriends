

namespace GeometryFriendsAgents
{
    public class Edge
    {
        private Node _parent;
        private Node _child;
        private int _action; // Specific to each edge, this represents the action that takes the agent from one state to the other (State AKA node)
        public Edge(Node p, Node c, int a)
        {
            this._parent = p;
            this._child = c;
            this._action = a;
        }

        public Edge(Node p, Node c)
        {
            this._parent = p;
            this._child = c;
        }

        public Node getParent()
        {
            return this._parent;
        }

        public Node getChild()
        {
            return this._child;
        }

        public int getAction()
        {
            return this._action;
        }

    }
}

