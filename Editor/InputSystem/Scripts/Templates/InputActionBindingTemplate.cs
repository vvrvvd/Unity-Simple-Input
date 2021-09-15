using SimpleInput.Utils;
using System;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class InputActionBindingTemplate
	{
		[Disable]
		public string name;
		public InputDeviceType device = InputDeviceType.Keyboard;
		public InputBindingType type = InputBindingType.Single;

		[SearchableEnum, ShowIf(nameof(IsMouseSingle), true)]
		public MouseInput mouseInput;
		[ShowIf(nameof(IsMouseAxis), true)]
		public MouseAxis1DBindingTemplate mouseAxis1D;
		[ShowIf(nameof(IsMouseVector2), true)]
		public MouseAxis2DBindingTemplate mouseAxis2D;

		[SearchableEnum, ShowIf(nameof(IsGamepadSingle), true)]
		public GamepadInput gamepadInput;
		[ShowIf(nameof(IsGamepadAxis), true)]
		public GamepadAxis1DBindingTemplate gamepadAxis1D;
		[ShowIf(nameof(IsGamepadVector2), true)]
		public GamepadAxis2DBindingTemplate gamepadAxis2D;

		[SearchableEnum, ShowIf(nameof(IsKeyboardSingle), true)]
		public KeyInput keyboardInput;
		[ShowIf(nameof(IsKeyboardAxis), true)]
		public KeyboardAxis1DBindingTemplate keyboardAxis1D;
		[ShowIf(nameof(IsKeyboardVector2), true)]
		public KeyboardAxis2DBindingTemplate keyboardAxis2D;

		public bool IsSingle(InputDeviceType device) => this.device == device && type == InputBindingType.Single;
		public bool IsAxis(InputDeviceType device) => this.device == device && type == InputBindingType.Composition1D;
		public bool IsVector2(InputDeviceType device) => this.device == device && type == InputBindingType.Composition2D;

		public bool IsMouseSingle() => IsSingle(InputDeviceType.Mouse);
		public bool IsMouseAxis() => IsAxis(InputDeviceType.Mouse);
		public bool IsMouseVector2() => IsVector2(InputDeviceType.Mouse);

		public bool IsGamepadSingle() => IsSingle(InputDeviceType.Gamepad);
		public bool IsGamepadAxis() => IsAxis(InputDeviceType.Gamepad);
		public bool IsGamepadVector2() => IsVector2(InputDeviceType.Gamepad);

		public bool IsKeyboardSingle() => IsSingle(InputDeviceType.Keyboard);
		public bool IsKeyboardAxis() => IsAxis(InputDeviceType.Keyboard);
		public bool IsKeyboardVector2() => IsVector2(InputDeviceType.Keyboard);

		public void UpdateName()
		{
			name = $"[{device}] ";

			if (IsMouseSingle())
			{
				name += mouseInput.ToEnumDisplayString();
			}
			else if (IsMouseAxis())
			{
				name += mouseAxis1D;
			}
			else if (IsMouseVector2())
			{
				name += mouseAxis2D;
			}
			else if (IsKeyboardSingle())
			{
				name += keyboardInput.ToEnumDisplayString();
			}
			else if (IsKeyboardAxis())
			{
				name += keyboardAxis1D;
			}
			else if (IsKeyboardVector2())
			{
				name += keyboardAxis2D;
			}
			else if (IsGamepadSingle())
			{
				name += gamepadInput.ToEnumDisplayString();
			}
			else if (IsGamepadAxis())
			{
				name += gamepadAxis1D;
			}
			else if (IsGamepadVector2())
			{
				name += gamepadAxis2D;
			}
		}
	}
}
