using System;
using System.Collections;
using System.Linq;
using System.Text;
using GeometryFriends.AI;
using System.Collections.Generic;

namespace GeometryFriendsAgents
{
    /*
    // For each node, the cost of getting from the start node to that node.
    gScore := map with default value of Infinity
    // The cost of going from start to start is zero.
    gScore[start] := 0 
    // For each node, the total cost of getting from the start node to the goal
    // by passing by that node. That value is partly known, partly heuristic.
    fScore := map with default value of Infinity
    // For the first node, that value is completely heuristic.
    fScore[start] := heuristic_cost_estimate(start, goal)

    while openSet is not empty
        current := the node in openSet having the lowest fScore[] value
        if current = goal
            return reconstruct_path(cameFrom, current)

        openSet.Remove(current)
        closedSet.Add(current)
        for each neighbor of current
            if neighbor in closedSet
                continue		// Ignore the neighbor which is already evaluated.
            // The distance from start to a neighbor
            tentative_gScore := gScore[current] + dist_between(current, neighbor)
            if neighbor not in openSet	// Discover a new node
                openSet.Add(neighbor)
            else if tentative_gScore >= gScore[neighbor]
                continue		// This is not a better path.

            // This path is the best until now. Record it!
            cameFrom[neighbor] := current
            gScore[neighbor] := tentative_gScore
            fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal)

    return failure
         */



    public class Idastar
    {
        //   node current node
        public Node agentNode;
        public Node goalNode;
        public Grid rep;
        public Graph graph;
        // The set of nodes already evaluated.
        public ArrayList closed = new ArrayList();
        // The set of currently discovered nodes still to be evaluated.
        public ArrayList open = new ArrayList();

        public Dictionary<Node, int> gCost = new Dictionary<Node, int>();
        public Dictionary<Node, int> fCost = new Dictionary<Node, int>();
        // For each node, which node it can most efficiently be reached from.
        // If a node can be reached from many nodes, cameFrom will eventually contain the
        // most efficient previous step.
        public Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

        //g  the cost to reach current node
        int g = 0;
        //f estimated cost of the cheapest path(root..node..goal)
        public Idastar(Node init , Node final, Grid r,Graph p) {
            agentNode = init;
            goalNode = final;
            rep = r;
            graph = p;
        }
        //h(node)           estimated cost of the cheapest path(node..goal)
        public float heuristic_man(Node node, Node goal )
        {
            float h;
            Position n, g;
            n = new Position(node.getState().getX(), node.getState().getY());
            g = new Position(goal.getState().getX(), goal.getState().getY());
            h = g.Mandistance(g);
            return h;
        }
        public int heuristic_wav(Node node)
        {
            int h;
            h = (rep.getCellbyId(node.cellId)).value;
            return h;
        }
        //cost(node, succ)  step cost function
        public int cost(Node node, Node succ)
        {
            return 1;
        }
        //is_goal(node)     goal test
        public Boolean is_goal(Node node)
        {
            if (agentNode.Equals(node))
                return true;
            else return false;
        }
        /// <summary>
        /// real successors
        /// </summary>
        /// <returns></returns>
        /*
        //successors(node)  node expanding function
        public ArrayList successors(Node n) {
            ArrayList succ = new ArrayList();
            ArrayList moves = new ArrayList();
            moves = rep.getCellbyId(n.cellId).getPossibleMoves();
            int i = 0;
            Action action;
            foreach(Moves m in moves)
            { 
                switch ((int)m)
                {
                    case (int)Moves.NO_ACTION:
                        {
                            action = new Action(n.getState(), Moves.NO_ACTION);
                            action.calculateExpected();
                            break;
                        }
                    case (int)Moves.ROLL_LEFT:
                        {
                            action = new Action(n.getState(), Moves.ROLL_LEFT);
                            action.calculateExpected();
                            break;
                        }
                    case (int)Moves.ROLL_RIGHT:
                        {
                            action = new Action(n.getState(), Moves.ROLL_RIGHT);
                            action.calculateExpected();
                            break;
                        }
                    case (int)Moves.JUMP:
                        {
                            action = new Action(n.getState(), Moves.JUMP);
                            action.calculateExpected();
                            break;
                        }
                    case (int)Moves.GROW:
                        {
                            action = new Action(n.getState(), Moves.GROW);
                            action.calculateExpected();
                            break;
                        }
                    case (int)Moves.MOVE_LEFT:
                        {
                            action = new Action(n.getState(), Moves.MOVE_LEFT);
                            action.calculateExpected();
                            break;
                        }
                    case (int)Moves.MOVE_RIGHT:
                        {
                            action = new Action(n.getState(), Moves.MOVE_RIGHT);
                            action.calculateExpected();
                            break;
                        }
                    case (int)Moves.MORPH_UP:
                        {
                            action = new Action(n.getState(), Moves.MORPH_UP);
                            action.calculateExpected();
                            break;
                        }
                    case (int)Moves.MORPH_DOWN:
                        {
                            action = new Action(n.getState(), Moves.MORPH_DOWN);
                            action.calculateExpected();
                            break;
                        }
                }
            }
            return succ;
            }
            */
        public ArrayList sucessors(Node n)
        {
            ArrayList succ = new ArrayList();
            foreach (Edge s in n.getEdges())
            {
                succ.Add(s.getChild());
            }
            return succ;
        }
        public Node getMin(ArrayList succ)
        {
            int min = 300;
            Node minNode = new Node();
            foreach (Node n in succ)
            {
                int v = rep.getCurrentCell(n.getState()).value;
                if (v < min)
                {
                    minNode = n;
                    min = v;
                }
            }
            return minNode;
        }
        public void init()
        {
            foreach (Node n in graph.getNodes())
            {
                if (gCost.ContainsKey(n) || fCost.ContainsKey(n))
                {
                    gCost[n] = 1000;
                    fCost[n] = 1000;
                }
                else {
                    gCost.Add(n, 1000);
                    fCost.Add(n, 1000);
                }
            }
        }
        //search routine
        public Path search()
        {
            Path p = new Path();
            init();
            open.Add(agentNode);
            int h = heuristic_wav(agentNode);
            gCost[agentNode] = 0;
            fCost[agentNode] = h;

            while (open.Count != 0)
            {
                Node curr = getMin(open);
                if (curr == goalNode)
                {
                    p.path = reconstructPath(curr);
                    return p;
                }
                open.Remove(curr);
                closed.Add(curr);
                ArrayList s = sucessors(curr);
                foreach (Node n in s)
                {
                    if (closed.Contains(n))
                    {
                        continue;
                    }
                    int cTemp = gCost[curr] + cost(curr, n);
                    if (!open.Contains(n))
                    {
                        open.Add(n);
                    }
                    else
                    {
                        if (cTemp >= gCost[n])
                        {
                            continue;
                        }        
                    }

                    if (cameFrom.ContainsKey(n))
                    {
                        cameFrom[n] = curr;
                    }
                    else {
                        cameFrom.Add(n, curr);
                    }
                    gCost[n] = cTemp;
                    fCost[n] = cTemp + heuristic_wav(n);
                }
            }


            return null;
        }
        /*
                 function reconstruct_path(cameFrom, current)
    total_path := [current]
    while current in cameFrom.Keys:
        current := cameFrom[current]
        total_path.append(current)
    return total_path
             */
        public ArrayList reconstructPath(Node current) {
            ArrayList total = new ArrayList();
            total.Add(current);
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                total.Add(current);
            }
            return total;
        }
    }
}
