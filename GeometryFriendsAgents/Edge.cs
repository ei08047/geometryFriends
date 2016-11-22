

namespace GeometryFriendsAgents
{
    public class Edge
    {
        private Node _parent;
        private Node _child;
        private float weight; // Specific to each edge, this represents the action that takes the agent from one state to the other (State AKA node)
        public Edge(Node p, Node c, float a)
        {
            this._parent = p;
            this._child = c;
            this.weight = a;
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
        public float getAction()
        {
            return this.weight;
        }
    }
}

