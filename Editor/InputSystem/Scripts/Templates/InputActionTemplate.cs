using SimpleInput.InputSystem.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
	public class InputActionTemplate
	{
		public string name;
		public InputActionType actionType;

		[ShowIf(nameof(actionType), InputActionType.Value)]
		public InputValueType valueType;

		[ReorderableList]
		public List<InputActionBindingTemplate> bindings;

		public InputActionTemplate()
		{
			name = "Action";
			bindings = new List<InputActionBindingTemplate>();
			actionType = InputActionType.Button;
		}

		public void ValidateBindings()
		{
			foreach (var binding in bindings)
			{
				if (actionType == InputActionType.Button)
				{
					if (binding.type != InputBindingType.Single)
					{
						binding.type = InputBindingType.Single;
					}
				}

				if (actionType == InputActionType.Value)
				{
					if (valueType == InputValueType.@float && binding.type == InputBindingType.Composition2D)
					{
						binding.type = InputBindingType.Composition1D;
					}
				}

				binding.UpdateName();
			}
		}
	}
}
