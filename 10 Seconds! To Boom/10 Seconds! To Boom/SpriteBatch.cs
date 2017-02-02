using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _10_Seconds__To_Boom
{
    class SpriteBatch
    {
        private BufferedGraphics BGfx;
        private BufferedGraphicsContext BCGFX = BufferedGraphicsManager.Current;
        private Size ClientSize;
        private Graphics GFX;

        public SpriteBatch(Size Csize , Graphics gfx)
        {
            BCGFX.MaximumBuffer = new Size(Csize.Width + 1, Csize.Height + 1);
            BGfx = BCGFX.Allocate(gfx, new Rectangle(Point.Empty,Csize));
            ClientSize = Csize;
            GFX = gfx;
        }

        public void Begin()
        {
            BGfx.Graphics.Clear(Color.Black);
        }

        public void Draw(Bitmap bmp , Rectangle rec)
        {
            BGfx.Graphics.DrawImageUnscaled(bmp, rec);
        }

        public void Draw(Sprite s)
        {
            BGfx.Graphics.DrawImageUnscaled(s.Texture, s.ToRec);
        }

        public void Draw(Bitmap b, int x, int y)
        {
            BGfx.Graphics.DrawImageUnscaled(b, x, y, ClientSize.Width, ClientSize.Height);
        }

        public void DrawBackground(Bitmap b)
        {
            BGfx.Graphics.DrawImageUnscaled(b,new Rectangle(Point.Empty,ClientSize));
        }

        public void DrawRectangle(Rectangle rec , Color c)
        {
            BGfx.Graphics.DrawRectangle(new Pen(c,10), rec);
        }

        public void DrawString(string Message , Point p)
        {
            BGfx.Graphics.DrawString(Message, Form1.DefaultFont, Brushes.Black, p);
        }

        public void DrawStringBig(string Message)
        {
            Font f = new Font(Form1.DefaultFont.OriginalFontName,48, FontStyle.Bold,Form1.DefaultFont.Unit);
            BGfx.Graphics.DrawString(Message, f, Brushes.Black, new Point(0,0));
        }

        public void DrawStringBig(string Message , Point p , Color c)
        {
            Font f = new Font(Form1.DefaultFont.OriginalFontName, 48, FontStyle.Bold, Form1.DefaultFont.Unit);
            BGfx.Graphics.DrawString(Message, f, new SolidBrush(c), p);
        }

        public void FillRectangle(Rectangle rec, Color c)
        {
            BGfx.Graphics.FillRectangle(new SolidBrush(c), rec);
        }

        public void End()
        {
            BGfx.Render(GFX);
        }
    }
}
