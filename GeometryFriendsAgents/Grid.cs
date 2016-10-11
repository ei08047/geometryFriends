using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryFriends.AI.Perceptions.Information;
using GeometryFriends;

namespace GeometryFriendsAgents
{
    public class Grid
    {
        int[,] grid;

        public Grid() {
            Log.LogInformation("Grid created");
            System.Diagnostics.Debug.WriteLine("zeeee");
            grid = new int[40, 40];
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    grid[i,j] = 0;
                }
            }
        }

        public void add(ObstacleRepresentation [] obs)
        {
            foreach (ObstacleRepresentation o in obs)
            {
                int xi = widthToCells(o.X - o.Width/2);
                int xf = widthToCells(o.X + o.Width/2);
                int yi = heightToCells(o.Y - o.Height/2);
                int yf = heightToCells(o.Y + o.Height/2);
                Log.LogInformation("grid update:" + "xi:"+ xi + "| xf:"+xf + "|yi:"+yi+ "|yf:"+ yf);
                //fill array 
                add_grid(xi, xf, yi, yf);
            }
        }

        public void addCircle(float x, float y) {
            int xi, xf, yi, yf;
            xi = widthToCells(x - 40);
            xf = widthToCells(x + 40);
            yi = heightToCells(y - 40);
            yf = heightToCells(y + 40);
            add_grid(xi, xf, yi, yf);
        }

        public void addObjective() {

        }

        public void add_grid(int xi, int xf, int yi, int yf)
        {
            int xDiff = xf - xi;
            int yDiff = yf - yi;
            for (int i = 0; i < xDiff; i++)
            {
                for (int j = 0; j < yDiff; j++)
                {
                    grid[xi + i, yi + j] = 1;
                }
            }
        }

        int widthToCells(float width) {
            return  (int)((width / 1200) * 40);
        }

        int heightToCells(float height) {
            return (int)((height / 700) * 40);
        }
    }
}
