using SimpleInput.Utils;
using System;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class GamepadAxis2DBindingTemplate
	{
		[SearchableEnum]
		public GamepadInput up;
		[SearchableEnum]
		public GamepadInput down;
		[SearchableEnum]
		public GamepadInput left;
		[SearchableEnum]
		public GamepadInput right;

		public override string ToString()
		{
			return up.ToEnumDisplayString() + " | " + down.ToEnumDisplayString() + " | " + left.ToEnumDisplayString() + " | " + right.ToEnumDisplayString();
		}
	}
}
