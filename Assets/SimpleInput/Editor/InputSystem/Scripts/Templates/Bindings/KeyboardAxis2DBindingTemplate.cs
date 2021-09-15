using SimpleInput.Utils;
using System;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class KeyboardAxis2DBindingTemplate
	{
		[SearchableEnum]
		public KeyInput up;
		[SearchableEnum]
		public KeyInput down;
		[SearchableEnum]
		public KeyInput left;
		[SearchableEnum]
		public KeyInput right;

		public override string ToString()
		{
			return up.ToEnumDisplayString() + " | " + down.ToEnumDisplayString() + " | " + left.ToEnumDisplayString() + " | " + right.ToEnumDisplayString();
		}
	}
}
