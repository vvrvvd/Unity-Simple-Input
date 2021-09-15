using SimpleInput.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[CreateAssetMenu(fileName = "InputActionSet", menuName = "Simple Input/Input System/Input Action Set")]
	public class InputActionSetScriptGenerator : ScriptableObject
	{
		private const char KeywordBracesChar = '#';

		private const string KeywordActionName = "ACTION_NAME";
		private const string KeywordActionVariableName = "ACTION_VARIABLE_NAME";

		private const string KeywordDeviceSingleButtonFormat = "{0}_BUTTON";
		private const string KeywordDeviceAxisSingleButtonFormat = "{0}_AXIS";
		private const string KeywordDeviceAxisButton1Format = "{0}_BUTTON_1A";
		private const string KeywordDeviceAxisButton2Format = "{0}_BUTTON_2A";
		private const string KeywordDeviceVector2SingleButtonFormat = "{0}_VECTOR2";
		private const string KeywordDeviceVector2Button1Format = "{0}_BUTTON_1B";
		private const string KeywordDeviceVector2Button2Format = "{0}_BUTTON_2B";
		private const string KeywordDeviceVector2Button3Format = "{0}_BUTTON_3B";
		private const string KeywordDeviceVector2Button4Format = "{0}_BUTTON_4B";

		private const string MouseDeviceName = "MOUSE";
		private const string GamepadDeviceName = "GAMEPAD";
		private const string KeyboardDeviceName = "KEYBOARD";
		private const string DefaultScriptTemplateName = "DefaultScriptTemplate";

		public InputActionSetGeneratorTemplate scriptGeneratorTemplate;
		public string scriptName = "ExampleInputActionSet";
		public string relativeNamespace = "Example.Namespace";
		public float gamepadStickDeadzone = 0.2f;
		[EditorButton(nameof(Generate), "Generate script"), SerializeField]
		public InputActionSetTemplate actionSetTemplate;

		private void Awake()
		{
			if (actionSetTemplate == null)
			{
				actionSetTemplate = new InputActionSetTemplate();
			}

			scriptGeneratorTemplate = Resources.Load<InputActionSetGeneratorTemplate>(DefaultScriptTemplateName);
		}

		private void OnValidate()
		{
			foreach (var action in actionSetTemplate.actions)
			{
				action.ValidateBindings();
			}
		}

		public void Generate()
		{
			var allActionsData = new List<KeywordSet>();
			var buttonActionsData = new List<KeywordSet>();
			var floatActionsData = new List<KeywordSet>();
			var vector2ActionsData = new List<KeywordSet>();
			var bindingsData = new List<KeywordSet>();
			foreach (var action in actionSetTemplate.actions)
			{
				action.ValidateBindings();
				var bindingsDict = CreateActionDataBindingsKeywordsList(action);
				bindingsData.AddRange(bindingsDict);

				var keywordsDict = CreateActionDataKeywordDict(action);
				switch (action.actionType)
				{
					case InputActionType.Button:
						buttonActionsData.Add(keywordsDict);
						break;
					case InputActionType.Value:
						if (action.valueType == InputValueType.@float)
						{
							floatActionsData.Add(keywordsDict);
						}
						else if (action.valueType == InputValueType.Vector2)
						{
							vector2ActionsData.Add(keywordsDict);
						}
						break;
					default:
						Debug.LogError($"[InputActionSetScriptable] There is a missing action type: {action.actionType}", this);
						throw new NotImplementedException();
				}

				allActionsData.Add(keywordsDict);
			}

			var scriptContentTemplateString = scriptGeneratorTemplate.scriptContentTemplate.ToString();
			var scriptBuilder = new TemplateScriptBuilder();
			scriptBuilder.SetTemplate(scriptContentTemplateString);

			for (var i = 0; i < scriptGeneratorTemplate.keywordLineTemplates.Count; i++)
			{
				var keywordLineTemplate = scriptGeneratorTemplate.keywordLineTemplates[i];
				List<KeywordSet> keywordSetList;
				if (keywordLineTemplate.keywordSetType == KeywordLineSetType.Bindings)
				{
					keywordSetList = bindingsData;
				}
				else
				{
					switch (keywordLineTemplate.actionSetType)
					{
						case ActionSetType.Button:
							keywordSetList = buttonActionsData;
							break;
						case ActionSetType.Float:
							keywordSetList = floatActionsData;
							break;
						case ActionSetType.Vector2:
							keywordSetList = vector2ActionsData;
							break;
						case ActionSetType.All:
						default:
							keywordSetList = allActionsData;
							break;
					}
				}
				scriptBuilder.AddGeneratedLines(keywordLineTemplate.keyword, keywordLineTemplate.lineTemplate, keywordSetList);
			}

			var scriptablePath = AssetDatabase.GetAssetPath(this);
			var folderPath = scriptablePath.Substring(0, scriptablePath.Length - Path.GetFileName(scriptablePath).Length);
			scriptBuilder.Generate(scriptName, relativeNamespace, folderPath);
		}

		private List<KeywordSet> CreateActionDataBindingsKeywordsList(InputActionTemplate action)
		{
			var bindingsKeywordsList = new List<KeywordSet>();
			for (var i = 0; i < action.bindings.Count; i++)
			{
				var binding = action.bindings[i];
				switch (binding.device)
				{
					case InputDeviceType.Mouse:
						var mouseBindingKeywords = AddBindingKeywords(action, binding, MouseDeviceName, InputDeviceType.Mouse);
						bindingsKeywordsList.Add(mouseBindingKeywords);
						break;
					case InputDeviceType.Gamepad:
						var gamepadBindingKeywords = AddBindingKeywords(action, binding, GamepadDeviceName, InputDeviceType.Gamepad);
						bindingsKeywordsList.Add(gamepadBindingKeywords);
						break;
					case InputDeviceType.Keyboard:
						var keyboardBindingKeywords = AddBindingKeywords(action, binding, KeyboardDeviceName, InputDeviceType.Keyboard);
						bindingsKeywordsList.Add(keyboardBindingKeywords);
						break;
					default:
						Debug.LogError($"[InputActionSetScriptable] There is a missing bindings device type: {binding.device} in action {action.name}");
						throw new NotImplementedException();
				}
			}

			return bindingsKeywordsList;
		}

		private KeywordSet AddBindingKeywords(InputActionTemplate actionTemplate, InputActionBindingTemplate binding, string deviceName, InputDeviceType deviceType)
		{
			var keywordsDict = CreateActionDataKeywordDict(actionTemplate);
			if (binding.IsSingle(deviceType))
			{
				keywordsDict.Add(string.Format(KeywordDeviceSingleButtonFormat, deviceName), GetSingleButtonString(binding, deviceType));
			}
			else if (binding.IsAxis(deviceType))
			{
				if (binding.type == InputBindingType.Single)
				{
					keywordsDict.Add(string.Format(KeywordDeviceAxisSingleButtonFormat, deviceName), GetSingleButtonString(binding, deviceType));
				}
				else if (binding.type == InputBindingType.Composition1D)
				{
					keywordsDict.Add(string.Format(KeywordDeviceAxisButton1Format, deviceName), GetAxisPositiveButtonString(binding, deviceType));
					keywordsDict.Add(string.Format(KeywordDeviceAxisButton2Format, deviceName), GetAxisNegativeButtonString(binding, deviceType));
				}
			}
			else if (binding.IsVector2(deviceType))
			{
				if (binding.type == InputBindingType.Single)
				{
					keywordsDict.Add(string.Format(KeywordDeviceVector2SingleButtonFormat, deviceName), GetSingleButtonString(binding, deviceType));
				}
				else if (binding.type == InputBindingType.Composition2D)
				{
					keywordsDict.Add(string.Format(KeywordDeviceVector2Button1Format, deviceName), GetVector2UpButtonString(binding, deviceType));
					keywordsDict.Add(string.Format(KeywordDeviceVector2Button2Format, deviceName), GetVector2DownButtonString(binding, deviceType));
					keywordsDict.Add(string.Format(KeywordDeviceVector2Button3Format, deviceName), GetVector2LeftButtonString(binding, deviceType));
					keywordsDict.Add(string.Format(KeywordDeviceVector2Button4Format, deviceName), GetVector2RightButtonString(binding, deviceType));
				}
			}

			return keywordsDict;
		}

		private string GetSingleButtonString(InputActionBindingTemplate binding, InputDeviceType deviceType)
		{
			switch (deviceType)
			{
				case InputDeviceType.Mouse:
					return $".{binding.mouseInput.ToString().FirstCharToLowerCase()}";
				case InputDeviceType.Gamepad:
					if (binding.gamepadInput == GamepadInput.RightStick || binding.gamepadInput == GamepadInput.LeftStick) {
						var gamepadInputString = binding.gamepadInput.ToString();
						return $".{gamepadInputString.FirstCharToLowerCase()}, {gamepadStickDeadzone.ToString("r", CultureInfo.GetCultureInfo("en-us"))}f";
					}

					return $"[GamepadButton.{binding.gamepadInput}]";
				case InputDeviceType.Keyboard:
					return $"[Key.{binding.keyboardInput}]";
			}

			return null;
		}

		private string GetAxisPositiveButtonString(InputActionBindingTemplate binding, InputDeviceType deviceType)
		{
			switch (deviceType)
			{
				case InputDeviceType.Mouse:
					return $".{binding.mouseAxis1D.positive.ToString().FirstCharToLowerCase()}";
				case InputDeviceType.Gamepad:
					if (binding.gamepadAxis1D.positive == GamepadInput.RightStick || binding.gamepadAxis1D.positive == GamepadInput.LeftStick) {
						return $".{binding.gamepadAxis1D.positive}";
					}

					return $"[GamepadButton.{binding.gamepadAxis1D.positive}]";
				case InputDeviceType.Keyboard:
					return $"[Key.{binding.gamepadAxis1D.positive}]";
			}

			return null;
		}

		private string GetAxisNegativeButtonString(InputActionBindingTemplate binding, InputDeviceType deviceType)
		{
			switch (deviceType)
			{
				case InputDeviceType.Mouse:
					return $".{binding.mouseAxis1D.negative.ToString().FirstCharToLowerCase()}";
				case InputDeviceType.Gamepad:
					if (binding.gamepadAxis1D.negative == GamepadInput.RightStick || binding.gamepadAxis1D.negative == GamepadInput.LeftStick) {
						return $".{binding.gamepadAxis1D.negative}";
					}

					return $"[GamepadButton.{binding.gamepadAxis1D.negative}]";
				case InputDeviceType.Keyboard:
					return $"[Key.{binding.gamepadAxis1D.negative}]";
			}

			return null;
		}

		private string GetVector2UpButtonString(InputActionBindingTemplate binding, InputDeviceType deviceType)
		{
			switch (deviceType)
			{
				case InputDeviceType.Mouse:
					return $".{binding.mouseAxis2D.up.ToString().FirstCharToLowerCase()}";
				case InputDeviceType.Gamepad:
					if(binding.gamepadAxis2D.up == GamepadInput.RightStick  || binding.gamepadAxis2D.up == GamepadInput.LeftStick) 
					{
						return $".{binding.gamepadAxis2D.up}";
					}

					return $"[GamepadButton.{binding.gamepadAxis2D.up}]";
				case InputDeviceType.Keyboard:
					return $"[Key.{binding.keyboardAxis2D.up}]";
			}

			return null;
		}

		private string GetVector2DownButtonString(InputActionBindingTemplate binding, InputDeviceType deviceType)
		{
			switch (deviceType)
			{
				case InputDeviceType.Mouse:
					return $".{binding.mouseAxis2D.down.ToString().FirstCharToLowerCase()}";
				case InputDeviceType.Gamepad:
					if (binding.gamepadAxis2D.down == GamepadInput.RightStick || binding.gamepadAxis2D.down == GamepadInput.LeftStick) {
						return $".{binding.gamepadAxis2D.down}";
					}

					return $"[GamepadButton.{binding.gamepadAxis2D.down}]";
				case InputDeviceType.Keyboard:
					return $"[Key.{binding.keyboardAxis2D.down}]";
			}

			return null;
		}

		private string GetVector2LeftButtonString(InputActionBindingTemplate binding, InputDeviceType deviceType)
		{
			switch (deviceType)
			{
				case InputDeviceType.Mouse:
					return $".{binding.mouseAxis2D.left.ToString().FirstCharToLowerCase()}";
				case InputDeviceType.Gamepad:
					if (binding.gamepadAxis2D.left == GamepadInput.RightStick || binding.gamepadAxis2D.left == GamepadInput.LeftStick) {
						return $".{binding.gamepadAxis2D.left}";
					}

					return $"[GamepadButton.{binding.gamepadAxis2D.left}]";
				case InputDeviceType.Keyboard:
					return $"[Key.{binding.keyboardAxis2D.left}]";
			}

			return null;
		}

		private string GetVector2RightButtonString(InputActionBindingTemplate binding, InputDeviceType deviceType)
		{
			switch (deviceType)
			{
				case InputDeviceType.Mouse:
					return $".{binding.mouseAxis2D.right.ToString().FirstCharToLowerCase()}";
				case InputDeviceType.Gamepad:
					if (binding.gamepadAxis2D.right == GamepadInput.RightStick || binding.gamepadAxis2D.right == GamepadInput.LeftStick) {
						return $".{binding.gamepadAxis2D.right}";
					}

					return $"[GamepadButton.{binding.gamepadAxis2D.right}]";
				case InputDeviceType.Keyboard:
					return $"[Key.{binding.keyboardAxis2D.right}]";
			}
			return null;
		}

		private KeywordSet CreateActionDataKeywordDict(InputActionTemplate action)
		{
			var keywordsDict = new KeywordSet();
			var actionName = action.name;
			var variableName = Regex.Replace(actionName, @"[^a-z^A-Z]+", String.Empty);
			keywordsDict.Add(KeywordActionName, actionName);
			keywordsDict.Add(KeywordActionVariableName, variableName);

			return keywordsDict;
		}
	}
}
