namespace Indigo.CrossCutting.Utilities.Localization
{
	#region class ToStringLocalizer<T>

	/// <summary>Default <see cref="IToStringLocalizer{T}"/></summary>
	/// <typeparam name="T">Type of object.</typeparam>
	public class ToStringLocalizer<T>
        : IToStringLocalizer<T>
	{
		#region static fields

		/// <summary>Default instance.</summary>
		public static readonly ToStringLocalizer<T> Instance = new ToStringLocalizer<T>();

		#endregion

		#region IToStringLocalizer<T> Members

		/// <summary>Localizes the specified value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>Localized string representation of given value.</returns>
		public string Localize(T value)
		{
			if (object.ReferenceEquals(value, null)) return "<null>";

			try
			{
				return value.ToString();
			}
			catch
			{
				return "<exception>";
			}
		}

		#endregion
	}

	#endregion
}
