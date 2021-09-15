using SimpleInput.Utils;
using System;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class MouseAxis2DBindingTemplate
	{
		[SearchableEnum]
		public MouseInput up;
		[SearchableEnum]
		public MouseInput down;
		[SearchableEnum]
		public MouseInput left;
		[SearchableEnum]
		public MouseInput right;

		public override string ToString()
		{
			return up.ToEnumDisplayString() + " | " + down.ToEnumDisplayString() + " | " + left.ToEnumDisplayString() + " | " + right.ToEnumDisplayString();
		}
	}
}
