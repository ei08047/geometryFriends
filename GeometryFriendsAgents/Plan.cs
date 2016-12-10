using GeometryFriends;
using GeometryFriends.AI.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace GeometryFriendsAgents
{ 
    public class Plan
    {
        public Grid worldRep;
        public Graph p;
        public Path path;
        public Goal goal;
        public State agent;
        public Cell agentCell;
        public int order = 0; //TO BE IMPLEMENTED
        public Boolean finished = false; //TO BE IMPLEMENTED
        public Boolean pathToGoal = false;
        public Boolean active = false;
        public Boolean collaborative=false;

        public Plan()
        {
        }
        /// <summary>
        /// setters
        /// </summary>
        public void setgoal(Goal g) {
            goal = g;
        }
        public void setGridWorld(Grid w) {
            worldRep = w;
        }
        public void setAgent(State a) {
            agent = a;
        }
        public void setGraph(Graph p)
        {
            this.p = p;
        }
        /// <summary>
        /// graph building, pathfinding
        /// </summary>
        /// 
        public Node locateAgent() {
            int id = worldRep.getCurrentCell(agent).id;
            foreach (Node n in p.nodes)
            {
                if (n.cellId == id)
                    return n;
            }
            return null;
        }
        public void logPath()
        { int i = 0;
            while (i < path.path.Count - 2)
            {
                Node cur = (Node)path.path[i];
                Node next = (Node)path.path[i+1];
                Action action = cur.getEdge(next);
                Log.LogInformation("cur" + cur.cellId + "||" + "next" + next.cellId + "-->" + action.getMove());
                i++;
            }
        }

        public void buildPath() {
            Cell goalCell = worldRep.locate(goal.getState());
            Cell agentCell = worldRep.locate(agent.getState());
            //agent not in reach
            if (!worldRep.emptyCells.Contains(agentCell))
            {
                GeometryFriends.Log.LogError( "agent not in free cells -> NO PATH");
                collaborative = true;
            }
            else
            {
                p.prepareSearch(p.getNodeByCellId(goalCell.id),p.getNodeByCellId(agentCell.id),this.worldRep);
                this.path =  p.astar.search();
                if (path == null)
                {
                    GeometryFriends.Log.LogError("NO SOLO PATH -> NO PATH");
                    collaborative = true;
                }
                else {
                    pathToGoal = true;
                }
            }
        }
        public Boolean finishedPlan() {
            return finished;
        }

        public AgentMessage talk()
        {
            AgentMessage m = new AgentMessage("ze", null);

            return m;
        }
        public Action executePlan()
        {
            Log.LogError("executing plan");

            Cell c = worldRep.locate(agent);
            if (c == null)
            {
                Log.LogError("nullllll cell");
            }
            Node current = p.getNodeByCellId(c.id);
            if (current == null)
            {
                Log.LogInformation("null locate"+ p.getNodes().Count);
            }
            else {
                Log.LogInformation("dammmmm" + p.getNodes().Count);
            }
            //Node nextNode = null;
            Log.LogError("located agent at:" + current.cellId + " type " + worldRep.getCellbyId(current.cellId).floor);
            current.eval(this.worldRep);
            //int currentVal = current.value;
            // find equal value node in path -> pathNODE
            //Node pathNode = path.Locate(currentVal );

            //Node nextPathNode = path.getNextNode();
            //Log.LogInformation("vertical value =" + worldRep.getCellbyId(current.cellId).verticalValue);
            Action f = new Action(current.getState(), worldRep.getCellbyId(current.cellId).movement);

            return f;

   //         return pathNode.getEdge(nextPathNode); 

        }
    }
}
