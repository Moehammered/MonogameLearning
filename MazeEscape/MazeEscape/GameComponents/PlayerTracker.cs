using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;

namespace MazeEscape.GameComponents
{
    class PlayerTracker : Component
    {
        public bool hitGoal = false, dead = false, isPoweredUp = true;
        public int points;

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
        }

        public void OnCollision(GameObject other)
        {
            if (other.name == "goal")
            {
                hitGoal = true;
                sounds.stopMoveSound();
                sounds.playWinSound();
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
                    owner.RemoveComponent<FirstPersonController>();
                    dead = true;
                }
            }
            else if (other.name == "pickup")
            {
                addScore();
                other.transform.Translate(0, -1000, 0); //move it out of the way to avoid unnecessary repeated collisions
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
