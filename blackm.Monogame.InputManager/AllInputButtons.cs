using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackm.Monogame.InputManager
{
    public struct AllInputButtons
    {
        internal Dictionary<Buttons, Keys> ButtonKeyMappings;

        internal void Initialize()
        {
            ButtonKeyMappings = new Dictionary<Buttons, Keys>()
            {
                {Buttons.A, Keys.K },
                {Buttons.B, Keys.L },
                {Buttons.X, Keys.J },
                {Buttons.Y, Keys.I },

                //Menu(Back) and Start
                {Buttons.Start, Keys.F2 },
                {Buttons.Back, Keys.F1 },

                //DPad
                {Buttons.DPadUp, Keys.NumPad8 },
                {Buttons.DPadDown, Keys.NumPad2 },
                {Buttons.DPadLeft, Keys.NumPad4 },
                {Buttons.DPadRight, Keys.NumPad6 },

                //Shoulders
                {Buttons.LeftShoulder,Keys.LeftControl },
                {Buttons.RightShoulder, Keys.RightControl },

                //Stickes
                {Buttons.LeftStick, Keys.Z },
                {Buttons.RightStick, Keys.X }
            };

        }

        private InputState GetState(Buttons button)
        {

            if (InputManager.GP1CurrentState.IsButtonDown(button) && InputManager.GP1PreviousState.IsButtonUp(button))
                return InputState.Pressed;
            if (InputManager.GP1PreviousState.IsButtonDown(button) && InputManager.GP1CurrentState.IsButtonUp(button))
                return InputState.Released;
            if (InputManager.GP1CurrentState.IsButtonDown(button))
                return InputState.Down;

            return InputState.Up;
        }

        private InputState GetState(Keys key)
        {
            if (InputManager.KBCurrentState.IsKeyDown(key) && InputManager.KBPreviousState.IsKeyUp(key))
                return InputState.Pressed;
            if (InputManager.KBPreviousState.IsKeyDown(key) && InputManager.KBCurrentState.IsKeyUp(key))
                return InputState.Released;
            if (InputManager.KBCurrentState.IsKeyDown(key))
                return InputState.Down;

            return InputState.Up;
        }

        private InputState GetState(Buttons button, Keys? key)
        {

            if (key == null)
                return GetState(button);

            if (InputManager.KBCurrentState.IsKeyDown(key.Value) && InputManager.KBPreviousState.IsKeyUp(key.Value) || InputManager.GP1CurrentState.IsButtonDown(button) && InputManager.GP1PreviousState.IsButtonUp(button))
                return InputState.Pressed;
            if (InputManager.KBPreviousState.IsKeyDown(key.Value) && InputManager.KBCurrentState.IsKeyUp(key.Value) || InputManager.GP1PreviousState.IsButtonDown(button) && InputManager.GP1CurrentState.IsButtonUp(button))
                return InputState.Released;
            if (InputManager.KBCurrentState.IsKeyDown(key.Value) || InputManager.GP1CurrentState.IsButtonDown(button))
                return InputState.Down;

            return InputState.Up;
        }

        internal InputState GetKeyState(Keys? key)
        {
            if (key != null)
                return GetState(key.Value);
            return InputState.Up;
        }

        /// <summary>
        /// Gets the Keyboard Key that is mapped to the GamePad Button
        /// </summary>
        /// <param name="button">Button to Find Key Mapping For</param>
        /// <returns>Key mapped to button, null if does not exist</returns>
        private Keys? GetKeyButtonMap(Buttons button)
        {
            Keys key;
            if (ButtonKeyMappings.TryGetValue(button, out key))
                return key;
            return null;
        }

        /// <summary>
        /// Checks any button in the list matches the given InputState
        /// </summary>
        /// <param name="buttonState">InputState to match</param>
        /// <param name="buttons">Button array to check</param>
        /// <returns>True if any button matchesd, false if no button matches InputState</returns>
        public bool AnyButton(InputState buttonState, params Buttons[] buttons)
        {
            foreach (var button in buttons)
            {
                if (GetState(button, GetKeyButtonMap(button)) == buttonState)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks that all buttons match the given state, kind of broken since buttons could be pressed or down and this would return false, and this is probably not desired functionality
        /// </summary>
        /// <param name="buttonState"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public bool AllButtons(InputState buttonState, params Buttons[] buttons)
        {
            foreach (var button in buttons)
            {
                if (GetState(button, GetKeyButtonMap(button)) != buttonState)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Checks that all buttons are down or pressed (at least one must be pressed)
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public bool AllButtonsPressed(params Buttons[] buttons)
        {
            bool somePressed = false;

            foreach (var button in buttons)
            {
                switch (GetState(button, GetKeyButtonMap(button)))
                {
                    case InputState.Up:
                    case InputState.Released:
                        return false;
                    case InputState.Pressed:
                        somePressed = true;
                        break;
                }
            }

            if (somePressed)
                return true;

            return false;
        }

    }
}
