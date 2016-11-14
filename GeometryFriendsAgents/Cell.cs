using System;
using System.Collections;
using System.Linq;
using System.Text;


namespace GeometryFriendsAgents
{
    public class Cell
    {
        public int id;
        public int value;
        public String drawable="";
        public int[] pos = new int[2];
        public int[] vector = new int[2];

        public Boolean roof = false; // means you might have a top colision
        public Boolean floor = false; // means you can jump

        public Boolean seen = false;

        public Boolean circle = false;
        public Boolean square = false;
        public Boolean goal = false;
        public Boolean obstacle = false;

        public Boolean edge = false;

        public Cell()
        {
        }

        public String getDrawable()
        {
            if (circle)
            {
                drawable = "C";
            }
            if (goal)
            {
                drawable = "G";
            }
            else
            {
                drawable = value.ToString();
            }
            return drawable;
        }

        public void set_value(int val)
        {
            this.value = val;
        }

        public void set_id(int d) {
            id = d;
        }

        public void set_pos(int i, int j) {
            pos[0] = i;
            pos[1] = j;
        }

        public ArrayList getKViz(ArrayList nodes,int k) {
            ArrayList kViz = new ArrayList();
            foreach (Cell c in nodes)
            {
                if (this.isViz(c,k))
                {
                    kViz.Add(c);
                }
            }
            return kViz;
        }

        public Boolean isViz(Cell t, int k)
        {
            int xTemp = Math.Abs( this.getX() - t.getX());
            int yTemp = Math.Abs( this.getY() - t.getY());
            int level = Math.Max(xTemp, yTemp);
            if (level == k)
                return true;
            else
                return false;
        }


        public void set_goal(Boolean val) {
            goal = val;
            drawable = "G";
        }

        public void set_circle(Boolean val)
        {
            circle = val;
            drawable = "C";
        }

        public void incVal(int v)
        {
            this.value += v;
        }

        public int getX() {
            return this.pos[0];
        }

        public int getY() {
            return this.pos[1];
        }

        public int getVectorX()
        {
            return this.vector[0];
        }

        public int getVectorY()
        {
            return this.vector[1];
        }

        public void clean()
        {
            this.value = 0;
        }

        public ArrayList getPossibleMoves()
        {
            ArrayList possible_moves = new ArrayList();
            //if (floor)
           // {
                possible_moves.Add(GeometryFriends.AI.Moves.JUMP);
            //}
            //if (!left_obs)
            //{
                possible_moves.Add(GeometryFriends.AI.Moves.ROLL_LEFT);
            //}
            //if (!right_obs)
            //{
                possible_moves.Add(GeometryFriends.AI.Moves.ROLL_RIGHT);
            //}
            return possible_moves;
        }

    }
}
