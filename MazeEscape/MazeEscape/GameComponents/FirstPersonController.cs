using System;
using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.Utilities;
using Microsoft.Xna.Framework.Audio;

namespace MazeEscape.GameComponents
{
    class FirstPersonController : Component
    {
        public bool hitGoal = false, dead = false;
        public int points;
        private Keys[] movementKeys;
        private Vector3 movementDirection;
        private FirstPersonMover fpMover;
        private FirstPersonSounds sounds;
        private const int FORWARD_KEY = 0, BACK_KEY = 1, 
            LEFT_KEY = 2, RIGHT_KEY = 3, MAX_KEYS = 4;
        private float stunTimer = 1;

        public FirstPersonController()
        {
            movementKeys = new Keys[MAX_KEYS];
            movementDirection = Vector3.Zero;
            ForwardKey = Keys.W;
            BackwardKey = Keys.S;
            LeftKey = Keys.A;
            RightKey = Keys.D;
            points = 0;
        }

        public Keys ForwardKey
        {
            get { return movementKeys[FORWARD_KEY]; }
            set { movementKeys[FORWARD_KEY] = value; }
        }

        public Keys BackwardKey
        {
            get { return movementKeys[BACK_KEY]; }
            set { movementKeys[BACK_KEY] = value; }
        }

        public Keys LeftKey
        {
            get { return movementKeys[LEFT_KEY]; }
            set { movementKeys[LEFT_KEY] = value; }
        }

        public Keys RightKey
        {
            get { return movementKeys[RIGHT_KEY]; }
            set { movementKeys[RIGHT_KEY] = value; }
        }

        public override void Initialize()
        {
            //make sure we have a first person mover on our gameobject
            Console.WriteLine("Initialising PlayerController!!!!");
            fpMover = owner.GetComponent<FirstPersonMover>();
            if (fpMover == null)
                fpMover = owner.AddComponent<FirstPersonMover>();
            fpMover.lookSensitivity = 20;
            fpMover.speed = 3;
            //get our component in charge of managing our sounds
            sounds = owner.GetComponent<FirstPersonSounds>();
            if (sounds == null)
                sounds = owner.AddComponent<FirstPersonSounds>();
        }

        public override void Update(GameTime gameTime)
        {
            if (!dead)
            {
                checkCameraMovement();
                if (stunTimer < 0)
                {
                    checkMovementKeys();
                    fpMover.move(movementDirection);
                }
                else
                {
                    stunTimer -= Time.DeltaTime;
                }
            }
        }

        private void checkMovementKeys()
        {
            movementDirection = Vector3.Zero;

            if (Input.IsKeyHeld(ForwardKey))
            {
                movementDirection += owner.transform.Forward;
            }
            else if(Input.IsKeyHeld(BackwardKey))
            {
                movementDirection -= owner.transform.Forward;
            }
            if(Input.IsKeyHeld(LeftKey))
            {
                movementDirection -= owner.transform.Right;
            }
            else if(Input.IsKeyHeld(RightKey))
            {
                movementDirection += owner.transform.Right;
            }
            if (movementDirection.LengthSquared() > 0)
            {
                movementDirection.Normalize();
                sounds.playMoveSound();
            }
            else
                sounds.stopMoveSound();
        }

        private void checkCameraMovement()
        {
            fpMover.look(-Input.HorizontalDelta, -Input.VerticalDelta, 0);
        }

        private void OnCollision()
        {
            Console.WriteLine("Player COllided");
            hitGoal = true;
            fpMover.move(-movementDirection);
            stunTimer = 0.5f;
        }

        public void OnCollision(GameObject other)
        {
            Console.WriteLine("Player Collided with: " + other.name);
            if (other.name == "goal")
            {
                hitGoal = true;
                sounds.stopMoveSound();
                sounds.playWinSound();
            }
            else if (other.name == "enemy")
            {
                /*dead = true;
                if (deathSound != null)
                    deathSound.Play();
                moveSound.Stop();*/
                GameObject.DestroyTest(other);
            }
            else if(other.name == "pickup")
            {
                points++;
                sounds.playWinSound();
                other.RemoveComponent<MeshRendererComponent>(); //remove rendering of pickup
                other.transform.Translate(0, -1000, 0); //move it out of the way to avoid unnecessary repeated collisions
            }
            else
            {
                fpMover.move(-movementDirection);
                stunTimer = 0.5f;
            }
        }

        private void OnCollisionExit()
        {
            Console.WriteLine("Player left");
            hitGoal = false;
        }

        public override void Destroy()
        {
        }
    }
}
