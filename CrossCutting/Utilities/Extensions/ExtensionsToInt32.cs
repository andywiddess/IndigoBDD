using System;
using System.Globalization;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToInt32
	{
		public static int FromHexToInt32(string text)
		{
			int value;
			if (int.TryParse(text, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out value))
				return value;

			throw new ArgumentException("'{0}' is not a valid hexidecimal value".FormatWith(text));
		}
	}
}