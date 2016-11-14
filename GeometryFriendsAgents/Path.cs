using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GeometryFriendsAgents
{ // not used
    class Path
    {
        public ArrayList _path;
        public Path()
        {
            _path = new ArrayList();
        }
        public ArrayList path
        {
            get { return _path; }
            set { _path = value; }
        }

        public void AddNewPosition(float x, float y)
        {
            Position p = new Position(x, y);
            path.Add(p);
        }

        public void AddNewPosition(Position p)
        {
            path.Add(p);
        }
    }
}
