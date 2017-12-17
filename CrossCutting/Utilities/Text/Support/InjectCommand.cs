using System;

namespace Indigo.CrossCutting.Utilities.Text.Support
{
	internal class InjectCommand
        : Command
	{
		private readonly string m_Text;

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="text"> The text. </param>
		public InjectCommand(string text) => m_Text = text;

        ///  <summary>
        ///  Executes the specified context.
        ///  </summary>
        /// 
        ///  <param name="context">  The context. </param>
        ///  <param name="name">     The name. </param>
        ///  <param name="format">   The format. </param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns>
        ///  Result of command to be inserted into Templatestring.
        ///  </returns>
        public override string Execute(ResolveContext context, string name, string format) => throw new NotImplementedException();
    }
}
