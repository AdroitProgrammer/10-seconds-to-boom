using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _10_Seconds__To_Boom
{
    class Animation
    {
        public long LastAni;
        public Bitmap[] Frames;
        public Bitmap CurrentFrame;
        public Bitmap Sheet;
        public int CurrentFrameNumber;
        public int MaxNumbOfFrames;
        public int FPS;
        public bool Playing;
        
        public Animation(Bitmap sheet , int numberofframes  , int fps)
        {
            Frames = new Bitmap[numberofframes];
            int imagewidth = sheet.Width / numberofframes;
            for (int i = 0; i < numberofframes; i++)
            {
                Frames[i] = sheet.Clone(new Rectangle(i * imagewidth, 0, imagewidth, sheet.Height), sheet.PixelFormat);
            }
            MaxNumbOfFrames = numberofframes;
            CurrentFrame = Frames[0];
            Sheet = sheet;
            FPS = fps;
        }
        
        public Animation(Bitmap[] frames ,int fps)
        {
            Frames = frames;
            FPS = fps;
            MaxNumbOfFrames = frames.Length - 1;
            CurrentFrame = Frames[0];
        }

        public void Play()
        {
            if(!Playing)
            Playing = true;
        }

        public void Stop()
        {
            if (Playing)
                Playing = false;
        }

        public void Update(InputManager Im)
        {
            if (Playing)
            {
                if (Im.GameTime.ElapsedMilliseconds - LastAni > 1000 / FPS)
                {
                    LastAni = Im.GameTime.ElapsedMilliseconds;
                    if (CurrentFrameNumber < MaxNumbOfFrames)
                        CurrentFrameNumber++;
                    else
                        CurrentFrameNumber = 0;
                    CurrentFrame = Frames[CurrentFrameNumber ];
                }
            }
        }

        public static Bitmap SheetFlip(Bitmap sheet)
        {
            Bitmap b = new Bitmap(sheet.Clone(new Rectangle(Point.Empty,new Size(sheet.Width,sheet.Height)),sheet.PixelFormat));
            b.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return b;
        }

    }
}
