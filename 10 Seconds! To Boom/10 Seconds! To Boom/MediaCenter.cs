using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

namespace _10_Seconds__To_Boom
{
    class MediaCenter
    {
        public List<string> Sounds = new List<string>();
        public SoundPlayer MusicPlayer;
        public SoundPlayer SoundHandler;
        public Dictionary<string,int> MusicList = new Dictionary<string,int>();
        public bool MusicPlaying;
        public int cmNumb;
        private long LastCheck;
        public void PlayMusic(string music , int seconds)
        {
            /* if music is running cancel and play new music
             string name then time in seconds*/
            MusicList.Add(music, seconds);
            MusicPlaying = true;
        }
        public void PlaySound(string sound)
        {
            //play a sound
            SoundHandler = new SoundPlayer(sound);
            SoundHandler.Play();
        }
        public void PushSound(string sound)
        {
            //cancel current sounds to play this sound.
            SoundHandler.Stop();
            SoundHandler = new SoundPlayer(sound);
            SoundHandler.Play();
        }

        public void Update(InputManager im)
        { 
            if (LastCheck == 0)
            {
                LastCheck = im.GameTime.ElapsedMilliseconds;
            }
            else if (MusicPlaying && im.GameTime.ElapsedMilliseconds - LastCheck / 1000 > MusicList.ToArray()[cmNumb].Value)
            {
                LastCheck = im.GameTime.ElapsedMilliseconds;
                MusicPlayer.Stop();
                cmNumb++;
                try
                {
                    MusicPlayer = new SoundPlayer(MusicList.ToArray()[cmNumb].Key);
                }
                catch { cmNumb = 0; MusicPlayer = new SoundPlayer(MusicList.ToArray()[cmNumb].Key); }
            }
        }
    }
}
