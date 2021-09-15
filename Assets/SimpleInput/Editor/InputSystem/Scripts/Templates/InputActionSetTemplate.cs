using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class InputActionSetTemplate
	{
		[ReorderableList(Foldable = false)]
		public List<InputActionTemplate> actions;

		public InputActionSetTemplate()
		{
			actions = new List<InputActionTemplate>();
		}
	}
}
