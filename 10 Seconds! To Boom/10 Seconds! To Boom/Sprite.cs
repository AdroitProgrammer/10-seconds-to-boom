using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _10_Seconds__To_Boom
{
    class Sprite
    {
        public Bitmap Texture;
        public float X, Y;
        public int Width, Height;

        public Sprite(Bitmap b , float x , float y , int width , int height)
        {
            Texture = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(Texture);
            g.DrawImage(b, 0, 0, width, height);
            g.Dispose();
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle ToRec
        {
            get { return new Rectangle((int)X, (int)Y, Width, Height); }
        }

        public Rectangle Top
        {
            get { return new Rectangle((int)X, (int)Y, Width, Height / 4); }
        }

        public Rectangle Bottom
        {
            get { return new Rectangle((int)X,(int)Y + ((Height / 2 )+ (Height / 4)), Width ,Height / 4); }
        }

        public Rectangle Left
        {
            get { return new Rectangle((int)X, (int)Y, Width / 4, Height); }
        }

        public Rectangle Right
        {
            get { return new Rectangle((int)X+ ((Width / 4) +( Width / 2)),(int)Y, Width / 4, Height); }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
        }
    }
}
