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
            Node initialNode = new Node(inicial);

            this.nodes.Add(initialNode);
        }

        public Graph(Cell inicial)
        {
            this.nodes = new ArrayList();
            Node initialNode = new Node(inicial);

            this.nodes.Add(initialNode);
        }

        public void addNode(Cell c) {
            Node initialNode = new Node(c);
            this.nodes.Add(initialNode);
        }

        public Node get_node()
        {
            Node temp = (Node)this.nodes[0];
            return temp;
        }

        public ArrayList getNodes() {
            return nodes;
        }




    }
}
