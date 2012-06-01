using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Space1939
{
    /// <summary>
    /// Used for playing music. Contains helper methods for playing tracks, 
    /// </summary>
    public class MusicPlayer:AudioEngine
    {
        WaveBank waveBank;
        SoundBank soundBank;
        int currentTrackNo=1;
        AudioCategory musicCategory;
        Game1 game;
        Cue currentTrack;
        int totalTracks;

        public MusicPlayer(Game1 game, String settingsFile, int totalTracks)
            : base(settingsFile+ ".xgs")
        {
            this.game = game;
            this.totalTracks = totalTracks;
            waveBank = new WaveBank(this, settingsFile+ ".xwb");
            soundBank = new SoundBank(this, settingsFile+ ".xsb");
            musicCategory = base.GetCategory("Music");

            initialize();
        }

        public void playTrack(int trackNo)
        {
           
            currentTrack = soundBank.GetCue("" + trackNo);
            currentTrackNo = trackNo;
            currentTrack.Play();
        }

        public void playRandomTrack()
        {
            Random rand = new Random();
            playTrack(rand.Next(totalTracks) + 1);
        }

        public void setVolume(float vol)
        {
            musicCategory.SetVolume(MathHelper.Clamp((float)(vol/50), 0.0f, 2.0f));
        }

        public void initialize()
        {
            float volume = game.getOptions().getOptionsData().musicVolume;
            setVolume(volume);
        }

        public void UpdateMusic()
        {
            if (currentTrack.IsStopped)
            {
                skipTrack();
            }
            base.Update();
        }

        public void skipTrack()
        {
            if (totalTracks > 1)
            {
                currentTrack.Stop(AudioStopOptions.Immediate);
                if (currentTrackNo < totalTracks)
                {
                    currentTrackNo++;
                }
                else
                {
                    currentTrackNo = 1;
                }
                playTrack(currentTrackNo);
            }
        }

        public void stop()
        {
            currentTrack.Stop(AudioStopOptions.Immediate);
        }

        public void addTrack()
        {
            if (totalTracks < 7)//the max number of cues that exist in the game.
            {
                if (totalTracks == 0)
                {
                    playRandomTrack();
                }
                totalTracks++;
            }
        }
    }
}
