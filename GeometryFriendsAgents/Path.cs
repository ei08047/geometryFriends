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
        public void addPath(Node curr, Node next ,Path path)
        {
            ArrayList newPath = new ArrayList();
            newPath.Add(curr);
            foreach (Node n in path.path)
            {
                newPath.Add(n);
            }
            int i = Locate(next);
            i++;
            while (i < this.path.Count)
            {
                newPath.Add(this.path[i]);
            }
            this.path = newPath;
        }
        public void sortPath() {
            path.Sort();
        }
        public int Locate(Node n)
        {
            foreach (Node i in _path)
            {
                if (n.cellId == i.cellId)
                    return _path.IndexOf(i);
            }
             return -1;
        }
        public Node getNextNode()
        {
            currentNodeIndex--;
            return (Node)this.path[currentNodeIndex];
        }


    }
}
