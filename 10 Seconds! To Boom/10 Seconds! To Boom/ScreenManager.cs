using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Media;

namespace _10_Seconds__To_Boom
{
    class ScreenManager
    {
        public List<ScreenItem> OnScreenItems = new List<ScreenItem>();
        public enum GameState { Splash , Menu , Paused  , Game }
        public SoundPlayer MusicPlayer;
        public int SpawnNumber = 4;
        public string Statment;
        public GameState CurrentState = GameState.Menu;
        public Stopwatch GameWatch = new Stopwatch();
        public bool SetEnenimies;
        public bool GameLoss;
        public InputManager Manager;
        bool playing;
        public String Time;
        private Player Player1;
        public int tilesqr = 839 / 16;
        Map GameMap;

        public ScreenManager()
        {

        }

        public void LoadContent(InputManager im)
        {
            Manager = im;
            int tilesqr = 839 / 16;
            MusicPlayer = new SoundPlayer(Properties.Resources.Ludum_Dare_Clear_Music);
            ScreenItem Start = new ScreenItem(Properties.Resources.Start,300,150,200,75);
            ScreenItem Credits = new ScreenItem(Properties.Resources.Credits, 300, 250, 200, 75);
            ScreenItem About = new ScreenItem(Properties.Resources.About,300,350,200,75);
            ScreenItem Title = new ScreenItem(Properties.Resources._10_seconds_title, 200, 50, 400, 65);
            OnScreenItems.Add(Start);
            OnScreenItems.Add(Credits);
            OnScreenItems.Add(About);
            OnScreenItems.Add(Title);
            Animation left = new Animation(Properties.Resources.CharWalkAnimationSheet, 5, 7);
            Animation right = new Animation(Animation.SheetFlip(Properties.Resources.CharWalkAnimationSheet),5,7);
            Animation Gunshotleft = new Animation(Properties.Resources.GunFireAnimationSheet,5,10);
            Animation gunshotright = new Animation(Animation.SheetFlip(Properties.Resources.GunFireAnimationSheet),5,10);
            Animation BoxWalkLeft = new Animation(Properties.Resources.BoxWalk, 2, 2);
            Animation BoxWalkRight = new Animation(Animation.SheetFlip(Properties.Resources.BoxWalk),2,2);
            Player1 = new Player(new Animation[4]{left,right,Gunshotleft,gunshotright}, 100, 100, tilesqr, tilesqr);
            GameMap = new Map(tilesqr);
            GameMap.TileTextures.Add(Properties.Resources.Dabes);
            GameMap.BackLayers.Add(Properties.Resources.background);
            GameMap.SetMap();
            im.CurrentPlayer = Player1;
        }



        public void Update(InputManager im)
        {
            if (im.Paused == true && CurrentState == GameState.Game)
                CurrentState = GameState.Paused;

            switch (CurrentState)
            {
                case GameState.Splash:
                    break;
                case GameState.Menu:
                    MenuUpdate(im);
                    break;
                case GameState.Paused:
                    if (!im.Paused)
                        CurrentState = GameState.Game;
                    break;
                case GameState.Game:
                    GameUpdate(im);
                    break;
            }
           
            
        }

        public void SetEnenmies()
        {
            Random rnd = new Random();
            Random Rnd2 = new Random();
            Animation a = new Animation(Properties.Resources.BoxWalk,2,2);
            int first = rnd.Next(0, 200);
            int second = Rnd2.Next(0, 200);
            int result = first + second;
            Statment = first.ToString() + " + " + second.ToString();
            Font f = new Font(Form1.DefaultFont.OriginalFontName, 22, FontStyle.Bold, Form1.DefaultFont.Unit);
            for (int i = 0; i < SpawnNumber; i++)
            {
                Bitmap f1 = a.Frames[0].Clone(new Rectangle(0,0,a.Frames[0].Width,a.Frames[0].Height),a.Frames[0].PixelFormat);
                Bitmap f2 = a.Frames[1].Clone(new Rectangle(0, 0, a.Frames[1].Width, a.Frames[1].Height), a.Frames[1].PixelFormat); ;
                if (i == 0)
                {
                    Random rndx = new Random();
                    Graphics g = Graphics.FromImage(f1);
                    g.DrawString(result.ToString(), f, Brushes.Red, Point.Empty);
                    g.Dispose();

                    Graphics gr = Graphics.FromImage(f2);
                    gr.DrawString(result.ToString(), f, Brushes.Red, Point.Empty);
                    gr.Dispose();

                    Animation newa = new Animation(new Bitmap[2]{ f1, f2 }, 2);

                    GameObject go = new GameObject(newa, rndx.Next(100,400), 50,tilesqr , tilesqr,true);
                    Manager.GameObjects.Add(go);
                }
                else
                {
                    Random rnddabes = new Random();
                    int newresult = rnddabes.Next(100, 200);
                    Graphics gc = Graphics.FromImage(f1);
                    gc.DrawString(newresult.ToString(), f, Brushes.Red, Point.Empty);
                    gc.Dispose();

                    Graphics grc = Graphics.FromImage(f2);
                    grc.DrawString(newresult.ToString(), f, Brushes.Red, Point.Empty);
                    grc.Dispose();
                    Animation newa = new Animation(new Bitmap[2] { f1, f2 }, 2);
                    Random rnd12 = new Random();
                    Random Rnd13 = new Random();
                    GameObject go = new GameObject(newa, rnd12.Next(300,500),Rnd13.Next(0,200), tilesqr, tilesqr,false);
                    Manager.GameObjects.Add(go);
                }

                
            }

        }

