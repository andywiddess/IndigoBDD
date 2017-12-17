
/* 
	This files has been generated from ObjectServices.tt T4 template. 
	Do not change it, change the template if you want your changes to be persistent.
*/

using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Helper class to construct ad hoc comparison functions.</summary>
	/// <typeparam name="T">Type of item to compare</typeparam>
	public class ObjectServices<T>
	{
		#region fields
	
		private Func<T, T, bool> m_Equality;
		private Func<T, object, bool> m_EqualityAny;
		private Func<T, int> m_HashCode;
		private Comparison<T> m_Comparision;
		
		#endregion
		
		#region properties

		/// <summary>Equality function.</summary>
		public Func<T, T, bool> Equality { get { return m_Equality; } }

		/// <summary>Equality function (for uknown object)</summary>
		public Func<T, object, bool> EqualityToAny { get { return m_EqualityAny; } }

		/// <summary>Comparison function.</summary>
		public Comparison<T> Comparision { get { return m_Comparision; } }

		/// <summary>Hash function.</summary>
		public Func<T, int> Hasher { get { return m_HashCode; } }
		
		#endregion
		
		#region constructor
		
		private ObjectServices()
		{
		}
		
		#endregion
	
		#region Services for <TField1>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1>(
			T that, T other, 
			Func<T, TField1> fun1
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1>(
			Func<T, TField1> fun1
		) {
			return (a, b) => CompareComposite(a, b, fun1);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1>(
			T that, T other,
			Func<T, TField1> fun1
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1>(
			Func<T, TField1> fun1
		) {
			return (a, b) => EqualsComposite(a, b, fun1);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1>(
			T that, object any,
			Func<T, TField1> fun1
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1>(
			Func<T, TField1> fun1
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1>(
			T that, 
			Func<T, TField1> fun1
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1>(
			Func<T, TField1> fun1
		) {
			return (that) => HashComposite(that, fun1);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1>(
			Func<T, TField1> fun1
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1)
			};
		}

		#endregion

		#region Services for <TField1, TField2>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1, TField2>(
			T that, T other, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(fun2(that), fun2(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1, TField2>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			return (a, b) => CompareComposite(a, b, fun1, fun2);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1, TField2>(
			T that, T other,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1, TField2>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			return (a, b) => EqualsComposite(a, b, fun1, fun2);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1, TField2>(
			T that, object any,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1, TField2>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1, fun2);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1, TField2>(
			T that, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that), fun2(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1, TField2>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			return (that) => HashComposite(that, fun1, fun2);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1, TField2>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1, fun2),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1, fun2),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1, fun2),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1, fun2)
			};
		}

		#endregion

		#region Services for <TField1, TField2, TField3>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1, TField2, TField3>(
			T that, T other, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(fun2(that), fun2(other))) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(fun3(that), fun3(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1, TField2, TField3>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			return (a, b) => CompareComposite(a, b, fun1, fun2, fun3);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1, TField2, TField3>(
			T that, T other,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1, TField2, TField3>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			return (a, b) => EqualsComposite(a, b, fun1, fun2, fun3);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1, TField2, TField3>(
			T that, object any,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1, TField2, TField3>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1, fun2, fun3);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1, TField2, TField3>(
			T that, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that), fun2(that), fun3(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1, TField2, TField3>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			return (that) => HashComposite(that, fun1, fun2, fun3);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1, TField2, TField3>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1, fun2, fun3),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1, fun2, fun3),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1, fun2, fun3),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1, fun2, fun3)
			};
		}

		#endregion

		#region Services for <TField1, TField2, TField3, TField4>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1, TField2, TField3, TField4>(
			T that, T other, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(fun2(that), fun2(other))) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(fun3(that), fun3(other))) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(fun4(that), fun4(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1, TField2, TField3, TField4>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			return (a, b) => CompareComposite(a, b, fun1, fun2, fun3, fun4);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1, TField2, TField3, TField4>(
			T that, T other,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1, TField2, TField3, TField4>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			return (a, b) => EqualsComposite(a, b, fun1, fun2, fun3, fun4);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1, TField2, TField3, TField4>(
			T that, object any,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1, TField2, TField3, TField4>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1, fun2, fun3, fun4);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1, TField2, TField3, TField4>(
			T that, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that), fun2(that), fun3(that), fun4(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1, TField2, TField3, TField4>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			return (that) => HashComposite(that, fun1, fun2, fun3, fun4);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1, TField2, TField3, TField4>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1, fun2, fun3, fun4),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1, fun2, fun3, fun4),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1, fun2, fun3, fun4),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1, fun2, fun3, fun4)
			};
		}

		#endregion

		#region Services for <TField1, TField2, TField3, TField4, TField5>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1, TField2, TField3, TField4, TField5>(
			T that, T other, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(fun2(that), fun2(other))) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(fun3(that), fun3(other))) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(fun4(that), fun4(other))) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(fun5(that), fun5(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1, TField2, TField3, TField4, TField5>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			return (a, b) => CompareComposite(a, b, fun1, fun2, fun3, fun4, fun5);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1, TField2, TField3, TField4, TField5>(
			T that, T other,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1, TField2, TField3, TField4, TField5>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			return (a, b) => EqualsComposite(a, b, fun1, fun2, fun3, fun4, fun5);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1, TField2, TField3, TField4, TField5>(
			T that, object any,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1, TField2, TField3, TField4, TField5>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1, fun2, fun3, fun4, fun5);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1, TField2, TField3, TField4, TField5>(
			T that, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that), fun2(that), fun3(that), fun4(that), fun5(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1, TField2, TField3, TField4, TField5>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			return (that) => HashComposite(that, fun1, fun2, fun3, fun4, fun5);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1, TField2, TField3, TField4, TField5>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1, fun2, fun3, fun4, fun5),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1, fun2, fun3, fun4, fun5),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1, fun2, fun3, fun4, fun5),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1, fun2, fun3, fun4, fun5)
			};
		}

		#endregion

		#region Services for <TField1, TField2, TField3, TField4, TField5, TField6>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1, TField2, TField3, TField4, TField5, TField6>(
			T that, T other, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(fun2(that), fun2(other))) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(fun3(that), fun3(other))) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(fun4(that), fun4(other))) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(fun5(that), fun5(other))) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(fun6(that), fun6(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1, TField2, TField3, TField4, TField5, TField6>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			return (a, b) => CompareComposite(a, b, fun1, fun2, fun3, fun4, fun5, fun6);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1, TField2, TField3, TField4, TField5, TField6>(
			T that, T other,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			if (!EqualityComparer<TField6>.Default.Equals(fun6(that), fun6(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1, TField2, TField3, TField4, TField5, TField6>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			return (a, b) => EqualsComposite(a, b, fun1, fun2, fun3, fun4, fun5, fun6);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1, TField2, TField3, TField4, TField5, TField6>(
			T that, object any,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			if (!EqualityComparer<TField6>.Default.Equals(fun6(that), fun6(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1, TField2, TField3, TField4, TField5, TField6>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1, fun2, fun3, fun4, fun5, fun6);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1, TField2, TField3, TField4, TField5, TField6>(
			T that, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that), fun2(that), fun3(that), fun4(that), fun5(that), fun6(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1, TField2, TField3, TField4, TField5, TField6>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			return (that) => HashComposite(that, fun1, fun2, fun3, fun4, fun5, fun6);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1, TField2, TField3, TField4, TField5, TField6>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1, fun2, fun3, fun4, fun5, fun6),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1, fun2, fun3, fun4, fun5, fun6),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1, fun2, fun3, fun4, fun5, fun6),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1, fun2, fun3, fun4, fun5, fun6)
			};
		}

		#endregion

		#region Services for <TField1, TField2, TField3, TField4, TField5, TField6, TField7>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			T that, T other, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(fun2(that), fun2(other))) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(fun3(that), fun3(other))) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(fun4(that), fun4(other))) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(fun5(that), fun5(other))) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(fun6(that), fun6(other))) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(fun7(that), fun7(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			return (a, b) => CompareComposite(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			T that, T other,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			if (!EqualityComparer<TField6>.Default.Equals(fun6(that), fun6(other))) return false;
			if (!EqualityComparer<TField7>.Default.Equals(fun7(that), fun7(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			return (a, b) => EqualsComposite(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			T that, object any,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			if (!EqualityComparer<TField6>.Default.Equals(fun6(that), fun6(other))) return false;
			if (!EqualityComparer<TField7>.Default.Equals(fun7(that), fun7(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			T that, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that), fun2(that), fun3(that), fun4(that), fun5(that), fun6(that), fun7(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			return (that) => HashComposite(that, fun1, fun2, fun3, fun4, fun5, fun6, fun7);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1, fun2, fun3, fun4, fun5, fun6, fun7),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1, fun2, fun3, fun4, fun5, fun6, fun7),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1, fun2, fun3, fun4, fun5, fun6, fun7),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1, fun2, fun3, fun4, fun5, fun6, fun7)
			};
		}

		#endregion

		#region Services for <TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			T that, T other, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(fun2(that), fun2(other))) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(fun3(that), fun3(other))) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(fun4(that), fun4(other))) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(fun5(that), fun5(other))) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(fun6(that), fun6(other))) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(fun7(that), fun7(other))) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(fun8(that), fun8(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			return (a, b) => CompareComposite(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			T that, T other,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			if (!EqualityComparer<TField6>.Default.Equals(fun6(that), fun6(other))) return false;
			if (!EqualityComparer<TField7>.Default.Equals(fun7(that), fun7(other))) return false;
			if (!EqualityComparer<TField8>.Default.Equals(fun8(that), fun8(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			return (a, b) => EqualsComposite(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			T that, object any,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			if (!EqualityComparer<TField6>.Default.Equals(fun6(that), fun6(other))) return false;
			if (!EqualityComparer<TField7>.Default.Equals(fun7(that), fun7(other))) return false;
			if (!EqualityComparer<TField8>.Default.Equals(fun8(that), fun8(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			T that, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that), fun2(that), fun3(that), fun4(that), fun5(that), fun6(that), fun7(that), fun8(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			return (that) => HashComposite(that, fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8)
			};
		}

		#endregion

		#region Services for <TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>
		
		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>Result of comparision.</returns>
		public static int CompareComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			T that, T other, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			if (object.ReferenceEquals(that, other)) return 0;
			if (object.ReferenceEquals(that, null)) return -1;
			if (object.ReferenceEquals(other, null)) return 1;

			int c; // = 0; 
			if ((c = Comparer<TField1>.Default.Compare(fun1(that), fun1(other))) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(fun2(that), fun2(other))) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(fun3(that), fun3(other))) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(fun4(that), fun4(other))) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(fun5(that), fun5(other))) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(fun6(that), fun6(other))) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(fun7(that), fun7(other))) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(fun8(that), fun8(other))) != 0) return c;
			if ((c = Comparer<TField9>.Default.Compare(fun9(that), fun9(other))) != 0) return c;

			return 0;
		}

		/// <summary>Composes comparision function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>The comparison function.</returns>
		public static Comparison<T> CompositeComparison<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			return (a, b) => CompareComposite(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8, fun9);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="other">The other object.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			T that, T other,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			if (object.ReferenceEquals(that, other)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(other, null)) return false;
			
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			if (!EqualityComparer<TField6>.Default.Equals(fun6(that), fun6(other))) return false;
			if (!EqualityComparer<TField7>.Default.Equals(fun7(that), fun7(other))) return false;
			if (!EqualityComparer<TField8>.Default.Equals(fun8(that), fun8(other))) return false;
			if (!EqualityComparer<TField9>.Default.Equals(fun9(that), fun9(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, T, bool> CompositeEquality<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			return (a, b) => EqualsComposite(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8, fun9);
		}

		/// <summary>Compares composite objects.</summary>
		/// <param name="that">The this object.</param>
		/// <param name="any">The other object (of unkown type).</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>Result of comparision.</returns>
		public static bool EqualsCompositeToAny<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			T that, object any,
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			if (object.ReferenceEquals(that, any)) return true;
			if (object.ReferenceEquals(that, null)) return false;
			if (object.ReferenceEquals(any, null)) return false;
			if (!(any is T)) return false;

			var other = (T)any; // can't use 'as', because it requires 'class' (and it may be a 'struct')
			if (!EqualityComparer<TField1>.Default.Equals(fun1(that), fun1(other))) return false;
			if (!EqualityComparer<TField2>.Default.Equals(fun2(that), fun2(other))) return false;
			if (!EqualityComparer<TField3>.Default.Equals(fun3(that), fun3(other))) return false;
			if (!EqualityComparer<TField4>.Default.Equals(fun4(that), fun4(other))) return false;
			if (!EqualityComparer<TField5>.Default.Equals(fun5(that), fun5(other))) return false;
			if (!EqualityComparer<TField6>.Default.Equals(fun6(that), fun6(other))) return false;
			if (!EqualityComparer<TField7>.Default.Equals(fun7(that), fun7(other))) return false;
			if (!EqualityComparer<TField8>.Default.Equals(fun8(that), fun8(other))) return false;
			if (!EqualityComparer<TField9>.Default.Equals(fun9(that), fun9(other))) return false;
			return true;
		}
		
		/// <summary>Composes equality function.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>The comparison function.</returns>
		public static Func<T, object, bool> CompositeEqualityToAny<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			return (a, b) => EqualsCompositeToAny(a, b, fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8, fun9);
		}
		
		/// <summary>Hashes the composite object.</summary>
		/// <param name="that">The that.</param>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>Hash calculated for extracted expressions.</returns>
		public static int HashComposite<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			T that, 
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			if (that == null) return HashCode.Hash(that);
			return HashCode.Hash(fun1(that), fun2(that), fun3(that), fun4(that), fun5(that), fun6(that), fun7(that), fun8(that), fun9(that));
		}

		/// <summary>Composes hash function for composite object.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>Function which calculates hash for given object.</returns>
		public static Func<T, int> CompositeHasher<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			return (that) => HashComposite(that, fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8, fun9);
		}
		
		/// <summary>Generates an object with easily accessible service functions.</summary>
		/// <typeparam name="TField1">The type of the expression 1.</typeparam>
		/// <param name="fun1">The extractor function for expression 1.</param>
		/// <typeparam name="TField2">The type of the expression 2.</typeparam>
		/// <param name="fun2">The extractor function for expression 2.</param>
		/// <typeparam name="TField3">The type of the expression 3.</typeparam>
		/// <param name="fun3">The extractor function for expression 3.</param>
		/// <typeparam name="TField4">The type of the expression 4.</typeparam>
		/// <param name="fun4">The extractor function for expression 4.</param>
		/// <typeparam name="TField5">The type of the expression 5.</typeparam>
		/// <param name="fun5">The extractor function for expression 5.</param>
		/// <typeparam name="TField6">The type of the expression 6.</typeparam>
		/// <param name="fun6">The extractor function for expression 6.</param>
		/// <typeparam name="TField7">The type of the expression 7.</typeparam>
		/// <param name="fun7">The extractor function for expression 7.</param>
		/// <typeparam name="TField8">The type of the expression 8.</typeparam>
		/// <param name="fun8">The extractor function for expression 8.</param>
		/// <typeparam name="TField9">The type of the expression 9.</typeparam>
		/// <param name="fun9">The extractor function for expression 9.</param>
		/// <returns>An object with easily accessible service functions.</returns>
		public static ObjectServices<T> Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			Func<T, TField1> fun1,
			Func<T, TField2> fun2,
			Func<T, TField3> fun3,
			Func<T, TField4> fun4,
			Func<T, TField5> fun5,
			Func<T, TField6> fun6,
			Func<T, TField7> fun7,
			Func<T, TField8> fun8,
			Func<T, TField9> fun9
		) {
			return new ObjectServices<T>
			{
				m_Equality = ObjectServices<T>.CompositeEquality(fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8, fun9),
				m_Comparision = ObjectServices<T>.CompositeComparison(fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8, fun9),
				m_EqualityAny = ObjectServices<T>.CompositeEqualityToAny(fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8, fun9),
				m_HashCode = ObjectServices<T>.CompositeHasher(fun1, fun2, fun3, fun4, fun5, fun6, fun7, fun8, fun9)
			};
		}

		#endregion

	}
}