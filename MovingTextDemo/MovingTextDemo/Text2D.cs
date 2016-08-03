using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MovingTextDemo
{
    class Text2D
    {
        //displaying text variables
        private Vector2 textPosition;
        private SpriteFont font;
        private string displayText;
        //colour display variables
        private Color textColour;

        //Default constructor
        public Text2D()
        {
            textPosition = new Vector2(0, 0);
            textColour = Color.White;
            displayText = "Hello";
        }

        public Text2D(string message)
        {
            displayText = message;
            textPosition = new Vector2(0, 0);
            textColour = Color.White;
        }

        public void translate(float x, float y)
        {
            textPosition.X += x;
            textPosition.Y += y;
        }

        public Vector2 Position
        {
            get
            {
                return textPosition;
            }
            set
            {
                textPosition = value;
            }
        }

        public Color Colour
        {
            get
            {
                return textColour;
            }
            set
            {
                textColour = value;
            }
        }

        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }

        public string Text
        {
            get
            {
                return displayText;
            }
            set
            {
                displayText = value;
            }
        }

    }
}
