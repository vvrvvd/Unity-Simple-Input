
using UnityEngine.InputSystem;

namespace SimpleInput.InputSystem
{
    public class InputSystemSimpleManager : SimpleInputManager
    {
        protected override InputDeviceSetType CheckLastInputDevice() 
        {
            var keyboard = Keyboard.current;
            if(keyboard!=null && keyboard.anyKey.CheckStateIsAtDefaultIgnoringNoise()) 
            {
                return InputDeviceSetType.MouseAndKeyboard;
			}

            var mouse = Mouse.current;
            if(mouse != null) 
            {
                for (var i = 0; i < mouse.allControls.Count; i++) 
                {
                    var control = mouse.allControls[i];
                    if (!control.CheckStateIsAtDefaultIgnoringNoise()) 
                    {
                        return InputDeviceSetType.MouseAndKeyboard;
                    }
                }
            }

            var gamepad = Gamepad.current;
			if (gamepad != null) 
            {
                for(var i=0; i< gamepad.allControls.Count; i++) 
                {
                    var control = gamepad.allControls[i];
                    if(!control.CheckStateIsAtDefaultIgnoringNoise()) 
                    {
                        return InputDeviceSetType.Gamepad;
					}
				}
			}

            return currentDeviceType;
        }
    }
}
