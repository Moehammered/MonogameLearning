using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using MonogameLearning.Utilities;
using MonogameLearning.GameComponents;

namespace FiniteStateMachine.GameComponents
{
    class PowerUpPill : Component
    {
        private Vector3 startPos, endPos;
        private float timer = 0, speed = 1;
        private int direction = 1;

        public PowerUpPill()
        {
        }

        public override void Initialize()
        {
            startPos = owner.transform.Position;
            endPos = startPos + Vector3.Up;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.enabled)
            {
                if (direction >= 1)
                {
                    timer += Time.DeltaTime;
                    if (timer > 1)
                        direction = -1;
                }
                else
                {
                    timer -= Time.DeltaTime;
                    if (timer < 0)
                        direction = 1;
                }
                owner.transform.Position = Vector3.Lerp(startPos, endPos, timer);
            }
            else
                Owner.Destroy();
        }

        public void OnCollision(GameObject col)
        {
            if(col.name == "Player")
            {
                PlayerController player = col.GetComponent<PlayerController>();
                player.isPoweredUp = true;
                this.enabled = false;
            }
        }
    }
}
