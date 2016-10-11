namespace GeometryFriendsAgents
{
    public class CircleEdge
    {
        private CircleNode _parent;
        private CircleNode _child;
        private int _action; // Specific to each edge, this represents the action that takes the agent from one state to the other (State AKA node)

        public CircleEdge(CircleNode p, CircleNode c, int a)
        {
            this._parent = p;
            this._child = c;
            this._action = a;
        }


        public CircleNode getParent()
        {
            return this._parent;
        }

        public CircleNode getChild()
        {
            return this._child;
        }

        public int getAction()
        {
            return this._action;
        }

    }
}

