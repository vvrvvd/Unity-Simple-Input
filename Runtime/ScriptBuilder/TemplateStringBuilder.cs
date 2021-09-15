using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleInput.InputSystem.Editor
{
	public class TemplateStringBuilder
	{
		public static string NewLineString = "\r\n";
		public static string[] NewLineSplitCharacters = new[] { "\r\n", "\r", "\n" };

		protected char keywordBracesChar;
		protected string keywordBracesString;
		protected string keywordFormat;
		protected string noTrimFormattedKeyword;
		protected string template;
		protected string[] templateLines;

		protected KeywordSet addedKeywordsSet;
		protected StringBuilder stringBuilder;

		protected List<string> cachedKeywords;
		protected Dictionary<string, HashSet<int>> cachedKeywordsLines;

		public List<string> TemplateKeywordsList => cachedKeywords;

		public TemplateStringBuilder(char keywordBracesChar = '#', string noTrimKeyword = "NOTRIM")
		{
			Initialize(keywordBracesChar, noTrimKeyword);
		}

		public TemplateStringBuilder(string template, char keywordBracesChar = '#', string noTrimKeyword = "NOTRIM")
		{
			Initialize(keywordBracesChar, noTrimKeyword);
			SetTemplate(template);
		}

		public virtual void SetTemplate(string customTemplate)
		{
			template = customTemplate;
			templateLines = template.Split(NewLineSplitCharacters, System.StringSplitOptions.None);
			ProcessTemplate(template);
			stringBuilder.Clear();
		}

		public virtual void SetKeyword(string keyword, string value)
		{
			addedKeywordsSet.Add(keyword, value);
		}

		public virtual void SetKeywords(KeywordSet keywordsSet)
		{
			foreach(var item in keywordsSet)
			{
				addedKeywordsSet.Add(item.Key, item.Value);
			}
		}

		public virtual void Remove(string keyword)
		{
			addedKeywordsSet.Remove(keyword);
		}

		public virtual void Reset()
		{
			if (template != null)
			{
				SetTemplate(template);
			}

			addedKeywordsSet.Clear();
		}

		public virtual void Clear()
		{
			template = string.Empty;
			templateLines = new string[0];
			stringBuilder.Clear();
			addedKeywordsSet.Clear();
			cachedKeywords.Clear();
			cachedKeywordsLines.Clear();
		}

		public override string ToString()
		{
			foreach(var item in addedKeywordsSet)
			{
				var keyword = item.Key;
				var value = item.Value;
				ReplaceKeyword(keyword, value);
			}

			stringBuilder.Clear();
			for (var i = 0; i < templateLines.Length; i++)
			{
				var line = templateLines[i];
				if (string.IsNullOrEmpty(line))
				{
					continue;
				}

				line = line.Replace(noTrimFormattedKeyword, string.Empty);
				stringBuilder.AppendLine(line);
			}

			return stringBuilder.ToString().TrimEnd('\r', '\n');
		}

		protected virtual void Initialize(char keywordBracesChar, string noTrimKeyword)
		{
			this.keywordBracesChar = keywordBracesChar;
			keywordBracesString = keywordBracesChar.ToString();
			keywordFormat = $"{keywordBracesChar}{{0}}{keywordBracesChar}";
			noTrimFormattedKeyword = $"{keywordBracesChar}{noTrimKeyword}{keywordBracesChar}";
			stringBuilder = new StringBuilder();
			addedKeywordsSet = new KeywordSet();
			cachedKeywords = new List<string>();
			cachedKeywordsLines = new Dictionary<string, HashSet<int>>();
		}

		protected virtual void ProcessTemplate(string template)
		{
			cachedKeywords.Clear();
			cachedKeywordsLines.Clear();

			string line;
			var lineCounter = 0;
			var stringReader = new StringReader(template);
			while ((line = stringReader.ReadLine()) != null)
			{
				CacheKeywordsInLine(line, lineCounter);
				lineCounter++;
			}
		}

		private void CacheKeywordsInLine(string line, int lineIndex)
		{
			var bracesStartIndex = -1;
			for (var i = 0; i < line.Length; i++)
			{
				if (!line[i].Equals(keywordBracesChar))
				{
					continue;
				}

				if (bracesStartIndex == -1)
				{
					bracesStartIndex = i;
				}
				else
				{
					var keyword = line.Substring(bracesStartIndex + 1, i - bracesStartIndex - 1);
					CacheKeyword(keyword, lineIndex);
					bracesStartIndex = -1;
				}
			}
		}

		private void CacheKeyword(string keyword, int lineIndex)
		{
			if (!cachedKeywordsLines.TryGetValue(keyword, out var linesIndices))
			{
				linesIndices = new HashSet<int>() { lineIndex };
				cachedKeywords.Add(keyword);
				cachedKeywordsLines.Add(keyword, linesIndices);
			}

			linesIndices.Add(lineIndex);
		}

		private void ReplaceKeyword(string keyword, string value)
		{
			if (!cachedKeywordsLines.TryGetValue(keyword, out var linesIndices))
			{
				return;
			}

			var isReplaceTextEmpty = string.IsNullOrEmpty(value);
			var formattedKeyword = string.Format(keywordFormat, keyword);
			foreach (var lineIndex in linesIndices)
			{
				var line = templateLines[lineIndex];

				var spacingCounter = 0;
				for (var j = 0; j < line.Length; j++)
				{
					var ch = line[j];
					if (char.IsWhiteSpace(ch) || ch == '\t')
					{
						spacingCounter++;
					}
					else
					{
						j = line.Length;
					}
				}

				var spacingSubstring = NewLineString + line.Substring(0, spacingCounter).ToString();
				value = value.Replace(NewLineString, spacingSubstring);

				templateLines[lineIndex] = templateLines[lineIndex].Replace(formattedKeyword, value);

				if (isReplaceTextEmpty)
				{
					templateLines[lineIndex] = templateLines[lineIndex].Trim();
				}
			}
		}
	}
}
