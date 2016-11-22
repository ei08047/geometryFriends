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
        //public int value =100; //TODO
        //Boolean active = false; //TODO
        public int order; 
        //int collaborative; //TODO


        public Plan()
        {
        }
        /// <summary>
        /// getters e setters
        /// </summary>
        public void setAction()
        {

        }
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
        public void updatePlan() {

        }

        public void buildPath() {
            Cell goalCell = worldRep.locate(goal.getState());
            Cell agentCell = worldRep.locate(agent.getState());
            GeometryFriends.Log.LogError("build path debug -- free cells:" + worldRep.emptyCells.Count);
            GeometryFriends.Log.LogError("build path debug -- agent cell:" + agentCell.id);
            GeometryFriends.Log.LogError("build path debug -- goal cell:" + goalCell.id);
            if (!worldRep.emptyCells.Contains(agentCell))
            {
                GeometryFriends.Log.LogError(" didnt found path");
            }
            else
            {
                p.prepareSearch(p.getNodeByCellId(goalCell.id),p.getNodeByCellId(agentCell.id),this.worldRep);
                this.path =  p.astar.search();
            }
                    
        }
        public void executePlan()
        {

        }

/*
        void evaluatePlan() {
            if (path == null)
                value = 0; 
        }
        */
    }
}
