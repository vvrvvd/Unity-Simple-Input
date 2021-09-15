using System.Collections.Generic;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[CreateAssetMenu(fileName = "ActionSetScriptTemplate", menuName = "Simple Input/Input System/Generator/Script Template")]
    public class InputActionSetGeneratorTemplate : ScriptableObject
    {
        public TextAsset scriptContentTemplate;
		[LabelByChild("keyword")]
        public List<KeywordLineTemplate> keywordLineTemplates;

		private TemplateStringBuilder templateStringBuilder;

		private void OnEnable()
		{
			AttachEditorDropdownKeywordsList();
		}

		private void OnValidate()
		{
			AttachEditorDropdownKeywordsList();
		}

		private void AttachEditorDropdownKeywordsList()
		{
			if (scriptContentTemplate == null)
			{
				return;
			}

			if (templateStringBuilder == null)
			{
				templateStringBuilder = new TemplateStringBuilder();
			}

			templateStringBuilder.Clear();
			templateStringBuilder.SetTemplate(scriptContentTemplate.text);

			var keywordsList = templateStringBuilder.TemplateKeywordsList;

			if (keywordsList == null)
			{
				return;
			}

			for (var i = 0; i < keywordLineTemplates.Count; i++)
			{
				var lineTemplate = keywordLineTemplates[i];
				lineTemplate.editorDropdownKeywordsList = keywordsList;
			}
		}
	}
}
