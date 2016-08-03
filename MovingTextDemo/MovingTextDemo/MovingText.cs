using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MovingTextDemo
{
    class MovingText
    {
        private float movementSpeed;
        private Keys up, down, left, right;
        private Text2D displayText;
        private SpriteBatch spriteBatch;

        //default constructor
        public MovingText()
        {
            movementSpeed = 100f;
            up = Keys.W;
            down = Keys.S;
            left = Keys.A;
            right = Keys.D;
            displayText = new Text2D();
        }

        public void setSpriteBatchReference(ref SpriteBatch batch)
        {
            spriteBatch = batch;
        }

        public void setMovementKeys(Keys up, Keys down, Keys left, Keys right)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }

        public Vector2 Position
        {
            get
            {
                return displayText.Position;
            }
            set
            {
                displayText.Position = value;
            }
        }

        public string Text
        {
            get
            {
                return displayText.Text;
            }
            set
            {
                displayText.Text = value;
            }
        }

        public SpriteFont Font
        {
            get
            {
                return displayText.Font;
            }
            set
            {
                displayText.Font = value;
            }
        }

        public Color Colour
        {
            get
            {
                return displayText.Colour;
            }
            set
            {
                displayText.Colour = value;
            }
        }

        public float Speed
        {
            get
            {
                return movementSpeed;
            }
            set
            {
                movementSpeed = value;
            }
        }

        public void update(float deltaTime)
        {
            checkInput(deltaTime);
        }

        private void checkInput(float deltaTime)
        {
            if (Keyboard.GetState().IsKeyDown(right))
            {
                displayText.translate(movementSpeed * deltaTime, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(left))
            {
                displayText.translate(-movementSpeed * deltaTime, 0);
            }
            if (Keyboard.GetState().IsKeyDown(up))
            {
                displayText.translate(0, -movementSpeed * deltaTime);
            }
            else if (Keyboard.GetState().IsKeyDown(down))
            {
                displayText.translate(0, movementSpeed * deltaTime);
            }
        }

        //needs to be called between begin and end of spriteBatch calls
        public void draw()
        {
            spriteBatch.DrawString(displayText.Font, displayText.Text, displayText.Position, displayText.Colour);
        }
    }
}
