using SimpleInput.Utils;
using System;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class KeyboardAxis1DBindingTemplate
	{
		[SearchableEnum]
		public KeyInput negative;
		[SearchableEnum]
		public KeyInput positive;

		public override string ToString()
		{
			return negative.ToEnumDisplayString() + " | " + positive.ToEnumDisplayString();
		}
	}
}
