using System.Collections.Generic;

namespace SimpleInput.InputSystem
{
	using UnityEngine.InputSystem;

	public abstract class InputSystemSimpleActionSet : SimpleInputActionSet
	{
		private HashSet<int> bindedDevicesIds;

		public InputSystemSimpleActionSet() : base()
		{
			bindedDevicesIds = new HashSet<int>();
			InputSystem.onDeviceChange += OnDeviceChanged;

			Initialize();

			foreach (var device in InputSystem.devices)
			{
				OnDeviceAdded(device);
			}
		}

		protected virtual void Initialize()
		{
			CreateActions();
			AddActions();
		}

		protected abstract void CreateInputActions();
		protected abstract void CreateInputAxisActions();
		protected abstract void CreateInputVector2Actions();

		protected abstract void AddInputActions();
		protected abstract void AddInputAxisActions();
		protected abstract void AddInputVector2Actions();

		protected abstract void OnMouseConnected(Mouse mouse);
		protected abstract void OnGamepadConnected(Gamepad gamepad);
		protected abstract void OnKeyboardConnected(Keyboard keyboard);

		protected abstract void OnMouseDisconnected(Mouse mouse);
		protected abstract void OnGamepadDisconnected(Gamepad gamepad);
		protected abstract void OnKeyboardDisconnected(Keyboard keyboard);

		private void CreateActions()
		{
			CreateInputActions();
			CreateInputAxisActions();
			CreateInputVector2Actions();
		}

		private void AddActions()
		{
			AddInputActions();
			AddInputAxisActions();
			AddInputVector2Actions();
		}

		private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
		{
			switch (change)
			{
				case InputDeviceChange.Added:
					OnDeviceAdded(device);
					break;
				case InputDeviceChange.Removed:
					OnDeviceRemoved(device);
					break;
				default:
					return;
			}
		}

		private void OnDeviceAdded(InputDevice device)
		{
			if (bindedDevicesIds.Contains(device.deviceId))
			{
				return;
			}

			if (device is Mouse mouse)
			{
				OnMouseConnected(mouse);
			}
			else if (device is Gamepad gamepad)
			{
				OnGamepadConnected(gamepad);
			}
			else if (device is Keyboard keyboard)
			{
				OnKeyboardConnected(keyboard);
			}

			bindedDevicesIds.Add(device.deviceId);
		}

		private void OnDeviceRemoved(InputDevice device)
		{
			if (!bindedDevicesIds.Contains(device.deviceId))
			{
				return;
			}

			if (device is Mouse mouse)
			{
				OnMouseDisconnected(mouse);
			}
			else if (device is Gamepad gamepad)
			{
				OnGamepadDisconnected(gamepad);
			}
			else if (device is Keyboard keyboard)
			{
				OnKeyboardDisconnected(keyboard);
			}

			bindedDevicesIds.Remove(device.deviceId);
		}
	}
}