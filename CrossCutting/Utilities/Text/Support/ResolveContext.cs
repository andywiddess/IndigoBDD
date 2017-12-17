using System;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.Collections;

namespace Indigo.CrossCutting.Utilities.Text.Support
{
	/// <summary>Resolution (expansion) context. Used when <see cref="TemplateString"/> is resolved.</summary>
	public class ResolveContext
	{
		#region properties

		/// <summary>Gets the resolver function.</summary>
		public Func<string, object> Resolver { get; internal set; }

		/// <summary>Expanded chunks (so far).</summary>
		public LinkedList<AbstractChunk> Chunks { get; internal set; }

		/// <summary>Gets or sets a value indicating whether after expansion <see cref="TemplateString"/> will be still 
		/// expandable. As it is not required (because it could be checked later, it can be done here with no additional 
		/// complexity, so it may increase performance.</summary>
		/// <value><c>true</c> if expandable; otherwise, <c>false</c>.</value>
		internal bool Expandable { get; set; }

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ResolveContext"/> class.
		/// </summary>
		/// <param name="resolver">The resolver.</param>
		public ResolveContext(Func<string, object> resolver) => Resolver = resolver;

	    #endregion

		#region public interface

        /// <summary>
        /// Adds the specified chunk.
        /// </summary>
        ///
        /// <param name="chunk">    The chunk. </param>
		public void Add(AbstractChunk chunk)
		{
			if (Chunks == null)
			{
				Chunks = new LinkedList<AbstractChunk>();
			}
			Chunks.AddLast(chunk);
		}

        /// <summary>
        /// Adds many chunks.
        /// </summary>
        ///
        /// <param name="newChunks">    The new chunks. </param>
		public void AddMany(IEnumerable<AbstractChunk> newChunks)
		{
			if (Chunks == null)
			{
				Chunks = new LinkedList<AbstractChunk>();
			}

			var oldChunks = Chunks;
			newChunks.ForEach((c) => oldChunks.AddLast(c));
		}

		#endregion
	}
}
