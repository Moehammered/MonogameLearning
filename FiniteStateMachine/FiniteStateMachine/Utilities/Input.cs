using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameLearning.Utilities
{
    public enum MouseButton
    {
        LEFT,
        RIGHT,
        MIDDLE
    }

    static class Input
    {
        private static KeyboardState prevKeyboardState;
        private static MouseState prevMouseState;

        #region Mouse Properties
        public static Vector2 MousePosition
        {
            get { return Mouse.GetState().Position.ToVector2(); }
        }

        public static Vector2 MouseDelta
        {
            get { return Mouse.GetState().Position.ToVector2() - prevMouseState.Position.ToVector2(); }
        }

        public static int MouseScrollDelta
        {
            get { return Mouse.GetState().ScrollWheelValue - prevMouseState.ScrollWheelValue; }
        }

        public static int HorizontalDelta
        {
            get { return Mouse.GetState().X - prevMouseState.X; }
        }

        public static int VerticalDelta
        {
            get { return Mouse.GetState().Y - prevMouseState.Y; }
        }

        public static bool AnyMouseButton
        {
            get
            {
                //bitwise OR all the buttons together to check for state
                ButtonState mouseButtons = Mouse.GetState().LeftButton | 
                    Mouse.GetState().MiddleButton | Mouse.GetState().RightButton;

                return mouseButtons == ButtonState.Pressed;
            }
        }
        #endregion

        #region Mouse Input Functions
        public static bool IsMousePressed(MouseButton btn)
        {
            MouseState currState = Mouse.GetState(); //just to shorten to return lines
            switch(btn)
            {
                case MouseButton.LEFT:
                    return (currState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released);

                case MouseButton.RIGHT:
                    return (currState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released);

                case MouseButton.MIDDLE:
                    return (currState.MiddleButton == ButtonState.Pressed && prevMouseState.MiddleButton == ButtonState.Released);

                default:
                    return false;
            }
        }

        public static bool IsMouseReleased(MouseButton btn)
        {
            MouseState currState = Mouse.GetState();
            switch(btn)
            {
                case MouseButton.LEFT:
                    return (currState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed);

                case MouseButton.RIGHT:
                    return (currState.RightButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Pressed);

                case MouseButton.MIDDLE:
                    return (currState.MiddleButton == ButtonState.Released && prevMouseState.MiddleButton == ButtonState.Pressed);

                default:
                    return false;
            }
        }

        public static bool IsMouseHeld(MouseButton btn)
        {
            switch(btn)
            {
                case MouseButton.LEFT:
                    return Mouse.GetState().LeftButton == ButtonState.Pressed;

                case MouseButton.RIGHT:
                    return Mouse.GetState().RightButton == ButtonState.Pressed;

                case MouseButton.MIDDLE:
                    return Mouse.GetState().MiddleButton == ButtonState.Pressed;

                default:
                    return false;
            }
        }
        #endregion

        #region Keyboard Properties
        public static bool AnyKey
        {
            get { return KeysHeld.Length > 0; }
        }

        public static Keys[] KeysHeld
        {
            get { return Keyboard.GetState().GetPressedKeys(); }
        }
        #endregion

        #region Keyboard Input Functions
        public static bool IsKeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && prevKeyboardState.IsKeyUp(key));
        }

        public static bool IsKeyReleased(Keys key)
        {
            return (Keyboard.GetState().IsKeyUp(key) && prevKeyboardState.IsKeyDown(key));
        }

        public static bool IsKeyHeld(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
        #endregion

        //required to keep track of input state
        public static void recordInputs()
        {
            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }
    }
}
