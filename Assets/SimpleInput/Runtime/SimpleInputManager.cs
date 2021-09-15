using System.Collections.Generic;
using UnityEngine;

namespace SimpleInput
{
    public abstract class SimpleInputManager : IInputManager
    {
        public event System.Action<InputDeviceSetType> OnInputDeviceChanged;

        protected List<SimpleInputAction> inputActionsList;
        protected Dictionary<string, SimpleInputAction> inputActionsDict;
        protected List<SimpleInputActionSet> inputActionSetsList;
        protected HashSet<SimpleInputActionSet> inputActionSetsHashset;

        protected InputDeviceSetType currentDeviceType = InputDeviceSetType.None;
		public InputDeviceSetType CurrentDeviceType 
        { 
            get => currentDeviceType; 
            set 
            { 
                if(currentDeviceType == value) 
                {
                    return;
				}

                currentDeviceType = value;
                OnInputDeviceChanged?.Invoke(currentDeviceType);
            }
        }

		public SimpleInputManager()
        {
            inputActionsList = new List<SimpleInputAction>();
            inputActionsDict = new Dictionary<string, SimpleInputAction>();
            inputActionSetsList = new List<SimpleInputActionSet>();
            inputActionSetsHashset = new HashSet<SimpleInputActionSet>();
        }

        public void Update()
        {
            for (var i = 0; i < inputActionSetsList.Count; i++)
            {
                inputActionSetsList[i].UpdateInputBindHandlers();
            }

            for (var i = 0; i < inputActionsList.Count; i++)
            {
                inputActionsList[i].UpdateInputBindHandlers();
            }

            CurrentDeviceType = CheckLastInputDevice();
        }

        public void RegisterInputActionSet(SimpleInputActionSet inputActionSet)
        {
            if (inputActionSetsHashset.Contains(inputActionSet))
            {
                Debug.LogWarning($"[InputManager] You cannot register {inputActionSet} action set twice!");
                return;
            }

            inputActionSetsHashset.Add(inputActionSet);
            inputActionSetsList.Add(inputActionSet);
        }

        public void UnregisterInputActionSet(SimpleInputActionSet inputActionSet)
        {
            if (inputActionSetsHashset.Contains(inputActionSet))
            {
                Debug.LogWarning($"[InputManager] You cannot unregister {inputActionSet} action set as it's not registered!");
                return;
            }

            inputActionSetsHashset.Remove(inputActionSet);
            inputActionSetsList.Remove(inputActionSet);
        }

        public void RegisterAction(SimpleInputAction inputAction)
        {
            if(inputActionsDict.ContainsKey(inputAction.Name))
            {
                Debug.LogWarning($"[InputManager] You cannot register {inputAction.Name} action twice!");
                return;
            }

            inputActionsDict.Add(inputAction.Name, inputAction);
            inputActionsList.Add(inputAction);
        }

        public void UnregisterAction(SimpleInputAction inputAction)
        {
            if(inputActionsDict.ContainsKey(inputAction.Name))
            {
                Debug.LogWarning($"[InputManager] You cannot unregister {inputAction.Name} as it's not registered!");
                return;
            }

            inputActionsDict.Remove(inputAction.Name);
            inputActionsList.Remove(inputAction);
        }

        protected abstract InputDeviceSetType CheckLastInputDevice();
    }
}
