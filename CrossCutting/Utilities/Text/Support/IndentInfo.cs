namespace Indigo.CrossCutting.Utilities.Text.Support
{
	/// <summary>Indentation info.</summary>
	public class IndentInfo
	{
		#region properties

		/// <summary>
		/// Gets or sets the indent mode.
		/// </summary>
		/// <value>The indent mode.</value>
		public IndentMode IndentMode { get; set; }

		/// <summary>
		/// Gets or sets the indent char.
		/// </summary>
		/// <value>The indent char.</value>
		public char IndentChar { get; set; }

		/// <summary>
		/// Gets or sets the size of the indent.
		/// </summary>
		/// <value>The size of the indent.</value>
		public int IndentSize { get; set; }

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="IndentInfo"/> class.
		/// It uses <c>\t</c> as indent character.
		/// </summary>
		public IndentInfo()
		{
			IndentMode = IndentMode.None;
			IndentChar = '\t';
			IndentSize = 1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IndentInfo"/> class.
		/// </summary>
		/// <param name="indentChar">The indent char.</param>
		/// <param name="indentSize">Size of the indent.</param>
		public IndentInfo(char indentChar, int indentSize)
		{
			IndentMode = IndentMode.Indented;
			IndentChar = indentChar;
			IndentSize = indentSize;
		}

		#endregion
	}
}
