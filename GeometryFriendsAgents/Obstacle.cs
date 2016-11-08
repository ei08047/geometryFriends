using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{
    public class Obstacle
    {
        Position v1, v2, v3, v4;
        int type;// case 0: general , case 1:circle, case 2:square

        public Obstacle(Position v1, Position v2 , Position v3, Position v4,int type) {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.type = type;
        }
    }
}
