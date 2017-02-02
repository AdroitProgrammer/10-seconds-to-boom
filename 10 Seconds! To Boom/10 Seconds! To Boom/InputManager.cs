using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace _10_Seconds__To_Boom
{
    class InputManager
    {
        public int Fps;
        public long ScoreTimeCounter;
        public float Deltatime;
        public Keys[] Keyspressed;
        public Point MousePoint;
        public bool Clicked;
        public bool Paused;
        public bool GameWin;
        public Stopwatch GameTime;
        public Player CurrentPlayer;
        public bool InstaLose;
        public List<GameObject> GameObjects = new List<GameObject>();
        public Size ScreenSize;

        public void Update(int fps , float dt , Keys[] kp , Point mp , Stopwatch gt , bool clicked , bool pause ,Size ssize)
        {
            Fps = fps;
            Deltatime = dt / 1000f;
            Keyspressed = kp;
            MousePoint = mp;
            GameTime = gt;
            Clicked = clicked;
            Paused = pause;
            ScreenSize = ssize;
        }
    }
}
