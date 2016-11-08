using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GeometryFriendsAgents
{
    public class Position
    {
        public float x;
        public float y;

        public Position(float _x, float _y)
        {
            x = _x;
            y = _y;
        }


        public double distance(Position pos)
        {
            double distance;
            distance = Math.Pow(x - pos.x, 2) + Math.Pow(x - pos.x, 2);
            return Math.Sqrt(distance);
        }

        public float Mandistance(float _x, float _y )
        {
            return Math.Abs(x - _x) + Math.Abs(y - _y);
        }

        public float Mandistance(Position p)
        {
            return Math.Abs(x - p.x) + Math.Abs(y - p.y);
        }
    }
}
