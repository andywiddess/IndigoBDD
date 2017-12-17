namespace Indigo.CrossCutting.Utilities.Error
{
	/// <summary>
	/// The severity of the error
	/// </summary>
	public enum Severity
	{
		/// <summary>Level:0 - The lowest possible severity, use to be decided</summary>
		None,
		/// <summary>Level:1 - Used for logging information via the error mechanism</summary>
		Info,
		/// <summary>Level:2 - Used for warning that an application warning has occured, but it's not a failure, such as import file was getting too long</summary>
		Warning,
		/// <summary>Level:3 - Used for indicating the application error (NOT exception), such as import file was the wrong type</summary>
		Error,
		/// <summary>Level:4 - Used for recording errors which can be deemed as terminal. These could be used to suggest the process has failed and cannot be continued</summary>
		Fatal,
	}
}
