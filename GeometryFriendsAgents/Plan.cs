using GeometryFriends;
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
        public int order = 0;
        public Boolean finished = false;
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
        public void updatePlan(State newState) {
            agent = newState;
        }
        public void buildPath() {
            Cell goalCell = worldRep.locate(goal.getState());
            Cell agentCell = worldRep.locate(agent.getState());
            //agent not in reach
            if (!worldRep.emptyCells.Contains(agentCell))
            {
                GeometryFriends.Log.LogError(" didnt found path");
            }
            else
            {
                p.prepareSearch(p.getNodeByCellId(goalCell.id),p.getNodeByCellId(agentCell.id),this.worldRep);
                this.path =  p.astar.search();
                if (path == null)
                {
                    GeometryFriends.Log.LogError(" didnt found path");
                    collaborative = true;
                    
                }

            }

        }
        public Boolean finishedPlan() {
            return finished;
        }
        public Action executePlan()
        {
            Log.LogError("executing plan");
            Node current = p.getNodeByCellId(worldRep.locate(agent).id);
            Node nextNode = null;
            Log.LogError("located agent at:" + current.cellId + " type " + worldRep.getCellbyId(current.cellId).floor);
            current.eval(this.worldRep);
            int currentVal = current.value;
            int i =path.Locate(current); // very likely to happen
            path.setIndex(i);
            if (path.currentNodeIndex < 0)
            {
                Log.LogError("finished path");
            }
            else {
                nextNode = path.getNextNode();
                Log.LogError("got plan next node at:" + nextNode.cellId);
            }
            if (i == -1)
            {
                Log.LogError("could not locate ");
                ArrayList _p = path.path;
                 p.prepareSearch_path(_p,current,worldRep);
                p.prepareSearch(nextNode, current, this.worldRep);
                Path anotherPath = p.astar.search();
                path.addPath(current , nextNode , anotherPath);
                return current.getEdge(nextNode);

            }
            else
            {
                Log.LogError("located in plan ");
                Log.LogError("got plan state index:"  + this.path.currentNodeIndex);
                //try ti find a edge
                return current.getEdge(nextNode);
            }
        }
    }
}
