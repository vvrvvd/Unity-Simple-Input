using SimpleInput.Utils;
using System;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class MouseAxis1DBindingTemplate
	{
		[SearchableEnum]
		public MouseInput negative;
		[SearchableEnum]
		public MouseInput positive;

		public override string ToString()
		{
			return negative.ToEnumDisplayString() + " | " + positive.ToEnumDisplayString();
		}
	}
}
