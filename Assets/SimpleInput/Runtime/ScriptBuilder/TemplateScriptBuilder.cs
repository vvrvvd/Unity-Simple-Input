using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SimpleInput.InputSystem.Editor
{
	public class TemplateScriptBuilder : TemplateStringBuilder
	{
		private const string ScriptNameKeyword = "SCRIPTNAME";
		private const string RelativeNamespaceKeyword = "RELATIVENAMESPACE";

		private TemplateStringBuilder lineTemplateBuilder;

		public void Generate(string scriptName, string relativeNamespace, string relativePath)
		{
			SetKeyword(ScriptNameKeyword, scriptName);
			SetKeyword(RelativeNamespaceKeyword, relativeNamespace);

			var generatedScript = ToString();
			File.Delete($"Assets/{scriptName}.cs");
			File.Delete($"Assets/{scriptName}.cs.meta");
			File.WriteAllText($"{relativePath}/{scriptName}.cs", generatedScript);

			#if UNITY_EDITOR
			AssetDatabase.Refresh();
			#endif
		}

		public void AddGeneratedLines<T>(string keyword, string lineTemplate, T values) where T : IEnumerable<KeywordSet>
		{
			stringBuilder.Clear();

			foreach(var keywords in values)
			{
				var generatedLine = GenerateLineFromTemplate(lineTemplate, keywords);
				if (!generatedLine.Contains(keywordBracesString))
				{
					stringBuilder.AppendLine(generatedLine);
				}
			}

			var linesValue = stringBuilder.ToString().TrimEnd('\r', '\n');
			
			SetKeyword(keyword, linesValue);
		}

		protected override void Initialize(char keywordBracesChar, string noTrimKeyword)
		{
			lineTemplateBuilder = new TemplateStringBuilder(keywordBracesChar, noTrimKeyword);
			base.Initialize(keywordBracesChar, noTrimKeyword);
		}

		private string GenerateLineFromTemplate(string lineTemplate, KeywordSet keywordsSet)
		{
			lineTemplateBuilder.Reset();
			lineTemplateBuilder.SetTemplate(lineTemplate);

			foreach (var keywordItem in keywordsSet)
			{
				lineTemplateBuilder.SetKeyword(keywordItem.Key, keywordItem.Value);
			}

			return lineTemplateBuilder.ToString();
		}
	}
}
