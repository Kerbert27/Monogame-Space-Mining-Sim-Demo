using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine {
    public static class InputManager {
        static KeyboardState PreviousKeyboardState { get; set; }
        static KeyboardState CurrentKeyboardState { get; set; }
        static MouseState PreviousMouseState { get; set; }
        static MouseState CurrentMouseState { get; set; }
        public static void Initialize() {
            PreviousKeyboardState = CurrentKeyboardState =
            Keyboard.GetState();
            PreviousMouseState = CurrentMouseState =
            Mouse.GetState();
        }
        public static void Update() {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public static bool IsKeyDown(Keys key) {
            return CurrentKeyboardState.IsKeyDown(key);
        }
        public static bool IsKeyPressed(Keys key) {
            return CurrentKeyboardState.IsKeyDown(key) &&
            PreviousKeyboardState.IsKeyUp(key);
        }
        public static bool IsKeyUp(Keys key) {
            return CurrentKeyboardState.IsKeyUp(key);
        }
        public static bool IsKeyReleased(Keys key) {
            return CurrentKeyboardState.IsKeyUp(key) &&
                PreviousKeyboardState.IsKeyUp(key);
        }
        public static Vector2 GetMousePosition() {
            return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }
        public static bool IsMouseDown(int btn) {
            return (btn == 0) ? CurrentMouseState.LeftButton == ButtonState.Pressed : CurrentMouseState.RightButton == ButtonState.Pressed;
        }
        /// <summary>
        /// 0 == Left Mouse
        /// 1 == Right Mouse
        /// </summary>
        /// <returns></returns>
        public static bool IsMouseReleased(int btn) {
            return (btn == 0) ? CurrentMouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed : 
                         CurrentMouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed;
        }
    }
}
