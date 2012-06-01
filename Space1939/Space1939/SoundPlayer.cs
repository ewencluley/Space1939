using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Space1939
{
    public class SoundPlayer : AudioEngine
    {
        WaveBank waveBank;
        SoundBank soundBank;
        AudioCategory musicCategory;
        Game1 game;
        Cue currentSound;

        public SoundPlayer(Game1 game, String settingsFile)
            : base(settingsFile + ".xgs")
        {
            this.game = game;
            waveBank = new WaveBank(this, settingsFile + ".xwb");
            soundBank = new SoundBank(this, settingsFile + ".xsb");
            musicCategory = base.GetCategory("Sound");
            initialize();
        }

        public void playSound(String soundName)
        {
            currentSound = soundBank.GetCue("" + soundName);
            currentSound.Play();
        }

        public void setVolume(float vol)
        {
            musicCategory.SetVolume(MathHelper.Clamp((float)(vol / 50), 0.0f, 2.0f));
        }

        public void initialize()
        {
            float volume = game.getOptions().getOptionsData().soundVolume;
            setVolume(volume);
        }

        public void UpdateSound()
        {
            base.Update();
        }

        public Cue getCue(String name)
        {
            return soundBank.GetCue(name);
        }

        internal void playCue(Cue cue)
        {
            cue.Play();
        }
    }
}

