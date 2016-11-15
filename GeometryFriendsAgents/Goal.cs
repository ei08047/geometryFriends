using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryFriends.AI.Perceptions.Information;


namespace GeometryFriendsAgents
{ // not used
    public class Goal
    {

        State goal;

        public Goal(CollectibleRepresentation coll)
        {
            goal = new State(coll.X, coll.Y);
        }

        public Goal(State s)
        {
            goal = s;
        }

        public Plan makeplan() {
            Plan p = new Plan();
            p.setgoal(this);
            return p;
        }

        public Position getPosition() {
            Position p = new Position(goal.getX(), goal.getY());
            return p;
        }





         
    }
}
