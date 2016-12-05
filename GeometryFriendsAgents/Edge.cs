

namespace GeometryFriendsAgents
{
    public class Edge
    {
        private Node _parent;
        private Node _child;
        private Action action;
        public Edge(Node p, Node c, Action a)
        {
            this._parent = p;
            this._child = c;
            this.action = a;
        }
        public Node getParent()
        {
            return this._parent;
        }
        public Node getChild()
        {
            return this._child;
        }
        public Action getAction()
        {
            return this.action;
        }
    }
}

