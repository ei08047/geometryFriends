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
        public int length = 40;
        public Cell[,] grid = new Cell[40, 40];
        public ArrayList obstacles = new ArrayList();
        public ArrayList emptyCells = new ArrayList();


        public Grid() {
            Log.LogInformation("start grid");
            for (int i = 0; i < 40; i++)
            {  
                for (int j = 0; j < 40 ; j++)
                {
                    grid[i, j] = new Cell();
                    grid[i, j].set_pos(j,i);
                } 
            }
            Log.LogInformation("Grid created");
        }

        public Cell getCell(int x, int y) {
            return grid[x, y];
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

        public void flood(Goal goal) {

            Log.LogInformation("start flood");
            int i = 1; // value increment
            ArrayList freeCells = getFreeCells();
            ArrayList viz= new ArrayList(); // h.getKViz(freeCells, i);
            // starting at goal cell
            // Cell c = getCellbyId(goalId);
            ArrayList g = locate(goal.getPosition() );
            foreach (Cell h in g)
            {
                h.set_value(0);
                h.seen = true;
                // find neighboors
                viz = h.getKViz(freeCells, i);
                freeCells.Remove(h);
                //if not there already add to A list
            }

            ArrayList nextViz = new ArrayList();
            ArrayList availableCells = freeCells;
            
            while (availableCells.Count != 0)
            {
                foreach (Cell v in viz)
                   {
                    if (!v.seen){
                         v.incVal(i);
                         v.seen = true;
                        availableCells.Remove(v);
                        //get possible neigh
                         ArrayList possibleNeighboors = v.getKViz(availableCells, 1);
                        
                            foreach (Cell n in possibleNeighboors)
                            {
                            if( availableCells.Contains(n) && !n.seen)
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
            freeCells = getFreeCells();
            calculateVectors(freeCells);
        }

        public void clear()
        {
            foreach (Cell e in emptyCells)
            {
                e.set_value(0);
            }
        }

        public void calculateVectors(ArrayList free) {
            foreach (Cell c in free)
            {
                calcVectorCell(c);
            }
        }

        public void calcVectorCell(Cell c)
        {
            int left, right, up, down;
            left = getLeftVal(c);
            right = getRightVal(c);
            up = getUpVal(c);
            down = getDownVal(c);
            c.vector[0] = left - right;
            c.vector[1] = up - down;
        }

        /// <summary>
        /// gets near Cell value
        /// </summary>
        /// <param name="c"></param>
        /// <returns> return -1 if outofbounds or obstacle cell</returns>
        /// 
        public int getLeftVal(Cell c)
        {
            int x = c.pos[0] - 1;
            int y = c.pos[1];
            if (x <= 0)
                return -1;
            else
            return grid[y, x].value;
        }

        public int getUpVal(Cell c)
        {
            int x = c.pos[0];
            int y = c.pos[1] -1;
            if (y <= 0 )
                return -1;
            else
                return grid[y,x].value;
        }

        public int getRightVal(Cell c)
        {
            int x = c.pos[0] + 1;
            int y = c.pos[1];
            if (x >= 40 )
                return -1;
            else
                return grid[y, x].value;
        }

        public int getDownVal(Cell c)
        {
            int x = c.pos[0];
            int y = c.pos[1] + 1 ;
            if (y >= 40 )
                return -1;
            else
                return grid[y, x].value;
        }

        public Cell getCurrentCell(State s)
        {
            int i = widthToCells( (int)s.getState().getX());
            int j = heightToCells((int)s.getState().getY ());
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
/*
        public void createGraph() {
            ArrayList nodes = new ArrayList();
            nodes = getFreeCells();      
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
*/


        public ArrayList locate(Position p) {
            ArrayList ret = new ArrayList(); 
            int i, j;
            i = widthToCells(p.x);
            j = heightToCells(p.y);
            ret.Add(getCell(i, j)); //TODO
            return ret;
        
        }

        public void setEmptyCells() {
            foreach (Cell c in grid)
            {
                if (!c.obstacle)
                    emptyCells.Add(c);
            }
        }
            /*
                /0 - Empty
                /1 - General obstacle
                /2 - Circle Platform
                /3 - Square Platform        
             */
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


        public int widthToCells(float width) {
            int temp = (int)( (width * 39) / 1200) ;
            if (temp > 39)
                temp = 39;
            if (temp < 0)
                temp = 0;
            return temp ;
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

        public void printGrid()
        {
            string line;
            for (int i = 0; i < 40; i++)
            {
                line = "";
                for (int j = 0; j < 40; j++)
                {
                    line += string.Format("{0} ", grid[i, j].value);

                }
                Log.LogInformation(line);
            }

        }


    }
}
