using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MazeEscape.GameComponents
{
    class FirstPersonSounds : Component
    {
        private SoundEffect moveSoundData, winSoundData, deadSoundData;
        private SoundEffectInstance moveSoundInst, winSoundInst, deadSoundInst;

        public FirstPersonSounds()
        {
        }

        public void stopMoveSound()
        {
            if (moveSoundInst != null)
                moveSoundInst.Stop();
        }

        public void playDeadSound(float volume = 1, float pitch = 1)
        {
            if(deadSoundInst != null)
            {
                deadSoundInst.Volume = volume;
                deadSoundInst.Pitch = pitch;
                deadSoundInst.Play();
            }
        }

        public void playMoveSound(float volume = 1, float pitch = 1)
        {
            if(moveSoundInst != null)
            {
                moveSoundInst.Volume = volume;
                moveSoundInst.Pitch = pitch;
                if (moveSoundInst.State == SoundState.Stopped)
                    moveSoundInst.Play();
            }
        }

        public void playWinSound(float volume = 1, float pitch = 1)
        {
            if (winSoundInst != null)
            {
                winSoundInst.Volume = volume;
                winSoundInst.Pitch = pitch;
                winSoundInst.Stop();
                winSoundInst.Play();
            }
        }

        public void stopAllSounds()
        {
            if (moveSoundInst != null)
                moveSoundInst.Stop();
            if (winSoundInst != null)
                winSoundInst.Stop();
            if (deadSoundInst != null)
                deadSoundInst.Stop();
        }

        public override void Destroy()
        {
            moveSoundInst = null;
            winSoundInst = null;
            deadSoundInst = null;
        }

        public override void Initialize()
        {
            loadSounds();
            setSoundInstances();
        }

        public override void Update(GameTime gameTime)
        {
        }

        #region Sound Initialisation
        private void loadSounds()
        {
            moveSoundData = GameInstance.Content.Load<SoundEffect>("Sounds/footsteps");
            deadSoundData = GameInstance.Content.Load<SoundEffect>("Sounds/dead");
            winSoundData = GameInstance.Content.Load<SoundEffect>("Sounds/win");
        }

        private void setSoundInstances()
        {
            moveSoundInst = moveSoundData.CreateInstance();
            deadSoundInst = deadSoundData.CreateInstance();
            winSoundInst = winSoundData.CreateInstance();
        }
        #endregion
    }
}
