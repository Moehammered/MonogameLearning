using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;

namespace MazeEscape.GameComponents
{
    class PlayerTracker : Component
    {
        public bool hitGoal = false, dead = false;
        public int points;

        public PlayerTracker()
        {
        }

        public override void Destroy()
        {
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void OnCollision(GameObject other)
        {
            if (other.name == "goal")
            {
                hitGoal = true;
            }
            else if (other.name == "enemy")
            {
                /*dead = true;
                if (deathSound != null)
                    deathSound.Play();
                moveSound.Stop();*/
                GameObject.DestroyTest(other);
            }
            else if (other.name == "pickup")
            {
                points++;
                //other.RemoveComponent<MeshRendererComponent>(); //remove rendering of pickup
                other.transform.Translate(0, -1000, 0); //move it out of the way to avoid unnecessary repeated collisions
                GameObject.DestroyTest(other);
            }
        }
    }
}
