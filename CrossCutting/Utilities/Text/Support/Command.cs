#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.Text.Support.Command.cs
// --------------------------------------------------------------------------------------
//
// TemplateString commands can be injected into processing. 
//
// Copyright (c) 2011 Sepura Plc
//
// Sepura Confidential
//
// Created: Milosz Krajewski
//
// --------------------------------------------------------------------------------------
#endregion

namespace Indigo.CrossCutting.Utilities.Text.Support
{
	/// <summary>
	/// TemplateString commands can be injected into processing. 
	/// </summary>
	public abstract class Command
	{
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        ///
        /// <param name="context">  The context. </param>
        /// <param name="name">     The name. </param>
        /// <param name="format">   The format. </param>
        ///
        /// <returns>
        /// Result of command to be inserted into Templatestring.
        /// </returns>
		public abstract string Execute(ResolveContext context, string name, string format);
	}
}