using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using MonogameLearning.Utilities;

namespace MazeEscape.GameComponents
{
    class PlayerTracker : Component
    {
        public bool hitGoal = false, dead = false, isPoweredUp = false;
        public int points;
        private float closeGameTime = -1;

        private FirstPersonSounds sounds;

        public PlayerTracker()
        {
        }

        public override void Destroy()
        {
        }

        public override void Initialize()
        {
            sounds = owner.GetComponent<FirstPersonSounds>();
        }

        public override void Update(GameTime gameTime)
        {
            if (sounds == null) //make sure there is a reference
                sounds = owner.GetComponent<FirstPersonSounds>();
            if(closeGameTime > 0)
            {
                closeGameTime -= Time.DeltaTime;
                if (closeGameTime < 0)
                    GameInstance.Exit();
            }
        }

        public void OnCollision(GameObject other)
        {
            if (other.name == "goal")
            {
                hitGoal = true;
                sounds.stopMoveSound();
                sounds.playWinSound();
                //owner.RemoveComponent<FirstPersonController>();
                FirstPersonController control = owner.GetComponent<FirstPersonController>();
                if (control != null)
                    control.Enabled = false;
                closeGameTime = 1;
            }
            else if (other.name == "enemy")
            {
                if(isPoweredUp)
                {
                    addScore(5);
                    GameObject.DestroyTest(other);
                }
                else
                {
                    //owner.RemoveComponent<FirstPersonController>();
                    FirstPersonController control = owner.GetComponent<FirstPersonController>();
                    if (control != null)
                        control.Enabled = false;
                    sounds.stopAllSounds();
                    dead = true;
                }
            }
            else if (other.name == "pickup")
            {
                addScore();
                other.transform.Translate(0, -1000, 0); //move it out of the way to avoid unnecessary repeated collisions
                GameObject.DestroyTest(other);
            }
            else if(other.name == "power")
            {
                isPoweredUp = true;
                GameObject.DestroyTest(other);
            }
        }

        private void addScore(int amount = 1)
        {
            points += amount;
            sounds.playWinSound();
        }
    }
}
