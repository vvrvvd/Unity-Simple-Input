using System.Collections;
using System.Collections.Generic;

namespace SimpleInput.InputSystem.Editor
{
	public class KeywordSet : IEnumerable<KeyValuePair<string, string>>
	{
		public Dictionary<string, string> KeywordsDict
		{
			get; private set;
		}

		public KeywordSet()
		{
			KeywordsDict = new Dictionary<string, string>();
		}

		public void Add(string keyword, string value)
		{
			if (KeywordsDict.ContainsKey(keyword))
			{
				KeywordsDict[keyword] = value;
			}
			else
			{
				KeywordsDict.Add(keyword, value);
			}
		}

		public void Clear() => KeywordsDict.Clear();
		public void Remove(string keyword) => KeywordsDict.Remove(keyword);
		public bool Contains(string keyword) => KeywordsDict.ContainsKey(keyword);

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return KeywordsDict.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
