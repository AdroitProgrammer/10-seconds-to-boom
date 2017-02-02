using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace _10_Seconds__To_Boom
{
    class Player:Sprite
    {
        public PointF Velocity;
        public bool Direction;
        public const float EnergyLoss = .90f;
        public const float Gravity = 9.8f;
        public Animation[] CurrentAnimations;
        public List<Shots> ShotsFired = new List<Shots>();
        public int Jumped = 0;
        public Player(Animation[] animations, int x , int y , int width , int height)
            :base(animations[0].CurrentFrame,x,y,width,height)
        {
            CurrentAnimations = animations;
        }

        public void Update(InputManager Im)
        {

            Velocity.Y += Gravity;
            MovmentUpdate(Im);
            this.X += Velocity.X * Im.Deltatime;
            this.Y += Velocity.Y * Im.Deltatime;
            

            for (int i = 0; i < ShotsFired.Count; i++)
            {
                if (ShotsFired[i].DeSpawn == true)
                {
                    ShotsFired.Remove(ShotsFired[i]);
                    break;
                }
            }

            foreach (Shots s in ShotsFired)
            {
                s.Update(Im);
            }

            if (this.X < 0)
                this.X = 0;
            if (this.X > Im.ScreenSize.Width - this.Width)
                this.X = Im.ScreenSize.Width - Width;
        }

        public void MovmentUpdate(InputManager im)
        {
            foreach (Keys k in im.Keyspressed)
            {
                switch (k)
                {
                    case Keys.Left:
                        this.Velocity.X -= 70;
                        if (Direction == true)
                            this.Texture = Animation.SheetFlip(this.Texture);
                        Direction = false;
                        break;
                    case Keys.Right:
                        this.Velocity.X += 70;
                        
                        if (Direction == false)
                            this.Texture = Animation.SheetFlip(this.Texture);
                        Direction = true;

                        break;
                    case Keys.Up:
                        if(Jumped < 2)
                        Velocity.Y += -400;
                        Jumped++;
                        break;
                    case Keys.Space:
                        Shots s = new Shots(this.X, this.Y + 7, 10, 10, Direction);
                        if (Direction == false)
                        s.Texture = Animation.SheetFlip(s.Texture);
                        ShotsFired.Add(s);
                        break;
                }
            }
        }
        public new void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
            foreach (Shots s in ShotsFired)
            {
                sb.Draw(s);
            }
        }
    }
    class Shots: Sprite
    {
        public bool DeSpawn;
        public PointF Velocity;
        public bool Direction;
        public Shots( float x, float y, int width, int height,bool direction)
            : base(Properties.Resources.Bullet, x, y, width, height)
        {
            // 0 = left 1 = right
            Direction = direction;
            
        }
        public void Update(InputManager im)
        {
            if (Direction == false)
            {
                this.Velocity.X -= 10;
            }
            else
            {
                this.Velocity.X += 10;
            }
            this.X += Velocity.X * im.Deltatime;

            if (this.X < 0)
                this.DeSpawn = true;
            if (this.X > im.ScreenSize.Width - this.Width)
                this.DeSpawn = true;
        }

    }
}
