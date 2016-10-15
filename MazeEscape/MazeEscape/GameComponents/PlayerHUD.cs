using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using MazeEscape.BaseComponents;

namespace MazeEscape.GameComponents
{
    class PlayerHUD : Component
    {
        private TextRenderer uiText;
        private PlayerTracker player;

        public PlayerHUD()
        {
        }

        public string Text
        {
            get { return uiText.text; }
        }

        public Color TextColour
        {
            get { return uiText.colour; }
            set { uiText.colour = value; }
        }

        public Vector2 TextPosition
        {
            get { return uiText.position; }
            set { uiText.position = value; }
        }

        public override void Initialize()
        {
            uiText = owner.GetComponent<TextRenderer>();
            if (uiText == null)
                uiText = owner.AddComponent<TextRenderer>();
            player = owner.GetComponent<PlayerTracker>();
        }

        public override void Update(GameTime gameTime)
        {
            updateHUD();
        }

        private void updateHUD()
        {
            if(player != null)
            {
                string message = "";
                if (player.hitGoal)
                    message = "You made it!";
                else if (player.dead)
                    message = "You died! :(";
                else
                {
                    message = "Find the green exit.\nAvoid the red enemies!";
                    message += "\n    Points: " + player.points;
                }
                uiText.text = message;
            }
        }

        public override void Destroy()
        {
        }
    }
}
