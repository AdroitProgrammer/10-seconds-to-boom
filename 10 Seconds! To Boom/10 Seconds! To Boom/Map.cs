using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _10_Seconds__To_Boom
{
    class Map
    {
        public List<Bitmap> BackLayers = new List<Bitmap>();
        public List<Maptile> Tiles = new List<Maptile>();
        public List<Bitmap> TileTextures = new List<Bitmap>();
        public int TileSqr;
     
        int[,] MapGrid = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0},
            {0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };
        public Map(int tilsqr)
        {
            TileSqr = tilsqr;

        }

        public void SetMap()
        {
            int mapwidth = MapGrid.GetLength(1);
            int mapheight = MapGrid.GetLength(0);
            for (int x = 0; x < mapwidth; x++)
            {
                for (int y = 0; y < mapheight; y++)
                {
                    int tileindex = MapGrid[y,x];
                    if (tileindex > 0)
                    {
                        Bitmap texture = TileTextures[tileindex - 1];
                        Maptile m = new Maptile(texture, x * TileSqr, y * TileSqr, TileSqr, TileSqr,tileindex);
                        Tiles.Add(m);
                    }
                }
            }

        }

        public void Update(InputManager im)
        {
            foreach (Maptile m in Tiles)
            {
                m.Update(im);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Bitmap layer in BackLayers)
            {
                sb.DrawBackground(layer);
            }
            foreach (Maptile m in Tiles)
            {
                m.Draw(sb);
            }
        }
    }

    class Maptile:Sprite
    {
        public int TileIndex;
        public Maptile(Bitmap Texture, float x, float y, int width, int height,int ti)
            : base(Texture, x, y, width, height)
        {
            TileIndex = ti;
        }

        public void Update(InputManager im )
        {
            Player p = im.CurrentPlayer;
            foreach (Shots s in im.CurrentPlayer.ShotsFired)
            {
                if (s.ToRec.IntersectsWith(this.ToRec))
                {
                    s.DeSpawn = true;
                }
            }

            foreach (GameObject go in im.GameObjects)
            {
                if (go.Bottom.IntersectsWith(this.Top))
                {
                    go.Y = this.Y - go.Height;
                    go.Velocity.Y = 0;
                }
                else if (go.Left.IntersectsWith(this.Right))
                {
                    go.Direction = true;
                    go.Velocity.X = 0;
                }
                else if(go.Right.IntersectsWith(this.Left))
                {
                    go.Direction = false;
                    go.Velocity.X = 0;
                }
            }

            if(p.Bottom.IntersectsWith(this.Top))
            {
                p.Y = this.Y - p.Height;
                p.Velocity.Y = 0;
                p.Velocity.X *= 0.65f;
                p.Jumped = 0;
            }
            else if(this.Bottom.IntersectsWith(p.Top))
            {
                p.Y = this.Y + this.Height + 1;
                if(p.Velocity.Y == Math.Abs(p.Velocity.Y))
                    p.Velocity.Y /= 2;
                else
                {
                    float num = Math.Abs(p.Velocity.Y);
                    p.Velocity.Y = -(num /2);
                }
                
            }
             else if (this.Left.IntersectsWith(p.Right))
            {
                p.X = this.X - p.Width;
            }
            else if (this.Right.IntersectsWith(p.Left))
            {
                p.X = this.X + this.Width;
            }
        }
    }
}
