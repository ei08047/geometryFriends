using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryFriends.AI.Perceptions.Information;


namespace GeometryFriendsAgents
{ // not used
    public class Goal
    {
        public int id;
        public State goal;
        public Goal(CollectibleRepresentation coll, int i)
        {
            goal = new State(coll.X, coll.Y);
            id = i;
        }
        public Goal(State s)
        {
            goal = s;
        }

        public Position getPosition()
        {
            Position p = new Position(goal.getX(), goal.getY());
            return p;
        }
        public State getState()
        {
            return this.goal;
        }
        public Plan makeplan() {
            Plan p = new Plan();
            p.setgoal(this);
            return p;
        }

    }
}