        private void GameUpdate(InputManager im)
        {
            if(SetEnenimies == false)
            {
                SetEnenmies();
                SetEnenimies = true;
            }
            if (im.InstaLose == true)
            {
                im.InstaLose = false;
                GameLoss = true;
            }

            if (!GameLoss)
            {
                GameWatch.Start();
                if (im.GameWin == true)
                {
                    im.ScoreTimeCounter += 100;
                    SetLevel(im);
                }
                if (GameWatch.ElapsedMilliseconds / 1000 > 10)
                {
                    GameLoss = true;
                }

                Time = Convert.ToString(GameWatch.ElapsedMilliseconds / 1000);
                Player1.Update(im);
                foreach (GameObject obj in im.GameObjects)
                {
                    obj.Update(im);
                }
                GameMap.Update(im);
            }
        }

        private void MenuUpdate(InputManager im)
        {
            if (!playing)
            {
                playing = true;
                MusicPlayer.PlayLooping() ;
            }
            for (int i = 0; i < OnScreenItems.Count; i++)
            {
                OnScreenItems[i].Update(im);
                if (OnScreenItems[i].Pressed)
                {
                    switch (i)
                    {
                        case 0:
                            MusicPlayer.Stop();
                            CurrentState = GameState.Game;
                            
                            OnScreenItems[i].Pressed = false;
                            break;
                        case 1:
                            MessageBox.Show("Music , Art , and Programming done by Jonathan Butler."
                                + "Special Thanks to Stefone Cobb , Janay Davis , and Kevin Apolo","Special Thanks",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            OnScreenItems[i].Pressed = false;
                            break;
                        case 2:
                            MessageBox.Show("Space To shoot |Up to Jump |Left And Right to Move | ESC to Pause","Instructions",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            OnScreenItems[i].Pressed = false;
                            break;
                        case 3:
                            MessageBox.Show("Game created for ludum dare 27 created by Jonathan Butler","10 Seconds to Boom",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            OnScreenItems[i].Pressed = false;
                            break;
                    }
                }
            }
        }

        private void SetLevel(InputManager  im)
        {
            Player1.X = 50;
            Player1.Y = 300;
            im.GameWin = false;
            Manager.CurrentPlayer.ShotsFired.Clear();
            Manager.CurrentPlayer.X = 50;
            Manager.CurrentPlayer.Y = 300;
            GameWatch.Reset();
            Time = "";
            Manager.GameObjects.Clear();
            SetEnenmies();
        }

        public void Draw(SpriteBatch spritebatch)
        {
            switch (CurrentState)
            {
                case GameState.Splash:
                    break;
                case GameState.Menu:
                    spritebatch.DrawBackground(Properties.Resources._10_secs_to_boom);
                    foreach (ScreenItem i in OnScreenItems)
                    {
                        i.Draw(spritebatch);
                    }
                    break;
                case GameState.Paused:
                    spritebatch.Draw(Properties.Resources.Paused,20,0);;
                    break;
                case GameState.Game:
                    GameMap.Draw(spritebatch);
                    foreach (GameObject obj in Manager.GameObjects)
                    {
                        obj.Draw(spritebatch);
                    }
                    Player1.Draw(spritebatch);
                    spritebatch.DrawString("Score:" +Manager.ScoreTimeCounter.ToString() +" Time: " + Time,new Point(700,0));
                    spritebatch.DrawStringBig(Statment, new Point(100, 0), Color.Red);
                    if (GameLoss)
                        spritebatch.DrawStringBig("Game Loss Press F5");
                    break;
            }
        }
    }

    class ScreenItem:Sprite
    {
        public bool Clicked;
        public bool Hovering;
        public bool Pressed;
        public ScreenItem(Bitmap b ,int x , int y , int width  , int height)
         :base(b,x,y,width,height)
        {

        }
        public void Update(InputManager im)
        {
            if (this.ToRec.Contains(im.MousePoint))
            {
                Hovering = true;
            }
            else
                Hovering = false;

            if (this.ToRec.Contains(im.MousePoint) && im.Clicked)
            {
                Pressed = true;
                Clicked = false;
            }
           
        }

        public new void Draw(SpriteBatch sb )
        {
            sb.Draw(this);
            if(Hovering)
            sb.DrawRectangle(this.ToRec, Color.Black);
        }
    }
}
