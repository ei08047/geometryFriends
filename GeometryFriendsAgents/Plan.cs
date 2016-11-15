using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace GeometryFriendsAgents
{ // not used
    public class Plan
    {
        ArrayList path = new ArrayList();
        Grid worldRep;
        ArrayList actions = new ArrayList();
        Graph p;
        Goal goal;
        Position agentPos;
        int value;
        Boolean active = false;
        int order;
        int collaborative;


        public Plan()
        {
        }



        public void setgoal(Goal g) {
            goal = g;
        }

        public void setWorld(Grid w) {
            worldRep = w;
        }

        public void buildPath() {
            ArrayList agent = new ArrayList();
            agent = worldRep.locate(agentPos);
            // if agent not in world.freeCells
                //dont exist
                //else it does
                //if any path with h > maxhAgent
                    //find another
                    
        }

        public void setAction()
        {

        }

        void evaluate(State value) { return; }
    }
}
