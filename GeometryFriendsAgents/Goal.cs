using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryFriends.AI.Perceptions.Information;


namespace GeometryFriendsAgents
{
    class Goal
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





         
    }
}
