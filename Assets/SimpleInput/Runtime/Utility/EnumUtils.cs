using System;

namespace SimpleInput.Utils
{
	public static class EnumUtils
	{
		public static string ToEnumDisplayString<T>(this T input) where T : Enum
		{
			var inputString = input.ToString();

			for (var i = 1; i < inputString.Length; i++)
			{
				if (char.IsLetter(inputString[i]) && char.IsLower(inputString[i]))
				{
					continue;
				}

				if (i == 0 || !char.IsWhiteSpace(inputString[i-1]))
				{
					inputString = inputString.Insert(i, " ");
				}
			}

			inputString = inputString.FirstCharToUpperCase();

			return inputString;
		}
	}
}
