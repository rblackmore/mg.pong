using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackm.Monogame.InputManager
{
    public struct AllInputTriggers
    {

        internal Dictionary<Buttons, Keys> TriggerKeyMappings;
        private const float _KEYTRIGGERVALUE = 1.0f;

        internal void Initialize()
        {
            TriggerKeyMappings = new Dictionary<Buttons, Keys>()
            {
                { Buttons.LeftTrigger, Keys.LeftAlt },
                { Buttons.RightTrigger, Keys.RightAlt }
            };
        }

        public float Left {
            get
            {
                switch (InputManager.Buttons.GetKeyState(TriggerKeyMappings[Buttons.LeftTrigger]))
                {
                    case InputState.Pressed:
                    case InputState.Down:
                        return _KEYTRIGGERVALUE;
                }

                if (InputManager.GP1CurrentState.IsConnected)
                    return InputManager.GP1CurrentState.Triggers.Left;

                return 0f;
            }
        }

        public float Right
        {
            get
            {
                switch (InputManager.Buttons.GetKeyState(TriggerKeyMappings[Buttons.RightTrigger]))
                {
                    case InputState.Pressed:
                    case InputState.Down:
                        return _KEYTRIGGERVALUE;
                }

                if (InputManager.GP1CurrentState.IsConnected)
                    return InputManager.GP1CurrentState.Triggers.Right;

                return 0f;
            }
        }

    }
}
