using GeometryFriends;
using GeometryFriends.AI.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GeometryFriendsAgents
{
    public class Cell
    {
        public int id;
        public int value;
        public int[] pos = new int[2];
        public float[] realpos = new float[2];
        public int[] vector = new int[2];
        public ArrayList adj_id = new ArrayList();
        public ArrayList adj_action = new ArrayList();
        public Boolean roof = false; //means you might have a top colision not implemented
        public Boolean floor = false; // means you can jump                       
        public Boolean seen = false; // used in flood alg
        public Boolean goal = false;
        public Boolean obstacle = false; 
        public Boolean edge = false;

        /// <summary>
        /// methods
        /// </summary>
        public Cell()
        {
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
        public void set_goal(Boolean val)
        {
            goal = val;
        }
        public ArrayList getKViz(ArrayList nodes,int k) {
            ArrayList kViz = new ArrayList();
            foreach (Cell c in nodes)
            {
                if (this.isViz(c, k) && c.obstacle == false)
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
        public void incVal(int v)
        {
            this.value += v;
        }
        public int getX() {
            return this.pos[0];
        }
        public int getY()
        {
            return this.pos[1];
        }
        public float getXcoord()
        {
            return (getX() * 1200) / 40;
        }
        public float getYcoord() {
            return (getY() * 720) / 40;
        }

        public void setAdjRectangle(ArrayList free)
        {
            int k = 1;
            GeometryFriends.AI.Moves j;
            ArrayList all = this.getKViz(free, k);
            foreach (Cell c in all)
            {
                if (floor)
                {
                    // same level or down
                    if (this.toTheLeft(c))
                    {
                        j = GeometryFriends.AI.Moves.MOVE_LEFT;
                        this.adj_id.Add(c.id);
                        this.adj_action.Add(j);
                    }

                    if (this.toTheRight(c))
                    {
                        j = GeometryFriends.AI.Moves.MOVE_RIGHT;
                        this.adj_id.Add(c.id);
                        this.adj_action.Add(j);
                    }
                } 
            }
        }

        public void setAdjCircle(ArrayList free)
        {
            int k = 10;
            GeometryFriends.AI.Moves j;
            ArrayList all =  this.getKViz(free, k);
            foreach (Cell c in all)
            {
                if (floor)
                {
                    // same level or down
                    if (c.toTheLeft(this))
                    {
                        j = GeometryFriends.AI.Moves.GROW;
                        this.adj_id.Add(c.id);
                        this.adj_action.Add(j);
                    }
                    else
                    {
                        j = GeometryFriends.AI.Moves.GROW;
                        this.adj_id.Add(c.id);
                        this.adj_action.Add(j);
                    }
                }
            }
        }

        public int getVectorX()
        {
            return this.vector[0];
        }
        public int getVectorY()
        {
            return this.vector[1];
        }
        public Boolean upper(Cell c) {
            if (c.getY() > this.getVectorY())
                return true;
            else
                return false;
        }
        public Boolean bellow(Cell c)
        {
            return (!upper(c));
        }
        public Boolean toTheLeft(Cell c)
        {
            if (this.getX() > c.getX())
                return true;
            else
                return false;
        }
        public Boolean toTheRight(Cell c)
        {
            if (this.getX() < c.getX())
                return true;
            else
                return false;
        }
        public void clean()
        {
            this.value = 0;
        }

    }
}
