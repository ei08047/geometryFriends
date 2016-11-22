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
        public ArrayList adj_fc = new ArrayList();
        public Boolean roof = false; //means you might have a top colision not implemented
        public Boolean floor = false; // means you can jump                       
        public Boolean seen = false;
        public Boolean goal = false;
        public Boolean obstacle = false; 
        public Boolean edge = false;  // not implemented
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
                if (this.isViz(c,k) && c.obstacle == false)
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
        public float getXcoord()
        {
            return (getX() * 1200) / 40;
        }
        public float getYcoord() {
            return (getY() * 720) / 40;
        }
        public void setAdj(ArrayList free)
        {
            int k = 1;
            ArrayList all =  this.getKViz(free, k);
            int val = 100;
            foreach (Cell c in all)
            {
                if (floor)
                {
                    if (c.upper(this))
                    {
                        this.adj_id.Add(c.id);
                        this.adj_fc.Add(val);
                    }
                    else {
                            this.adj_id.Add(c.id);
                            this.adj_fc.Add(val);
                         }
                }
                else
                {
                    if (c.upper(this))
                    {
                        this.adj_id.Add(c.id);
                        this.adj_fc.Add(val*0.8);
                    }
                    else
                    {
                        this.adj_id.Add(c.id);
                        this.adj_fc.Add(val);
                    }
                }
            }
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
        public Boolean upper(Cell c) {
            if (c.getY() > this.getVectorY())
                return true;
            else
                return false;
        }
        public void clean()
        {
            this.value = 0;
        }
        public ArrayList getPossibleMoves()
        {
            ArrayList possible_moves = new ArrayList();
            if (floor)
            {
                possible_moves.Add(GeometryFriends.AI.Moves.JUMP);
            }
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
