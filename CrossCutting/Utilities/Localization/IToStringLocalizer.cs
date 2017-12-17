using System;

namespace Indigo.CrossCutting.Utilities.Localization
{
	/// <summary>Interface of localizer.</summary>
	/// <typeparam name="T">Type of localized value.</typeparam>
	public interface IToStringLocalizer<T>
	{
		/// <summary>Localizes the specified value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>Localized string representation of given value.</returns>
		string Localize(T value);
	}
}
