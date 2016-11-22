using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{ // TODO
    public class Action
    {
        State inicial, expected, final;
        GeometryFriends.AI.Moves move;
        GeometryFriends.XNAStub.GameTime iniciated;

        public Action(State init, GeometryFriends.AI.Moves move) {
        }
        public State calculateExpected() { return expected; }



    }
}
