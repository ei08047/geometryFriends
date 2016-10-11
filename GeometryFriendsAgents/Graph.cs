using System;
using System.Collections;
using System.Linq;
using System.Text;
using GeometryFriends.AI.Perceptions.Information;

namespace GeometryFriendsAgents
{
    public class Graph
    {
        private ArrayList nodes;


        public Graph()
        {
            this.nodes = new ArrayList();
        }

        public Graph(State inicial)
        {
            this.nodes = new ArrayList();
            CircleNode initialNode = new CircleNode(inicial);

            this.nodes.Add(initialNode);
        }

    }
}
