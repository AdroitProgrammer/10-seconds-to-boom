using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace _10_Seconds__To_Boom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadContent();
        }

        private List<Keys> KeysPressed = new List<Keys>();
        private InputManager Manager = new InputManager();
        private ScreenManager MyScreen = new ScreenManager();
        private Stopwatch GameTime = new Stopwatch();
        private SpriteBatch spritebatch;
        private int FPS;
        private int fpsCounter;
        private long LastFps;
        private long LastTime;
        private float DeltaTime;
        private Point MP;
        private bool AllowInput;
        private bool ShowDetails = true;
        private bool Paused;
        private bool Clicked;
        private int Interval = 1000 / 60;
        
        private void LoadContent()
        {
            spritebatch = new SpriteBatch(this.Size, this.CreateGraphics());
            MyScreen.LoadContent(Manager);
            Thread Game = new Thread(GameLoop);
            Game.Start();
        }

        private void GameLoop()
        {
            GameTime.Start();
            while (this.Created)
            {
                DeltaTime = GameTime.ElapsedMilliseconds - LastTime;
                LastTime = GameTime.ElapsedMilliseconds;
                Input();
                Logic();
                Render();
                while (GameTime.ElapsedMilliseconds - LastTime < Interval) { }
            }
        }

        private void Input()
        {
            CountFps();
            AllowInput = false;

            if (KeysPressed.Contains(Keys.Escape) && Paused)
                Paused = false;
            else if (KeysPressed.Contains(Keys.Escape) && !Paused)
                Paused = true;
            else if (KeysPressed.Contains(Keys.F5))
            {
                MyScreen.CurrentState = ScreenManager.GameState.Menu;
                MyScreen.MusicPlayer.PlayLooping();
                MyScreen.GameLoss = false;
                Manager.CurrentPlayer.ShotsFired.Clear();
                Manager.CurrentPlayer.X = 50;
                Manager.CurrentPlayer.Y = 300;
                MyScreen.GameWatch.Reset();
                MyScreen.Time = "";
                Manager.GameObjects.Clear();
                Manager.ScoreTimeCounter = 0;
                MyScreen.SetEnenmies();
            }

            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MP = this.PointToClient(Cursor.Position);
                        if(ShowDetails)
                        this.Text = MP.ToString() + " FPS " + FPS.ToString();
                    }));
                }
            }
            catch { Environment.Exit(0); } 
            Manager.Update(FPS, DeltaTime, KeysPressed.ToArray(), MP, GameTime,Clicked,Paused,this.Size);
            Clicked = false;
            KeysPressed.Clear();
            AllowInput = true;
        }

        private void Logic()
        {
            MyScreen.Update(Manager);
        }

        private void Render()
        {
            spritebatch.Begin();
            MyScreen.Draw(spritebatch);
            spritebatch.End();
        }

        private void CountFps()
        {
            if (GameTime.ElapsedMilliseconds - LastFps > 1000)
            {
                LastFps = GameTime.ElapsedMilliseconds;
                FPS = fpsCounter;
                fpsCounter = 0;
            }
            fpsCounter++;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (AllowInput)
                KeysPressed.Add(e.KeyCode);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (AllowInput)
                KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            if (MyScreen.CurrentState == ScreenManager.GameState.Game) 
            Paused = true;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
                Clicked = true;
        }
    }
}
