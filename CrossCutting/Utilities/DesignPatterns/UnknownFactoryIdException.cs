#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.DesignPatterns.UnknownFactoryIdException.cs
// --------------------------------------------------------------------------------------
// 
// Exception thrown requesting the FactoryID that doesn't exist from the Indigo.CrossCutting.Utilities.DesignPatters.Factory class
//
// Copyright (c) 2009 Sepura Plc
//
// Sepura Confidential (c)
//
// Created: November 2009 : Simon Hirst
// 
//
// --------------------------------------------------------------------------------------
#endregion

using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.DesignPatterns
{
	/// <summary>
	/// Exception thrown requesting the FactoryID that doesn't exist from the Indigo.CrossCutting.Utilities.DesignPatters.Factory class
	/// </summary>
	public class UnknownFactoryIdException: Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UnknownFactoryIdException"/> class.
		/// </summary>
		public UnknownFactoryIdException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnknownFactoryIdException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public UnknownFactoryIdException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnknownFactoryIdException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public UnknownFactoryIdException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnknownFactoryIdException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
		protected UnknownFactoryIdException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
