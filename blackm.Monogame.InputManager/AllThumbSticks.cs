using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackm.Monogame.InputManager
{
    public struct AllThumbSticks
    {
        internal Dictionary<Buttons, Keys> ThumbStickKeyMappings;
        private const float _KEYTHUMBSTICKVALUE = 1.0f;

        internal void Initialize()
        {
            ThumbStickKeyMappings = new Dictionary<Buttons, Keys>()
            {
                //Left ThumbStick
                {Buttons.LeftThumbstickUp,Keys.W },
                {Buttons.LeftThumbstickDown,Keys.S },
                {Buttons.LeftThumbstickLeft,Keys.A },
                {Buttons.LeftThumbstickRight,Keys.D },

                //Right ThumbStick
                {Buttons.RightThumbstickUp,Keys.Up },
                {Buttons.RightThumbstickDown,Keys.Down },
                {Buttons.RightThumbstickLeft,Keys.Left },
                {Buttons.RightThumbstickRight,Keys.Right }
            };
        }

        private Keys? GetKeyButtonMap(Buttons button)
        {
            Keys key;
            if (ThumbStickKeyMappings.TryGetValue(button, out key))
                return key;
            return null;
        }

        private Vector2 GetState(Vector2 thumbStickValue, Thumbstick stick)
        {
            Vector2 ret = Vector2.Zero;


            if (InputManager.GP1CurrentState.IsConnected)
                ret = thumbStickValue;

            Buttons up = (stick == Thumbstick.Left) ? Buttons.LeftThumbstickUp : Buttons.RightThumbstickUp;
            Buttons down = (stick == Thumbstick.Left) ? Buttons.LeftThumbstickDown : Buttons.RightThumbstickDown;
            Buttons left = (stick == Thumbstick.Left) ? Buttons.LeftThumbstickLeft : Buttons.RightThumbstickLeft;
            Buttons right = (stick == Thumbstick.Left) ? Buttons.LeftThumbstickLeft : Buttons.RightThumbstickLeft;

            switch (InputManager.Buttons.GetKeyState(GetKeyButtonMap(up)))
            {
                case InputState.Down:
                case InputState.Pressed:
                    ret.Y = _KEYTHUMBSTICKVALUE;
                    break;
            }

            switch (InputManager.Buttons.GetKeyState(GetKeyButtonMap(down)))
            {
                case InputState.Down:
                case InputState.Pressed:
                    ret.Y = -_KEYTHUMBSTICKVALUE;
                    break;
            }

            switch (InputManager.Buttons.GetKeyState(GetKeyButtonMap(left)))
            {
                case InputState.Down:
                case InputState.Pressed:
                    ret.X = -_KEYTHUMBSTICKVALUE;
                    break;
            }

            switch (InputManager.Buttons.GetKeyState(GetKeyButtonMap(right)))
            {
                case InputState.Down:
                case InputState.Pressed:
                    ret.X = _KEYTHUMBSTICKVALUE;
                    break;
            }


            return ret;
        }

        public Vector2 Left { get { return GetState(InputManager.GP1CurrentState.ThumbSticks.Left, Thumbstick.Left); } }
        public Vector2 Right { get { return GetState(InputManager.GP1CurrentState.ThumbSticks.Right, Thumbstick.Right); } }
    }
}
