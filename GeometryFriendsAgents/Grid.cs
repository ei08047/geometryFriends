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
        public int length = 42;
        public Cell[,] grid = new Cell[42, 42];
        public ArrayList obstacles = new ArrayList();
        public ArrayList emptyCells = new ArrayList();
        Goal goal;
        string agentName;
        public Grid() {
            int id = 0;
            for (int i = 0; i < length; i++)
            {  
                for (int j = 0; j < length; j++)
                {
                    if ( j > 40 || i > 40 || j < 1 || i < 1   )
                    {
                        Cell c = new Cell();
                        c.obstacle = true;
                        grid[i, j] = c;
                        grid[i, j].set_pos(j, i);
                        grid[i, j].set_id(id);
                        id++;
                    }
                    else {
                        Cell c = new Cell();
                        grid[i, j] = c;
                        grid[i, j].set_pos(j, i);
                        grid[i, j].set_id(id);
                        id++;
                    }
                } 
            }

        }
        //getters
        public Cell getCell(int i, int j) {
            return grid[j,i];
        }
        public Cell getCellbyId(int id)
        {
            foreach (Cell c in grid)
            {
                if (c.id == id) {
                    return c;
                }
            }
            return null;
        }
        /// <summary>
        /// gets near Cell
        /// </summary>
        /// <param name="c"></param>
        /// <returns> return -1 if outofbounds or obstacle cell</returns>
        /// 
        public Cell getLeft(Cell c)
        {
            int x = c.pos[0] - 1;
            int y = c.pos[1];
            if (x <= 0)
                return null;
            else
                return grid[y, x];
        }
        public Cell getUp(Cell c)
        {
            int x = c.pos[0];
            int y = c.pos[1] - 1;
            if (y <= 0)
                return null;
            else
                return grid[y, x];
        }
        public Cell getRight(Cell c)
        {
            int x = c.pos[0] + 1;
            int y = c.pos[1];
            if (x >= 40)
                return null;
            else
                return grid[y, x];
        }
        public Cell getDown(Cell c)
        {
            int x = c.pos[0];
            int y = c.pos[1] + 1;
            if (y >= 40)
                return null;
            else
                return grid[y, x];
        }
        public Cell getCurrentCell(State s)
        {
            int i = widthToCells((int)s.getState().getX());
            int j = heightToCells((int)s.getState().getY());
            return getCell(j, i);
        }
        public ArrayList getFreeCells()
        {
            ArrayList nodes = new ArrayList();
            foreach (Cell c in grid)
            {
                if (!c.obstacle) // nao obstaculo
                {
                    nodes.Add(c);
                }
            }
            return nodes;
        }
        //setters
        public void setAgent(string name) { this.agentName = name; }
        public void add(ObstacleRepresentation[] obs)
        {
            foreach (ObstacleRepresentation o in obs)
            {
                int xi = widthToCells(o.X - o.Width / 2);
                int xf = widthToCells(o.X + o.Width / 2);
                int yi = heightToCells(o.Y - o.Height / 2);
                int yf = heightToCells(o.Y + o.Height / 2);
                if (!(xi < 0 || xi > 40 || xf < 0 || xf > 40 || yi < 0 || yi > 40 || yf < 0 || yf > 40))
                {
                    Log.LogInformation("grid update:" + "-Obstacle ->" + "xi:" + xi + "| xf:" + xf + "|yi:" + yi + "|yf:" + yf);
                    //fill array 
                    add_grid_obstacle(xi, xf, yi, yf);
                }
                else {
                    Log.LogInformation("uppsie dozzie");
                }
            }
        }

        public int setFloor() {
            int count = 0;
            foreach (Cell c in grid)
            {
                try
                {
                    if (c.getY() > 20)
                    {
                        c.floor = true;
                        count++;
                    }
                    if (getDown(c).obstacle)
                    {
                        c.floor = true;
                        count++;
                    }
                }
                catch (Exception e)
                {
                    {
                        Console.WriteLine("An error occurred setting floor: '{0}'", e);
                    }
                }
            }
            return count;
        }
        public int setEdges() {
            int count = 0;
            foreach (Cell c in grid)
            {
                try
                {
                    if (getLeft(c).obstacle || getRight(c).obstacle)
                    {
                        c.edge = true;
                        count++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred setting edge: '{0}'", e);
                }
            }
            return count;
        } 
        public int setEmptyCells() {
            int count = 0;
            foreach (Cell c in grid)
            {
                if (!c.obstacle)
                {
                    emptyCells.Add(c);
                    count++;
                }    
            }
            return count;
        }
        public void setAdjMatrix() {
            foreach (Cell c in emptyCells)
            {
                if (agentName == "myCircle")
                {
                    c.setAdjCircle(this.emptyCells);
                }
                else
                {
                    c.setAdjRectangle(this.emptyCells);
                }
            }
       }
        public void add_grid_obstacle(int xi, int xf, int yi, int yf)
        {
            int xDiff = xf - xi;
            int yDiff = yf - yi;

            for (int i = 0; i < xDiff; i++)
            {
                for (int j = 0; j < yDiff; j++)
                {
                    grid[yi + j, xi + i].obstacle = true;
                }
            }
        }

        public void flood(Goal goal)
        {
            this.goal = goal;
            Log.LogInformation("start flood");
            int i = 1; // value increment
            emptyCells = getFreeCells();
            ArrayList viz = new ArrayList();
            // starting at goal cell
            Cell h = locate(goal.getState());
            Log.LogInformation("Goal" + goal.id + " located at(H/W):" + h.pos[0] + " / " + h.pos[1]);

            h.set_value(0);
            h.seen = true;
            // find neighboors
            viz = h.getKViz(emptyCells, i);

            ArrayList nextViz = new ArrayList();
            ArrayList availableCells = emptyCells;
            availableCells.Remove(h);
            while (availableCells.Count != 0)
            {
                foreach (Cell v in viz)
                {
                    if (!v.seen)
                    {
                        v.incVal(i);
                        v.seen = true;
                        availableCells.Remove(v);
                        //get possible neigh
                        ArrayList possibleNeighboors = v.getKViz(availableCells, 1);

                        foreach (Cell n in possibleNeighboors)
                        {
                            if (availableCells.Contains(n) && !n.seen)
                                nextViz.Add(n);
                        }
                    }
                }
                i++;
                foreach (Cell next in nextViz)
                {
                    if (!viz.Contains(next) && availableCells.Contains(next))
                        viz.Add(next);
                }
            }
            setEmptyCells();
            Log.LogInformation("calculating vectors" + emptyCells.Count);
            calculateVectors(emptyCells);
            Log.LogInformation("calculated flood values");
        }
        public void reflood()
        {

            Log.LogInformation("start flood");
            int i = 1; // value increment
            emptyCells = getFreeCells();
            ArrayList viz = new ArrayList();
            // starting at goal cell
            Cell h = locate(goal.getState());

            h.set_value(0);
            h.seen = true;
            // find neighboors
            viz = h.getKViz(emptyCells, i);

            ArrayList nextViz = new ArrayList();
            ArrayList availableCells = emptyCells;
            availableCells.Remove(h);
            while (availableCells.Count != 0)
            {
                foreach (Cell v in viz)
                {
                    if (!v.seen)
                    {
                        v.incVal(i);
                        v.seen = true;
                        availableCells.Remove(v);
                        //get possible neigh
                        ArrayList possibleNeighboors = v.getKViz(availableCells, 1);

                        foreach (Cell n in possibleNeighboors)
                        {
                            if (availableCells.Contains(n) && !n.seen)
                                nextViz.Add(n);
                        }
                    }
                }
                i++;
                foreach (Cell next in nextViz)
                {
                    if (!viz.Contains(next) && availableCells.Contains(next))
                        viz.Add(next);
                }
            }
            setEmptyCells();
            Log.LogInformation("calculating vectors" + emptyCells.Count);
            calculateVectors(emptyCells);
            Log.LogInformation("calculated flood values");
        }
        public void clear()
        {
            foreach (Cell e in grid)
            {
                try
                {
                    e.set_value(0);
                    e.seen = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("merda", ex);
                }

            }
        }
        public void calculateVectors(ArrayList free)
        {
            foreach (Cell c in free)
            {
                calcVectorCell(c);
            }
        }
        public void calcVectorCell(Cell c)
        {
            int left, right, up, down;
            try
            {
                left = getLeft(c).value;
                right = getRight(c).value;
                up = getUp(c).value;
                down = getDown(c).value;
                if ((left == 0 && right > 3))
                {
                    c.vector[0] = 1;
                }
                else
                {
                    if ((right == 0 && left > 3))
                    {
                        c.vector[0] = -1;
                    }
                    else
                    {
                        c.vector[0] = left - right;
                    }
                }
                ///
                if ((up == 0 && down > 3))
                {
                    c.vector[1] = 1;
                }
                else
                {
                    if ((down == 0 && up > 3))
                    {
                        c.vector[1] = 0;
                    }
                    else
                    {
                        c.vector[1] = up - down;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("calc vector", e);
            }
        }
        public Cell locate(State p)
        {
            int i, j;
            i = widthToCells(p.getX());
            j = heightToCells(p.getY());
            return getCell(i, j);
        }

        public void add_grid_agent(int xi, int xf, int yi, int yf)
        {

            int xDiff = xf - xi;
            int yDiff = yf - yi;

            for (int i = 0; i < xDiff; i++)
            {
                for (int j = 0; j < yDiff; j++)
                {
                    grid[yi + j, xi + i].obstacle = true;
                    grid[yi + j, xi + i].rectangle = true;
                }
            }
        }
        public void updateGrid(State p)
        {

            foreach (Cell c in grid)
            {
                try
                {
                    if (c.rectangle)
                    {
                        c.rectangle = false;
                        c.obstacle = false;
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine("ashads", err);
                }

            }

    
            int xi = widthToCells(p.getX() - 2*p.h);
            int xf = widthToCells(p.getX() + 2*p.h);
            int yi = heightToCells(p.getY() - 4*p.h);
            int yf = heightToCells(p.getY() + (p.h)/2);
            add_grid_agent(xi,xf,yi,yf);
        }

        //casters
        public int widthToCells(float width) {

                int temp = (int)((width * 39) / 1200);
                if (temp > 39)
                    temp = 39;
                if (temp < 0)
                    temp = 0;
                return temp;

        }
        public int heightToCells(float height) {
            int temp = (int)((height * 39) / 720);
            if (temp > 39)
                temp = 39;
            if (temp < 0)
                temp = 0;
            return temp;
        }
        public float CelltoWidth(int i)
        {
            float temp = (float)((i * 1200) / 39);
            return temp;
        }
        public float CelltoHeight(int j)
        {
            float temp = (float)((j* 720) / 39);
            return temp;
        }
    }
}
