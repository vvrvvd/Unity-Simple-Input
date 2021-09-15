using SimpleInput;
using SimpleInput.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
namespace Test.Test2
{
	public class TestInputActionSet : InputSystemSimpleActionSet
	{
        public SimpleInputAction Interact { get; private set;}
        public SimpleInputAction<Vector2> Move { get; private set;}
        public SimpleInputAction<Vector2> MouseLookAxis { get; private set;}
        public SimpleInputAction<Vector2> GamepadLookAxis { get; private set;}
        
        protected override void CreateInputActions()
        {
            Interact = new SimpleInputAction("Interact");
        }
        
        protected override void CreateInputAxisActions()
        {
        }
        
        protected override void CreateInputVector2Actions()
        {
            Move = new SimpleInputAction<Vector2>("Move");
            MouseLookAxis = new SimpleInputAction<Vector2>("MouseLookAxis");
            GamepadLookAxis = new SimpleInputAction<Vector2>("GamepadLookAxis");
        }
        
        protected override void AddInputActions()
        {
            AddInputAction(Interact);
        }
        
        protected override void AddInputAxisActions()
        {
        }
        
        protected override void AddInputVector2Actions()
        {
            AddInputAction(Move);
            AddInputAction(MouseLookAxis);
            AddInputAction(GamepadLookAxis);
        }
        
        protected override void OnMouseConnected(Mouse mouse)
        {
            MouseLookAxis.RegisterInputListener(mouse.delta);
            Interact.RegisterInputListener(mouse.leftButton);
        }
        
        protected override void OnKeyboardConnected(Keyboard keyboard)
        {
            Interact.RegisterInputListener(keyboard[Key.Space]);
            Move.RegisterInputListener(keyboard[Key.W], keyboard[Key.S], keyboard[Key.A], keyboard[Key.D]);
        }
        
        protected override void OnGamepadConnected(Gamepad gamepad)
        {
            Move.RegisterInputListener(gamepad.leftStick, 0.5f);
            GamepadLookAxis.RegisterInputListener(gamepad.rightStick, 0.5f);
            Interact.RegisterInputListener(gamepad[GamepadButton.A]);
        }
        
        protected override void OnMouseDisconnected(Mouse mouse)
        {
        }
        
        protected override void OnGamepadDisconnected(Gamepad gamepad)
        {
        }
        
        protected override void OnKeyboardDisconnected(Keyboard keyboard)
        {
        }
    }
}