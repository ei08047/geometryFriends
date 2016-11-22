using System;
using System.Collections;
using System.Linq;
using System.Text;
using GeometryFriends.AI.Perceptions.Information;
using System.Collections.Generic;

namespace GeometryFriendsAgents
{
    public class Graph
    {
        public ArrayList nodes;
        public Grid disRep;
        public Path path;
        public Greedy greedy;
        public Idastar astar;


        public Graph()
        {
            this.nodes = new ArrayList();
        }
        public Graph( Grid r)
        {
            disRep = r;
            this.nodes = new ArrayList();
        }
        public Node getNodeByCellId(int id)
        {
            try
            {
                foreach (Node n in getNodes())
                {
                    if (n.cellId == id)
                        return n;
                }
            }
            catch (Exception e)
            {
                GeometryFriends.Log.LogError("could not find node with that id");
            }
            return null;
        }
        public Node getNode(Node any)
        {
            foreach (Node n in this.nodes)
            {
                if (any.cellId == n.cellId)
                    return n;
            }
            return null;
        }
        public ArrayList getNodes()
        {
            return nodes;
        }
        public Node get_root()
        {
            Node temp = (Node)this.nodes[0];
            return temp;
        }

        public void addnode(Node n)
        {
            nodes.Add(n);
        }
        public int addNodes()
        {
            foreach (Cell c in disRep.emptyCells)
            {
                Node n = new Node(new State(c.getXcoord(), c.getYcoord()),c.id);
                for (int i = 0; i < c.adj_id.Count; i++)
                {
                    try
                    {
                        int id = Convert.ToUInt16( c.adj_id[i]);
                        int val = Convert.ToUInt16( c.adj_fc[i]);
                        n.adj.Add(id, val );
                    }
                    catch (Exception e)
                    {
                        GeometryFriends.Log.LogError("got one!" + e.Message.ToString());
                    }
                }
                
                addnode(n);
                
            }

            return nodes.Count;
        }
        public int createEdges()
        {
            int noEdges = 0;
            foreach (Node n in getNodes())
            {
                foreach (KeyValuePair<int, int> entry in n.adj)
                {
                    n.addEdge(getNodeByCellId(entry.Key), entry.Value);
                    noEdges++;
                }
            }
            return noEdges;
        }
        public void prepareSearch( Node g,Node a,Grid rep) 
        {
            //greedy = new Greedy(this, g,a, rep);
            astar = new Idastar( a, g, rep, this);
        }








    }
}
