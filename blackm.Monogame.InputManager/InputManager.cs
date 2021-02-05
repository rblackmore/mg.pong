using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackm.Monogame.InputManager
{
    public static class InputManager
    {
        static public AllInputButtons Buttons = new AllInputButtons();
        static public AllInputTriggers Triggers = new AllInputTriggers();
        static public AllThumbSticks ThumbSticks = new AllThumbSticks();

        static private KeyboardState _kbCurrentState, _kbPreviousState;
        static private GamePadState _gp1CurrentState, _gp1PreviousState;

        static internal KeyboardState KBCurrentState { get { return _kbCurrentState; } }
        static internal KeyboardState KBPreviousState { get { return _kbPreviousState; } }
        static internal GamePadState GP1CurrentState { get { return _gp1CurrentState; } }
        static internal GamePadState GP1PreviousState { get { return _gp1PreviousState; } }

        static public void Initialize()
        {
            Buttons.Initialize();
            Triggers.Initialize();
            ThumbSticks.Initialize();
        }

        static public void Update()
        {
            _kbPreviousState = _kbCurrentState;
            _gp1PreviousState = _gp1CurrentState;

            _kbCurrentState = Keyboard.GetState();
            _gp1CurrentState = GamePad.GetState(PlayerIndex.One);
        }
    }
}
