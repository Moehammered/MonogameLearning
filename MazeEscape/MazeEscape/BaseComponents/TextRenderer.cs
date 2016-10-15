using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeEscape.BaseComponents
{
    class TextRenderer : RenderComponent
    {
        public Color colour;
        public string text;
        public Vector2 position;

        private SpriteFont font;
        private SpriteBatch batch;
        
        public TextRenderer()
        {
            colour = Color.White;
            position = Vector2.Zero;
            text = "";
        }

        public override void Initialize()
        {
            batch = new SpriteBatch(GameInstance.GraphicsDevice);
            font = GameInstance.Content.Load<SpriteFont>("Arial");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Destroy()
        {
        }

        public override void Draw(GameTime gameTime)
        {
            batch.Begin();
            batch.DrawString(font, text, position, colour);
            batch.End();
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }
    }
}
