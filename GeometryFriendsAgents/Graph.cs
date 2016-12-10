using System;
using System.Collections;
using System.Linq;
using System.Text;
using GeometryFriends.AI.Perceptions.Information;
using System.Collections.Generic;
using GeometryFriends;

namespace GeometryFriendsAgents
{
    public class Graph
    {
        public ArrayList nodes;
        public Grid disRep;
        public Path path;
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
                        Action val = new Action(n.getState(), (GeometryFriends.AI.Moves) c.adj_action[i] );
                        State dest = val.calculateExpected();
                        Cell dest_cell = disRep.locate(dest);
                        int id = dest_cell.id;
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

            ArrayList freeCells = this.disRep.getFreeCells();
            ArrayList floorCells = new ArrayList();
            foreach (Cell f in freeCells)
            {
                if (f.floor)
                {
                    floorCells.Add(f);
                }
            }
            
            int noEdges = 0;
            int i = 0;
            int inc = 1;
            while (floorCells.Count > 1)
            {
                Cell currCell = (Cell)floorCells[0];
                Cell nextCell = (Cell)floorCells[inc];
                // check if possible edge
                Node curr = getNodeByCellId(currCell.id);
                Node next = getNodeByCellId(nextCell.id);
                //curr.addEdge(next, );
                //next.addEdge(curr);
                if (inc >= floorCells.Count)
                {
                    floorCells.Remove(currCell);
                    inc = 1;
                }
                else {
                    inc++;
                }
               
            }

            return -2;
        }
        public void prepareSearch( Node g,Node a,Grid rep) 
        {
            //greedy = new Greedy(this, g,a, rep);
            astar = new Idastar( a, g, rep, this);
        }
        public void prepareSearch_path(ArrayList path, Node curr, Grid rep)
        {
            foreach (Node p in path)
            {
                if (!(curr.value > p.value))
                {
                    prepareSearch(p, curr, rep);
                } 
            }
        }  // hazard
        public void InitValGraph()
        {
            foreach (Node n in nodes)
            {
                Cell temp = this.disRep.getCellbyId(n.cellId);
                n.value =temp.value;
                n.vector[0] = temp.vector[0];
                n.vector[1] = temp.vector[1];



            }
        }
        public void sortGraph()
        {
            nodes.Sort();
        } // NOT USED
        public void pruneGraph(Node value)
        {
            foreach (Node outVal in nodes)
            {
                try
                {
                    if (outVal == value )
                    {
                        nodes.Remove(outVal);
                    }
                }
                catch (Exception e)
                {
                    Log.LogError(e.Message);
                }
                { }
              
            }
        } //not used
        







    }
}
