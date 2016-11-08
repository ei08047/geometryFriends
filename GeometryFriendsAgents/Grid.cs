using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryFriends.AI.Perceptions.Information;
using GeometryFriends;
using System.Collections;

namespace GeometryFriendsAgents
{
    public class Grid
    {
        public Cell[,] grid;
        public ArrayList obstacles = new ArrayList();
        public ArrayList emptyCells = new ArrayList();
        public ArrayList goals = new ArrayList();

        public int length = 40;

        public Grid() {
            Log.LogInformation("Grid created");
            grid = new Cell[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    grid[i, j].clean();
                    grid[i, j].set_pos(i, j);
                }
            }
        }

        public Cell getCell(float x, float y) {
            //param x , y
            int _x = widthToCells(x);
            int _y = heightToCells(y);
            return grid[_x, _y];
        }

        public Cell getCellbyId(int id) {

            foreach (Cell c in grid)
            {
                if (c.id == id)
                    return c;
            }
            return null;
        }

        public void add(ObstacleRepresentation[] obs)
        {
            foreach (ObstacleRepresentation o in obs)
            {
                int xi = widthToCells(o.X - o.Width / 2);
                int xf = widthToCells(o.X + o.Width / 2);
                int yi = heightToCells(o.Y - o.Height / 2);
                int yf = heightToCells(o.Y + o.Height / 2);
                Log.LogInformation("grid update:" + "-Obstacle ->" + "xi:" + xi + "| xf:" + xf + "|yi:" + yi + "|yf:" + yf);
                //fill array 
                add_grid(xi, xf, yi, yf, 1);
            }
        }

        public void addCircle(CircleRepresentation circle) {
            int x = (int)circle.X;
            int y = (int)circle.Y;
            getCell(x, y).set_circle(true);
        }

        public void createGraph() {
            ArrayList nodes = new ArrayList();
            foreach (Cell c in grid) {
                if (c.value != 1) // nao obstaculo
                {
                    nodes.Add(c);
                }
            }
            //create edges
            foreach (Cell c in nodes)
            {
                c.setViz(nodes);
            }

            //create graph and add nodes to it
            Graph p = new Graph();
            foreach (Cell c in nodes)
            {
                p.addNode(c); 
            }

            //create edges
            foreach (Node n in p.getNodes())
            {
                ArrayList l = new ArrayList();
                l = n.getCell().viz;
                foreach (Cell c in l)
                {
                    Node m = new Node(c);
                    n.addEdge(m);
                }
            }

            



      






        }

        //checks if obstacle exists beetween a cell and a goal
        public Boolean obstacle_exist() {
            return true;
        }

        public Boolean distance_obstacle_left(int i, int j) {
            if (grid[i - 1, j].value == 1)
                return true;
            else
                return false;
        }

        public Boolean is_obstacle_right(int i, int j)
        {
            if (grid[i + 1, j].value ==  1)
                return true;
            else
                return false;
        }

        public Boolean is_fall_left(int i, int j)
        {
            if (grid[i - 1, j + 1].value == 0)
                return true;
            else
                return false;
        }

        public Boolean is_fall_right(int i, int j)
        {
            if (grid[i + 1, j + 1].value == 0)
                return true;
            else
                return false;
        }

        public Boolean is_walkable_right(int i, int j)
        {
            if (!is_obstacle_right(i, j) && !is_fall_right(i, j))
                return true;
            else
                return false;
        }

        public void addGoals(CollectibleRepresentation[] goals) {
            int i = 0;
            foreach (CollectibleRepresentation g in goals)
            {
                addObjective(g.X , g.Y, i);
                i++;
            }
        }

        public void flood(int goal_id) {
            ArrayList toBeFlooded = new ArrayList();
            getCellbyId(goal_id);

        }

        public void addObjective(float x, float y,int id) {
            getCell(x, y).set_goal(true);
            getCell(x, y).set_id(id);
            goals.Add(getCell(x, y));
        }

        public void setEmptyCells() {
            foreach (Cell c in grid)
            {
                if (c.value == 0)
                    emptyCells.Add(c);
            }
        }


            /*
                /0 - Empty
                /1 - General obstacle
                /2 - Circle Platform
                /3 - Square Platform        
             */
        public void add_grid(int xi, int xf, int yi, int yf,int type)
        {
            int xDiff = xf - xi;
            int yDiff = yf - yi;
            switch (type) {
                default :
                    ArrayList obs = new ArrayList();
                    for (int i = 0; i < xDiff; i++)
                    {
                        for (int j = 0; j < yDiff; j++)
                        {
                           grid[yi + j, xi + i].set_value(type);    
                            obs.Add(grid[yi + j, xi + i]);
                        }
                    }
                    obstacles.Add(obs);
                    break;
            }
            
                   
        }

        public void set_flags()
        {
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {

                    if (grid[i, j].value == 0)
                    {
                        if (j != 0)
                        {
                            //IF TOP IS SOLID -> ROOF
                        }

                        //IF BOTTOM IS SOLID -> FLOOR
                        //IF LEFT SOLID -> LEFT OBSTACLE
                        //IF RIGHT SOLID -> RIGHT OBSTACLE
                    }
                }
            }
        }

        public ArrayList getViz(Cell c) {
            ArrayList ret = new ArrayList();
            int i = c.pos[0];
            int j = c.pos[1];
            if ( i==0 || i==length-1 || j == 0 || j == length-1  ) // outside cells
            {
                if (i == 0)
                {
                    if (j == 0) {
                        ret.Add(grid[i + 1, j + 1]);
                        ret.Add(grid[i + 1, j]);
                        ret.Add(grid[i, j + 1]);
                    }
                    if (j == length - 1)
                    {
                        ret.Add(grid[i + 1, j - 1]);
                        ret.Add(grid[i + 1, j]);
                        ret.Add(grid[i, j + 1]);
                    }
                }
                else {

                }
                
            }
            else
            {

                ret.Add(grid[i, j+1]);
                ret.Add(grid[i+1, j+1]);
                ret.Add(grid[i + 1, j]);
                ret.Add(grid[i + 1, j - 1]);
                ret.Add(grid[i, j - 1]);
                ret.Add(grid[i , j - 1]);
                ret.Add(grid[i - 1, j]);
                ret.Add(grid[i-1, j+1]);
            }
            return ret;
            
        }

        public void updateVectors() {

            foreach (Cell c in grid)
            {
                if (c.value != 1)
                {
                    
                }
                    
            }

        }

        //add / remove circle 
    /*      
        public void addCircleToGrid(int x, int y, int radius)
        {

        }
        public void removeCircle()
        {
            int i, j;
            for (i = 0; i < 40; i++)
            {
                for (j = 0; j < 40; j++)
                {
                    if (grid[i, j]._circle)
                    {
                        grid[i,j].set_value(0);
                    }

                }
            }

        }

    */


        int widthToCells(float width) {
            return  (int)((width / 1200) * 40) - 1;
        }

        int heightToCells(float height) {
            return (int)((height / 720) * 40) -1 ;
        }

        public void printGrid()
        {
            string line;
            for (int i = 0; i < 40; i++)
            {
                line = "";
                for (int j = 0; j < 40; j++)
                {
                    line += string.Format("{0} ", grid[i, j]);

                }
                Log.LogInformation(line);
            }

        }


        public void print2dArray(int[,] array , int _i, int _j)
        {
            string line;
            for (int j = 0; j < _j; j++)
            {
                line = "";
                for (int i = 0; i < _i; i++)
                {
                    line += array[i, j];
                }
                Log.LogInformation("line:" + line);
            }
        }
    }
}
