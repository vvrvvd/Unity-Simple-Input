using System;

namespace SimpleInput
{
    public interface IInputManager
    {
        event Action<InputDeviceSetType> OnInputDeviceChanged;
        
        InputDeviceSetType CurrentDeviceType { get; }

        void RegisterInputActionSet(SimpleInputActionSet inputActionSet);
        void UnregisterInputActionSet(SimpleInputActionSet inputActionSet);
        void RegisterAction(SimpleInputAction inputAction);
        void UnregisterAction(SimpleInputAction inputAction);
    }

}