namespace Indigo.CrossCutting.Utilities.Text.Support
{
	/// <summary>Special chunk used for indentation.</summary>
	public class UnindentChunk
        : AbstractChunk
	{
		#region static fields

		/// <summary>We need only one of these.</summary>
		public static readonly UnindentChunk Default = new UnindentChunk();

		#endregion

		#region properties

		/// <summary>Gets a value indicating whether this chunk is multiline.</summary>
		/// <value><c>true</c> if this instance is multiline; otherwise, <c>false</c>.</value>
		public override bool IsMultiline => false;

	    #endregion

		#region constructor

		/// <summary>
		/// Prevents a default instance of the <see cref="UnindentChunk"/> class from being created.
		/// </summary>
		private UnindentChunk()
		{
		}

        #endregion

        #region public interface

        /// <summary>Produces (converts chunk to string) the chunk to specified context.</summary>
        /// <param name="context">The context.</param>
        public override void Produce(ProduceContext context) => context.Unindent();

        /// <summary>Resolves (expands) the chunk to specified context.</summary>
        /// <param name="context">The context.</param>
        public override void Resolve(ResolveContext context) => context.Add(this);

        #endregion
    }
}
