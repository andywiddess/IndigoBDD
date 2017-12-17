
/* 
	This files has been generated from RTuple.tt T4 template. 
	Do not change it, change the template if you want your changes to be persistent. 
*/

using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	#region RTuple helper class

	/// <summary>RTuple helper class. Makes creating tuples easier thanks to type inference (can't do the same with constuctor).</summary>
	public static class RTuple
	{
	
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2> 
			Make<TField1, TField2>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2))
		{
			return new RTuple<TField1, TField2>(
				item1,
				item2);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2> 
			Clone<TField1, TField2>(
			this RTuple<TField1, TField2> other)
		{
			return new RTuple<TField1, TField2>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3> 
			Make<TField1, TField2, TField3>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3))
		{
			return new RTuple<TField1, TField2, TField3>(
				item1,
				item2,
				item3);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3> 
			Clone<TField1, TField2, TField3>(
			this RTuple<TField1, TField2, TField3> other)
		{
			return new RTuple<TField1, TField2, TField3>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4> 
			Make<TField1, TField2, TField3, TField4>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4))
		{
			return new RTuple<TField1, TField2, TField3, TField4>(
				item1,
				item2,
				item3,
				item4);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4> 
			Clone<TField1, TField2, TField3, TField4>(
			this RTuple<TField1, TField2, TField3, TField4> other)
		{
			return new RTuple<TField1, TField2, TField3, TField4>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5> 
			Make<TField1, TField2, TField3, TField4, TField5>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5))
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5>(
				item1,
				item2,
				item3,
				item4,
				item5);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5> 
			Clone<TField1, TField2, TField3, TField4, TField5>(
			this RTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <param name="item6">The value 6.</param>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5, TField6> 
			Make<TField1, TField2, TField3, TField4, TField5, TField6>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6))
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5, TField6>(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5, TField6> 
			Clone<TField1, TField2, TField3, TField4, TField5, TField6>(
			this RTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5, TField6>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <param name="item6">The value 6.</param>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <param name="item7">The value 7.</param>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> 
			Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7))
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
				item7);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> 
			Clone<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			this RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <param name="item6">The value 6.</param>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <param name="item7">The value 7.</param>
		/// <typeparam name="TField8">The type of the field 8.</typeparam>
		/// <param name="item8">The value 8.</param>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> 
			Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7),
			TField8 item8 = default(TField8))
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
				item7,
				item8);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <typeparam name="TField8">The type of the field 8.</typeparam>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> 
			Clone<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			this RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <param name="item6">The value 6.</param>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <param name="item7">The value 7.</param>
		/// <typeparam name="TField8">The type of the field 8.</typeparam>
		/// <param name="item8">The value 8.</param>
		/// <typeparam name="TField9">The type of the field 9.</typeparam>
		/// <param name="item9">The value 9.</param>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> 
			Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7),
			TField8 item8 = default(TField8),
			TField9 item9 = default(TField9))
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
				item7,
				item8,
				item9);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <typeparam name="TField8">The type of the field 8.</typeparam>
		/// <typeparam name="TField9">The type of the field 9.</typeparam>
		/// <returns>New tuple.</returns>
		public static RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> 
			Clone<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			this RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			return new RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(other);
		}
		
	}
	
	#endregion
	
	#region VTuple helper class

	/// <summary>VTuple helper class. Makes creting tuples easier thanks to type inference (can't do the same with constuctor).</summary>
	public static class VTuple
	{
	
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2> 
			Make<TField1, TField2>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2))
		{
			return new VTuple<TField1, TField2>(
				item1,
				item2);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2> 
			Clone<TField1, TField2>(
			ITuple<TField1, TField2> other)
		{
			return new VTuple<TField1, TField2>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3> 
			Make<TField1, TField2, TField3>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3))
		{
			return new VTuple<TField1, TField2, TField3>(
				item1,
				item2,
				item3);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3> 
			Clone<TField1, TField2, TField3>(
			ITuple<TField1, TField2, TField3> other)
		{
			return new VTuple<TField1, TField2, TField3>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4> 
			Make<TField1, TField2, TField3, TField4>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4))
		{
			return new VTuple<TField1, TField2, TField3, TField4>(
				item1,
				item2,
				item3,
				item4);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4> 
			Clone<TField1, TField2, TField3, TField4>(
			ITuple<TField1, TField2, TField3, TField4> other)
		{
			return new VTuple<TField1, TField2, TField3, TField4>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5> 
			Make<TField1, TField2, TField3, TField4, TField5>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5))
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5>(
				item1,
				item2,
				item3,
				item4,
				item5);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5> 
			Clone<TField1, TField2, TField3, TField4, TField5>(
			ITuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <param name="item6">The value 6.</param>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5, TField6> 
			Make<TField1, TField2, TField3, TField4, TField5, TField6>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6))
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5, TField6>(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5, TField6> 
			Clone<TField1, TField2, TField3, TField4, TField5, TField6>(
			ITuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5, TField6>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <param name="item6">The value 6.</param>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <param name="item7">The value 7.</param>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> 
			Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7))
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
				item7);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> 
			Clone<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(
			ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <param name="item6">The value 6.</param>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <param name="item7">The value 7.</param>
		/// <typeparam name="TField8">The type of the field 8.</typeparam>
		/// <param name="item8">The value 8.</param>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> 
			Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7),
			TField8 item8 = default(TField8))
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
				item7,
				item8);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <typeparam name="TField8">The type of the field 8.</typeparam>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> 
			Clone<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(
			ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>(other);
		}
		
		/// <summary>Makes the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <param name="item1">The value 1.</param>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <param name="item2">The value 2.</param>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <param name="item3">The value 3.</param>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <param name="item4">The value 4.</param>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <param name="item5">The value 5.</param>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <param name="item6">The value 6.</param>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <param name="item7">The value 7.</param>
		/// <typeparam name="TField8">The type of the field 8.</typeparam>
		/// <param name="item8">The value 8.</param>
		/// <typeparam name="TField9">The type of the field 9.</typeparam>
		/// <param name="item9">The value 9.</param>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> 
			Make<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7),
			TField8 item8 = default(TField8),
			TField9 item9 = default(TField9))
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
				item7,
				item8,
				item9);
		}
		
		/// <summary>Clones the tuple.</summary>
		/// <typeparam name="TField1">The type of the field 1.</typeparam>
		/// <typeparam name="TField2">The type of the field 2.</typeparam>
		/// <typeparam name="TField3">The type of the field 3.</typeparam>
		/// <typeparam name="TField4">The type of the field 4.</typeparam>
		/// <typeparam name="TField5">The type of the field 5.</typeparam>
		/// <typeparam name="TField6">The type of the field 6.</typeparam>
		/// <typeparam name="TField7">The type of the field 7.</typeparam>
		/// <typeparam name="TField8">The type of the field 8.</typeparam>
		/// <typeparam name="TField9">The type of the field 9.</typeparam>
		/// <returns>New tuple.</returns>
		public static VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> 
			Clone<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(
			ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			return new VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>(other);
		}
		
	}
	
	#endregion

	#region ITuple<TField1, TField2>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	public interface ITuple<TField1, TField2>: 
		IEquatable<ITuple<TField1, TField2>>,
		IComparable<ITuple<TField1, TField2>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		TField2 Item2 { get; set; }

		#endregion
	}
	
	#endregion

	#region RTuple<TField1, TField2>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// Please note, that RTuple is a class and has all the advantages and disadvantages of a class.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	public class RTuple<TField1, TField2>: 
		ITuple<TField1, TField2>,
		IComparable<RTuple<TField1, TField2>>,
		IComparable<VTuple<TField1, TField2>>,
		IEquatable<RTuple<TField1, TField2>>,
		IEquatable<VTuple<TField1, TField2>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		public RTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2))
		{
			Item1 = item1;
			Item2 = item2;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public RTuple(ITuple<TField1, TField2> other)
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1}";
			return string.Format(
				"RTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
	
	#region VTuple<TField1, TField2>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly. 
	/// Please note, that VTuple is a struct and has all the advantages and disadvantages of a struct.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	public struct VTuple<TField1, TField2>: 
		ITuple<TField1, TField2>,
		IComparable<RTuple<TField1, TField2>>,
		IComparable<VTuple<TField1, TField2>>,
		IEquatable<RTuple<TField1, TField2>>,
		IEquatable<VTuple<TField1, TField2>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		public VTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2))
			: this()
		{
			Item1 = item1;
			Item2 = item2;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public VTuple(ITuple<TField1, TField2> other)
			: this()
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1}";
			return string.Format(
				"VTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion

	#region ITuple<TField1, TField2, TField3>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	public interface ITuple<TField1, TField2, TField3>: 
		IEquatable<ITuple<TField1, TField2, TField3>>,
		IComparable<ITuple<TField1, TField2, TField3>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		TField3 Item3 { get; set; }

		#endregion
	}
	
	#endregion

	#region RTuple<TField1, TField2, TField3>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// Please note, that RTuple is a class and has all the advantages and disadvantages of a class.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	public class RTuple<TField1, TField2, TField3>: 
		ITuple<TField1, TField2, TField3>,
		IComparable<RTuple<TField1, TField2, TField3>>,
		IComparable<VTuple<TField1, TField2, TField3>>,
		IEquatable<RTuple<TField1, TField2, TField3>>,
		IEquatable<VTuple<TField1, TField2, TField3>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		public RTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3))
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public RTuple(ITuple<TField1, TField2, TField3> other)
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2}";
			return string.Format(
				"RTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
	
	#region VTuple<TField1, TField2, TField3>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly. 
	/// Please note, that VTuple is a struct and has all the advantages and disadvantages of a struct.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	public struct VTuple<TField1, TField2, TField3>: 
		ITuple<TField1, TField2, TField3>,
		IComparable<RTuple<TField1, TField2, TField3>>,
		IComparable<VTuple<TField1, TField2, TField3>>,
		IEquatable<RTuple<TField1, TField2, TField3>>,
		IEquatable<VTuple<TField1, TField2, TField3>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		public VTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3))
			: this()
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public VTuple(ITuple<TField1, TField2, TField3> other)
			: this()
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2}";
			return string.Format(
				"VTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion

	#region ITuple<TField1, TField2, TField3, TField4>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	public interface ITuple<TField1, TField2, TField3, TField4>: 
		IEquatable<ITuple<TField1, TField2, TField3, TField4>>,
		IComparable<ITuple<TField1, TField2, TField3, TField4>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		TField4 Item4 { get; set; }

		#endregion
	}
	
	#endregion

	#region RTuple<TField1, TField2, TField3, TField4>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// Please note, that RTuple is a class and has all the advantages and disadvantages of a class.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	public class RTuple<TField1, TField2, TField3, TField4>: 
		ITuple<TField1, TField2, TField3, TField4>,
		IComparable<RTuple<TField1, TField2, TField3, TField4>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		public RTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4))
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public RTuple(ITuple<TField1, TField2, TField3, TField4> other)
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3}";
			return string.Format(
				"RTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
	
	#region VTuple<TField1, TField2, TField3, TField4>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly. 
	/// Please note, that VTuple is a struct and has all the advantages and disadvantages of a struct.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	public struct VTuple<TField1, TField2, TField3, TField4>: 
		ITuple<TField1, TField2, TField3, TField4>,
		IComparable<RTuple<TField1, TField2, TField3, TField4>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		public VTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4))
			: this()
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public VTuple(ITuple<TField1, TField2, TField3, TField4> other)
			: this()
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3}";
			return string.Format(
				"VTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion

	#region ITuple<TField1, TField2, TField3, TField4, TField5>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	public interface ITuple<TField1, TField2, TField3, TField4, TField5>: 
		IEquatable<ITuple<TField1, TField2, TField3, TField4, TField5>>,
		IComparable<ITuple<TField1, TField2, TField3, TField4, TField5>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		TField5 Item5 { get; set; }

		#endregion
	}
	
	#endregion

	#region RTuple<TField1, TField2, TField3, TField4, TField5>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// Please note, that RTuple is a class and has all the advantages and disadvantages of a class.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	public class RTuple<TField1, TField2, TField3, TField4, TField5>: 
		ITuple<TField1, TField2, TField3, TField4, TField5>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		public RTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5))
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public RTuple(ITuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4}";
			return string.Format(
				"RTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
	
	#region VTuple<TField1, TField2, TField3, TField4, TField5>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly. 
	/// Please note, that VTuple is a struct and has all the advantages and disadvantages of a struct.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	public struct VTuple<TField1, TField2, TField3, TField4, TField5>: 
		ITuple<TField1, TField2, TField3, TField4, TField5>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		public VTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5))
			: this()
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public VTuple(ITuple<TField1, TField2, TField3, TField4, TField5> other)
			: this()
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4}";
			return string.Format(
				"VTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion

	#region ITuple<TField1, TField2, TField3, TField4, TField5, TField6>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	public interface ITuple<TField1, TField2, TField3, TField4, TField5, TField6>: 
		IEquatable<ITuple<TField1, TField2, TField3, TField4, TField5, TField6>>,
		IComparable<ITuple<TField1, TField2, TField3, TField4, TField5, TField6>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		TField6 Item6 { get; set; }

		#endregion
	}
	
	#endregion

	#region RTuple<TField1, TField2, TField3, TField4, TField5, TField6>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// Please note, that RTuple is a class and has all the advantages and disadvantages of a class.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	public class RTuple<TField1, TField2, TField3, TField4, TField5, TField6>: 
		ITuple<TField1, TField2, TField3, TField4, TField5, TField6>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		public TField6 Item6 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		/// <param name="item6">The item 6.</param>
		public RTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6))
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public RTuple(ITuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
			Item6 = other.Item6;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5, TField6>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5, Item6);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4},{5}";
			return string.Format(
				"RTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name,
					typeof(TField6).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString(),
					object.ReferenceEquals(Item6, null) ? "null" : Item6.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
	
	#region VTuple<TField1, TField2, TField3, TField4, TField5, TField6>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly. 
	/// Please note, that VTuple is a struct and has all the advantages and disadvantages of a struct.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	public struct VTuple<TField1, TField2, TField3, TField4, TField5, TField6>: 
		ITuple<TField1, TField2, TField3, TField4, TField5, TField6>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		public TField6 Item6 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		/// <param name="item6">The item 6.</param>
		public VTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6))
			: this()
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public VTuple(ITuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
			: this()
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
			Item6 = other.Item6;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5, TField6>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5, Item6);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4},{5}";
			return string.Format(
				"VTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name,
					typeof(TField6).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString(),
					object.ReferenceEquals(Item6, null) ? "null" : Item6.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5, TField6> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion

	#region ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	public interface ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>: 
		IEquatable<ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>,
		IComparable<ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		TField7 Item7 { get; set; }

		#endregion
	}
	
	#endregion

	#region RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// Please note, that RTuple is a class and has all the advantages and disadvantages of a class.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	public class RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>: 
		ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		public TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		public TField7 Item7 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		/// <param name="item6">The item 6.</param>
		/// <param name="item7">The item 7.</param>
		public RTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7))
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public RTuple(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
			Item6 = other.Item6;
			Item7 = other.Item7;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5, Item6, Item7);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4},{5},{6}";
			return string.Format(
				"RTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name,
					typeof(TField6).Name,
					typeof(TField7).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString(),
					object.ReferenceEquals(Item6, null) ? "null" : Item6.ToString(),
					object.ReferenceEquals(Item7, null) ? "null" : Item7.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
	
	#region VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly. 
	/// Please note, that VTuple is a struct and has all the advantages and disadvantages of a struct.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	public struct VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>: 
		ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		public TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		public TField7 Item7 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		/// <param name="item6">The item 6.</param>
		/// <param name="item7">The item 7.</param>
		public VTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7))
			: this()
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public VTuple(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
			: this()
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
			Item6 = other.Item6;
			Item7 = other.Item7;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5, Item6, Item7);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4},{5},{6}";
			return string.Format(
				"VTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name,
					typeof(TField6).Name,
					typeof(TField7).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString(),
					object.ReferenceEquals(Item6, null) ? "null" : Item6.ToString(),
					object.ReferenceEquals(Item7, null) ? "null" : Item7.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion

	#region ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	/// <typeparam name="TField8">The type of field 8.</typeparam>
	public interface ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>: 
		IEquatable<ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>,
		IComparable<ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		TField7 Item7 { get; set; }
		/// <summary>Gets or sets the item 8.</summary>
		/// <value>The item 8.</value>
		TField8 Item8 { get; set; }

		#endregion
	}
	
	#endregion

	#region RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// Please note, that RTuple is a class and has all the advantages and disadvantages of a class.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	/// <typeparam name="TField8">The type of field 8.</typeparam>
	public class RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>: 
		ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		public TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		public TField7 Item7 { get; set; }
		/// <summary>Gets or sets the item 8.</summary>
		/// <value>The item 8.</value>
		public TField8 Item8 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		/// <param name="item6">The item 6.</param>
		/// <param name="item7">The item 7.</param>
		/// <param name="item8">The item 8.</param>
		public RTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7),
			TField8 item8 = default(TField8))
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
			Item8 = item8;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public RTuple(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
			Item6 = other.Item6;
			Item7 = other.Item7;
			Item8 = other.Item8;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4},{5},{6},{7}";
			return string.Format(
				"RTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name,
					typeof(TField6).Name,
					typeof(TField7).Name,
					typeof(TField8).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString(),
					object.ReferenceEquals(Item6, null) ? "null" : Item6.ToString(),
					object.ReferenceEquals(Item7, null) ? "null" : Item7.ToString(),
					object.ReferenceEquals(Item8, null) ? "null" : Item8.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
	
	#region VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly. 
	/// Please note, that VTuple is a struct and has all the advantages and disadvantages of a struct.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	/// <typeparam name="TField8">The type of field 8.</typeparam>
	public struct VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>: 
		ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		public TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		public TField7 Item7 { get; set; }
		/// <summary>Gets or sets the item 8.</summary>
		/// <value>The item 8.</value>
		public TField8 Item8 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		/// <param name="item6">The item 6.</param>
		/// <param name="item7">The item 7.</param>
		/// <param name="item8">The item 8.</param>
		public VTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7),
			TField8 item8 = default(TField8))
			: this()
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
			Item8 = item8;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public VTuple(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
			: this()
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
			Item6 = other.Item6;
			Item7 = other.Item7;
			Item8 = other.Item8;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4},{5},{6},{7}";
			return string.Format(
				"VTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name,
					typeof(TField6).Name,
					typeof(TField7).Name,
					typeof(TField8).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString(),
					object.ReferenceEquals(Item6, null) ? "null" : Item6.ToString(),
					object.ReferenceEquals(Item7, null) ? "null" : Item7.ToString(),
					object.ReferenceEquals(Item8, null) ? "null" : Item8.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion

	#region ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	/// <typeparam name="TField8">The type of field 8.</typeparam>
	/// <typeparam name="TField9">The type of field 9.</typeparam>
	public interface ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>: 
		IEquatable<ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>,
		IComparable<ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		TField7 Item7 { get; set; }
		/// <summary>Gets or sets the item 8.</summary>
		/// <value>The item 8.</value>
		TField8 Item8 { get; set; }
		/// <summary>Gets or sets the item 9.</summary>
		/// <value>The item 9.</value>
		TField9 Item9 { get; set; }

		#endregion
	}
	
	#endregion

	#region RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly.
	/// Please note, that RTuple is a class and has all the advantages and disadvantages of a class.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	/// <typeparam name="TField8">The type of field 8.</typeparam>
	/// <typeparam name="TField9">The type of field 9.</typeparam>
	public class RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>: 
		ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		public TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		public TField7 Item7 { get; set; }
		/// <summary>Gets or sets the item 8.</summary>
		/// <value>The item 8.</value>
		public TField8 Item8 { get; set; }
		/// <summary>Gets or sets the item 9.</summary>
		/// <value>The item 9.</value>
		public TField9 Item9 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		/// <param name="item6">The item 6.</param>
		/// <param name="item7">The item 7.</param>
		/// <param name="item8">The item 8.</param>
		/// <param name="item9">The item 9.</param>
		public RTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7),
			TField8 item8 = default(TField8),
			TField9 item9 = default(TField9))
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
			Item8 = item8;
			Item9 = item9;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public RTuple(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
			Item6 = other.Item6;
			Item7 = other.Item7;
			Item8 = other.Item8;
			Item9 = other.Item9;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";
			return string.Format(
				"RTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name,
					typeof(TField6).Name,
					typeof(TField7).Name,
					typeof(TField8).Name,
					typeof(TField9).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString(),
					object.ReferenceEquals(Item6, null) ? "null" : Item6.ToString(),
					object.ReferenceEquals(Item7, null) ? "null" : Item7.ToString(),
					object.ReferenceEquals(Item8, null) ? "null" : Item8.ToString(),
					object.ReferenceEquals(Item9, null) ? "null" : Item9.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8) &&
				Item9.Equals(other.Item9);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8) &&
				Item9.Equals(other.Item9);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8) &&
				Item9.Equals(other.Item9);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			if ((c = Comparer<TField9>.Default.Compare(Item9, other.Item9)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			if ((c = Comparer<TField9>.Default.Compare(Item9, other.Item9)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			if ((c = Comparer<TField9>.Default.Compare(Item9, other.Item9)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
	
	#region VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>

	/// <summary>
	/// Tuple which handles Equals and GetHashCode properly. 
	/// Please note, that VTuple is a struct and has all the advantages and disadvantages of a struct.
	/// </summary>
	/// <typeparam name="TField1">The type of field 1.</typeparam>
	/// <typeparam name="TField2">The type of field 2.</typeparam>
	/// <typeparam name="TField3">The type of field 3.</typeparam>
	/// <typeparam name="TField4">The type of field 4.</typeparam>
	/// <typeparam name="TField5">The type of field 5.</typeparam>
	/// <typeparam name="TField6">The type of field 6.</typeparam>
	/// <typeparam name="TField7">The type of field 7.</typeparam>
	/// <typeparam name="TField8">The type of field 8.</typeparam>
	/// <typeparam name="TField9">The type of field 9.</typeparam>
	public struct VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>: 
		ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>,
		IComparable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>,
		IComparable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>,
		IEquatable<RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>,
		IEquatable<VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>>
	{
		#region properties

		/// <summary>Gets or sets the item 1.</summary>
		/// <value>The item 1.</value>
		public TField1 Item1 { get; set; }
		/// <summary>Gets or sets the item 2.</summary>
		/// <value>The item 2.</value>
		public TField2 Item2 { get; set; }
		/// <summary>Gets or sets the item 3.</summary>
		/// <value>The item 3.</value>
		public TField3 Item3 { get; set; }
		/// <summary>Gets or sets the item 4.</summary>
		/// <value>The item 4.</value>
		public TField4 Item4 { get; set; }
		/// <summary>Gets or sets the item 5.</summary>
		/// <value>The item 5.</value>
		public TField5 Item5 { get; set; }
		/// <summary>Gets or sets the item 6.</summary>
		/// <value>The item 6.</value>
		public TField6 Item6 { get; set; }
		/// <summary>Gets or sets the item 7.</summary>
		/// <value>The item 7.</value>
		public TField7 Item7 { get; set; }
		/// <summary>Gets or sets the item 8.</summary>
		/// <value>The item 8.</value>
		public TField8 Item8 { get; set; }
		/// <summary>Gets or sets the item 9.</summary>
		/// <value>The item 9.</value>
		public TField9 Item9 { get; set; }

		#endregion
		
		#region constructor

		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="item1">The item 1.</param>
		/// <param name="item2">The item 2.</param>
		/// <param name="item3">The item 3.</param>
		/// <param name="item4">The item 4.</param>
		/// <param name="item5">The item 5.</param>
		/// <param name="item6">The item 6.</param>
		/// <param name="item7">The item 7.</param>
		/// <param name="item8">The item 8.</param>
		/// <param name="item9">The item 9.</param>
		public VTuple(
			TField1 item1 = default(TField1),
			TField2 item2 = default(TField2),
			TField3 item3 = default(TField3),
			TField4 item4 = default(TField4),
			TField5 item5 = default(TField5),
			TField6 item6 = default(TField6),
			TField7 item7 = default(TField7),
			TField8 item8 = default(TField8),
			TField9 item9 = default(TField9))
			: this()
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
			Item7 = item7;
			Item8 = item8;
			Item9 = item9;
		}
		
		/// <summary>Initializes a new instance of the tuple.</summary>
		/// <param name="other">The other tuple.</param>
		public VTuple(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
			: this()
		{
			Item1 = other.Item1;
			Item2 = other.Item2;
			Item3 = other.Item3;
			Item4 = other.Item4;
			Item5 = other.Item5;
			Item6 = other.Item6;
			Item7 = other.Item7;
			Item8 = other.Item8;
			Item9 = other.Item9;
		}

		#endregion
		
		#region overrides

		/// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9>);
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9);
		}
		
		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			var csv = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";
			return string.Format(
				"VTuple<{0}>({1})",
				string.Format(csv, 
					typeof(TField1).Name,
					typeof(TField2).Name,
					typeof(TField3).Name,
					typeof(TField4).Name,
					typeof(TField5).Name,
					typeof(TField6).Name,
					typeof(TField7).Name,
					typeof(TField8).Name,
					typeof(TField9).Name),
				string.Format(csv,
					object.ReferenceEquals(Item1, null) ? "null" : Item1.ToString(),
					object.ReferenceEquals(Item2, null) ? "null" : Item2.ToString(),
					object.ReferenceEquals(Item3, null) ? "null" : Item3.ToString(),
					object.ReferenceEquals(Item4, null) ? "null" : Item4.ToString(),
					object.ReferenceEquals(Item5, null) ? "null" : Item5.ToString(),
					object.ReferenceEquals(Item6, null) ? "null" : Item6.ToString(),
					object.ReferenceEquals(Item7, null) ? "null" : Item7.ToString(),
					object.ReferenceEquals(Item8, null) ? "null" : Item8.ToString(),
					object.ReferenceEquals(Item9, null) ? "null" : Item9.ToString()));
		}

		#endregion
		
		#region IEquatable Members

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8) &&
				Item9.Equals(other.Item9);
		}
		
		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8) &&
				Item9.Equals(other.Item9);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
		public bool Equals(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			if (object.ReferenceEquals(other, this)) return true;

			return
				Item1.Equals(other.Item1) &&
				Item2.Equals(other.Item2) &&
				Item3.Equals(other.Item3) &&
				Item4.Equals(other.Item4) &&
				Item5.Equals(other.Item5) &&
				Item6.Equals(other.Item6) &&
				Item7.Equals(other.Item7) &&
				Item8.Equals(other.Item8) &&
				Item9.Equals(other.Item9);
		}
		
		#endregion
		
		#region IComparable Members

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(ITuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			if ((c = Comparer<TField9>.Default.Compare(Item9, other.Item9)) != 0) return c;
			return 0;
		}
		
		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(RTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			if ((c = Comparer<TField9>.Default.Compare(Item9, other.Item9)) != 0) return c;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
		public int CompareTo(VTuple<TField1, TField2, TField3, TField4, TField5, TField6, TField7, TField8, TField9> other)
		{
			int c;
			if (object.ReferenceEquals(other, null)) return 1;
			if (object.ReferenceEquals(other, this)) return 0;
			if ((c = Comparer<TField1>.Default.Compare(Item1, other.Item1)) != 0) return c;
			if ((c = Comparer<TField2>.Default.Compare(Item2, other.Item2)) != 0) return c;
			if ((c = Comparer<TField3>.Default.Compare(Item3, other.Item3)) != 0) return c;
			if ((c = Comparer<TField4>.Default.Compare(Item4, other.Item4)) != 0) return c;
			if ((c = Comparer<TField5>.Default.Compare(Item5, other.Item5)) != 0) return c;
			if ((c = Comparer<TField6>.Default.Compare(Item6, other.Item6)) != 0) return c;
			if ((c = Comparer<TField7>.Default.Compare(Item7, other.Item7)) != 0) return c;
			if ((c = Comparer<TField8>.Default.Compare(Item8, other.Item8)) != 0) return c;
			if ((c = Comparer<TField9>.Default.Compare(Item9, other.Item9)) != 0) return c;
			return 0;
		}

		#endregion
	}
	
	#endregion
}