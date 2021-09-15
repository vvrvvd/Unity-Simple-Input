using System;
using System.Collections.Generic;

namespace SimpleInput
{
	public class SimpleInputAction
	{
		protected Action[] inputEvents;
		protected List<SimpleInputListener> inputList;

		public string Name
		{
			get; private set;
		}

		public SimpleInputAction(string actionName)
		{
			this.Name = actionName;
			inputList = new List<SimpleInputListener>();
			inputEvents = new Action[Enum.GetNames(typeof(InputEventType)).Length];
		}

		public void RegisterInputListener(SimpleInputListener handler)
		{
			inputList.Add(handler);
		}

		public void UnregisterInputListener(SimpleInputListener handler)
		{
			inputList.Remove(handler);
		}

		public void BindEvent(Action inputEvent, InputEventType inputType)
		{
			inputEvents[(int)inputType] += inputEvent;
		}

		public void UnbindEvent(Action inputEvent, InputEventType inputType)
		{
			inputEvents[(int)inputType] -= inputEvent;
		}
		public void ClearEvent(InputEventType inputType)
		{
			inputEvents[(int)inputType] = null;
		}

		public virtual void UpdateInputBindHandlers()
		{
			for (var i = 0; i < inputList.Count; i++)
			{
				var inputActionHandler = inputList[i];
				if (inputActionHandler.RefreshEventType())
					inputEvents[(int)inputActionHandler.ActiveEventType]?.Invoke();
			}
		}
	}

	public class SimpleInputAction<T> : SimpleInputAction where T : struct
	{
		protected Action<T> axisInputEvent;
		protected List<InputListener<T>> axisInputList;

		public SimpleInputAction(string actionName) : base(actionName)
		{
			axisInputList = new List<InputListener<T>>();
			inputEvents = new Action[Enum.GetNames(typeof(InputEventType)).Length];
		}

		public void RegisterInputListener(InputListener<T> handler)
		{
			axisInputList.Add(handler);
		}

		public void UnregisterInputListener(InputListener<T> handler)
		{
			axisInputList.Remove(handler);
		}

		public void BindEvent(Action<T> inputEvent)
		{
			axisInputEvent += inputEvent;
		}

		public void UnbindEvent(Action<T> inputEvent)
		{
			axisInputEvent -= inputEvent;
		}

		public override void UpdateInputBindHandlers()
		{
			base.UpdateInputBindHandlers();

			for (var i = 0; i < axisInputList.Count; i++)
			{
				var inputActionHandler = axisInputList[i];
				if (inputActionHandler.IsActive)
				{
					axisInputEvent?.Invoke(inputActionHandler.Value);
				}
			}
		}
	}
}