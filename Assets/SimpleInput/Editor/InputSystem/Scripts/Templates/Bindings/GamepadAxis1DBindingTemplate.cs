using SimpleInput.Utils;
using System;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class GamepadAxis1DBindingTemplate
	{
		[SearchableEnum]
		public GamepadInput negative;
		[SearchableEnum]
		public GamepadInput positive;

		public override string ToString()
		{
			return negative.ToEnumDisplayString() + " | " + positive.ToEnumDisplayString();
		}
	}
}
