using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInput.InputSystem.Editor
{
	[Serializable]
    public class KeywordLineTemplate
    {
        [Preset(nameof(editorDropdownKeywordsList))]
        public string keyword;
        public KeywordLineSetType keywordSetType;
        [ShowIf(nameof(keywordSetType), KeywordLineSetType.Actions)]
        public ActionSetType actionSetType;
        [TextArea]
        public string lineTemplate;

        [SerializeField, HideInInspector]
        internal List<string> editorDropdownKeywordsList;
    }
}
