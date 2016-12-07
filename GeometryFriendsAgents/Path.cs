using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GeometryFriends.AI.Debug;
using System.Drawing;
using GeometryFriends;

namespace GeometryFriendsAgents
{
    public class Path
    {

        public ArrayList _path;
        public int currentNodeIndex = 0;
        public Path()
        {
            _path = new ArrayList();
           
        }
        public ArrayList path
        {
            get { return _path; }
            set { _path = value; }
        }
        public Boolean isPathEmpty() {
            if (_path.Count == 0)
                return true;
            else return false;
        }
        public void setIndex(int v) {
            currentNodeIndex = v;
        }
        public void sortPath() {
            path.Sort();
        }
        public Node Locate(int v) 
        {
            foreach (Node i in _path)
            {
                if (i.value == v)
                {
                    currentNodeIndex = _path.IndexOf(i);
                    return (Node)_path[currentNodeIndex];
                }
            }
             return null;
        }
        public Node getNextNode()
        {
            currentNodeIndex--;
            return (Node)this.path[currentNodeIndex];
        }


    }
}
