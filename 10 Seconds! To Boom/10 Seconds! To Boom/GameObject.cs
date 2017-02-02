using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _10_Seconds__To_Boom
{
    class GameObject:Sprite
    {
        public PointF Velocity;
        public const float EnergyLoss = .90f;
        public const float Gravity = 9.8f;
        public bool IsWinningGameObject;
        public bool Direction = false;
        public Animation LeftAni;
        public GameObject(Animation a , int x , int y , int width , int height , bool IWGO)
            :base(a.CurrentFrame,x,y,width,height)
        {
            IsWinningGameObject = IWGO;
            LeftAni = a;
        }

        public void Update(InputManager Im)
        {
            Velocity.Y += Gravity;
            if (Direction == true)
            {
                Velocity.X = 0;
                Velocity.X += 80;
            }
            else
            {
                Velocity.X = 0;
                Velocity.X -= 80;
            }

            LeftAni.Play();
            LeftAni.Update(Im);
            this.Texture = LeftAni.CurrentFrame;
            foreach (Shots s in Im.CurrentPlayer.ShotsFired)
            {
                if (this.ToRec.IntersectsWith(s.ToRec) && IsWinningGameObject)
                {
                    Im.GameWin = true;
                    
                }
                else if (this.ToRec.IntersectsWith(s.ToRec) && !IsWinningGameObject)
                {
                    Im.InstaLose = true;
                }
            }

            Velocity.X *= .65f;

            this.X += Velocity.X * Im.Deltatime;
            this.Y += Velocity.Y  * Im.Deltatime;

            if (this.X < 0)
            {
                this.X = 0;
                Direction = true;
            }
            if (this.X - this.Width > Im.ScreenSize.Width)
            {
                Direction = false;
                this.X = Im.ScreenSize.Width - Width;
            }
        }
    }
}
