using System.Collections.Generic;
using UnityEngine;

namespace SimpleInput
{
    public class SimpleInputActionSet
    {
        protected List<SimpleInputAction> inputActionsList;
        protected Dictionary<string, SimpleInputAction> inputActionsDict;

        public SimpleInputAction this[string actionName]
        {
            get => GetInputAction(actionName);
            set => AddInputAction(value);
        }

        public SimpleInputActionSet()
        {
            inputActionsList = new List<SimpleInputAction>();
            inputActionsDict = new Dictionary<string, SimpleInputAction>();
        }

        public void UpdateInputBindHandlers()
        {
            for (var i = 0; i < inputActionsList.Count; i++)
            {
                inputActionsList[i].UpdateInputBindHandlers();
            }
        }

        public void AddInputAction(SimpleInputAction inputAction)
        {
            if (inputActionsDict.ContainsKey(inputAction.Name)) {
                Debug.LogWarning($"[InputActionSet] You cannot register {inputAction.Name} action twice!");
                return;
            }

            inputActionsList.Add(inputAction);
            inputActionsDict.Add(inputAction.Name, inputAction);
        }

        public void RemoveInputAction(SimpleInputAction inputAction)
        {
            if (inputActionsDict.ContainsKey(inputAction.Name))
            {
                Debug.LogWarning($"[InputActionSet] You cannot unregister {inputAction.Name} action as it doesn't exists!");
                return;
            }

            inputActionsList.Remove(inputAction);
            inputActionsDict.Remove(inputAction.Name);
        }

        public SimpleInputAction GetInputAction(string actionName)
        {
            if (inputActionsDict.TryGetValue(actionName, out var inputAction))
            {
                return inputAction;
            }

            Debug.LogWarning($"[InputActionSet] You cannot get {actionName} action as it doesn't exists!");
            return null;
        }
    }
}