#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.DesignPatterns.Factory.cs
// --------------------------------------------------------------------------------------
// 
// Representation of the factory design pattern, provides the creation of objects based on an ID.
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

using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.DesignPatterns
{
	/// <summary>
	/// Representation of the factory design pattern, provides the creation of objects based on an ID.
	/// </summary>
	/// <typeparam name="Id">The type of the d.</typeparam>
	/// <typeparam name="Type">The type of the ype.</typeparam>
	public class Factory<Id, Type> where Type: class
	{
		#region Constants, enums, definations

		/// <summary>
		/// the delegate that gets called to do the actual construction
		/// </summary>
		/// <returns></returns>
		public delegate Type Constructor();

		#endregion

		#region Private Members

		/// <summary>
		/// map the Id's to factory functions
		/// </summary>
		private SortedDictionary<Id, Constructor> m_classMap = new SortedDictionary<Id, Constructor>();

		#endregion

		#region Constructors

		/// <summary>
		/// default constructor
		/// </summary>
		public Factory()
		{ }

		#endregion

		#region Public methods

		/// <summary>
		/// Creates the specified object from it's classId
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>The object created</returns>
		/// <exception cref="UnknownFactoryIdException">Thrown when the Id isn't registered in the factory</exception>
		public Type Create(Id id)
		{
			Constructor generator = null;
			Type ret = null;

			if (m_classMap.TryGetValue(id, out generator))
				ret = generator();
			else
				throw new UnknownFactoryIdException();

			return ret;
		}

		/// <summary>
		/// Register a class in the factory
		/// </summary>
		/// <param name="classId">The classId of the class to add to factory</param>
		/// <param name="classToAdd">The class to add to the factory</param>
		public void Register(Id classId, Constructor classToAdd)
		{
			m_classMap.Add(classId, classToAdd);
		}

		#endregion
	}

}
