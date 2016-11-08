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

        public int[] pos = new int[2];

        public int distance;
        public int[,] vector = new int[1, 4];

        public ArrayList viz = new ArrayList(); // lista de vizinhos

        public Boolean roof = false; // means you might have a top colision
        public Boolean floor = false; // means you can jump
        public Boolean left_obs = false;
        public Boolean right_obs = false;
        public Boolean circle = false;
        public Boolean square = false;
        public Boolean goal = false;
        public Boolean edge = false;

        public Cell(int val)
        {
            this.value = val;
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

        public void setViz(ArrayList nodes) {
            foreach (Cell c in nodes)
            {
                if (this.isViz(c))
                {
                    this.viz.Add(c);
                }
            }
        }

        public Boolean isViz(Cell t)
        {
            int xTemp = Math.Abs( this.getX() - t.getX());
            int yTemp = Math.Abs( this.getY() - t.getY());

            if (xTemp == 0 && yTemp == 0)
                return false;
            else {
                if (xTemp < 2 && yTemp < 2)
                { return true; }
            }
            return false;
        }

        public void set_goal(Boolean val) {
            goal = val;
        }

        public void set_circle(Boolean val)
        {
            circle = val;
        }

        public int getX() {
            return this.pos[0];
        }

        public int getY() {
            return this.pos[1];
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
            if (!left_obs)
            {
                possible_moves.Add(GeometryFriends.AI.Moves.ROLL_LEFT);
            }
            if (!right_obs)
            {
                possible_moves.Add(GeometryFriends.AI.Moves.ROLL_RIGHT);
            }
            return possible_moves;
        }



    }
}
