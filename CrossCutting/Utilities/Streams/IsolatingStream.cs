using System.IO;

using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Streams
{
	internal class IsolatingStream
        : GenericTransparentStream
	{
		public IsolatingStream(Stream innerStream)
			: base(innerStream)
		{
		}

		public override void Close()
		{
			// do not close inner stream
			// base.Close();
			Patterns.NoOp();
		}

		protected override void Dispose(bool disposing)
		{
			// do not dispose inner stream
			// base.Dispose(disposing);
			Patterns.NoOp();
		}
	}
}
