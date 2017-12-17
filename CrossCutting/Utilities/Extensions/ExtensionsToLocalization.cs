using Indigo.CrossCutting.Utilities.Localization;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	/// <summary>
	/// Extension class providing localization methods.
	/// </summary>
	public static class ExtensionsToLocalization
    {
		/// <summary>Converts object to its localized string representation. Note, it will look for registered 
		/// <see cref="IToStringLocalizer{T}"/> in <see cref="AppServices"/></summary>
		/// <typeparam name="T">Type of object.</typeparam>
		/// <param name="subject">The subject.</param>
		/// <returns>Localized string representation (or just .ToString() if no localizer has been registered).</returns>
		public static string ToLocalizedString<T>(this T subject)
		{
			var localizer = AppServices.Resolve<IToStringLocalizer<T>>(null);
			if (localizer == null) localizer = ToStringLocalizer<T>.Instance;
			return localizer.Localize(subject);
		}
	}
}
