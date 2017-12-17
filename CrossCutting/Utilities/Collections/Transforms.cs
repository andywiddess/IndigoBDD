using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Frequently used data structure transformations.
	/// </summary>
	public static class Transforms
	{
		#region virtual collection

		/// <summary>
		/// Creates virtual directory.
		/// </summary>
		/// <typeparam name="C">Context.</typeparam>
		/// <typeparam name="K">Key.</typeparam>
		/// <typeparam name="V">Value.</typeparam>
		/// <param name="context">The context.</param>
		/// <param name="handler">The handler.</param>
		/// <returns>Dictionary.</returns>
		public static IDictionary<K, V> Virtual<C, K, V>(C context, IVirtualDictionaryHandler<C, K, V> handler)
		{
			return new VirtualDictionary<C, K, V>(context, handler);
		}

		/// <summary>
		/// Creates virtual list.
		/// </summary>
		/// <typeparam name="C">Context.</typeparam>
		/// <typeparam name="T">Type.</typeparam>
		/// <param name="context">The context.</param>
		/// <param name="handler">The handler.</param>
		/// <returns>List.</returns>
		public static IList<T> Virtual<C, T>(C context, IVirtualListHandler<C, T> handler)
		{
			return new VirtualList<C, T>(context, handler);
		}

		/// <summary>
		/// Creates virtual collection.
		/// </summary>
		/// <typeparam name="C">Context.</typeparam>
		/// <typeparam name="T">Type.</typeparam>
		/// <param name="context">The context.</param>
		/// <param name="handler">The handler.</param>
		/// <returns>Collection.</returns>
		public static ICollection<T> Virtual<C, T>(C context, IVirtualCollectionHandler<C, T> handler)
		{
			return new VirtualCollection<C, T>(context, handler);
		}

		#endregion

		#region translate

		/// <summary>
		/// Dynamically translates one dictionary into another.
		/// </summary>
		/// <typeparam name="K">Key.</typeparam>
		/// <typeparam name="SV">Source value.</typeparam>
		/// <typeparam name="TV">Target value.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="translator">The value translator.</param>
		/// <returns>Dictionary.</returns>
		[Obsolete("Superseded by Linq")]
		public static IDictionary<K, TV> Translate<K, SV, TV>(IDictionary<K, SV> source, IObjectTranslator<SV, TV> translator)
		{
			return new TranslatedDictionary<K, SV, TV>(source, translator);
		}

		/// <summary>
		/// Dynamically translates one dictionary into another.
		/// </summary>
		/// <typeparam name="K">Key.</typeparam>
		/// <typeparam name="SV">Source value.</typeparam>
		/// <typeparam name="TV">Target value.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>Dictionary</returns>
		[Obsolete("Superseded by Linq")]
		public static IDictionary<K, TV> Translate<K, SV, TV>(IDictionary<K, SV> source)
		{
			return new TranslatedDictionary<K, SV, TV>(source);
		}

		/// <summary>
		/// Dynamically translates one list into another.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="translator">The translator.</param>
		/// <returns>List.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Translate<S, T>(IList<S> source, IObjectTranslator<S, T> translator)
		{
			return new TranslatedList<S, T>(source, translator);
		}

		/// <summary>
		/// Dynamically translates one list into another.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>List.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Translate<S, T>(IList<S> source)
		{
			return new TranslatedList<S, T>(source);
		}

		/// <summary>
		/// Dynamically translates one collection into another.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="translator">The translator.</param>
		/// <returns>Collection.</returns>
		[Obsolete("Superseded by Linq")]
		public static ICollection<T> Translate<S, T>(ICollection<S> source, IObjectTranslator<S, T> translator)
		{
			return new TranslatedCollection<S, T>(source, translator);
		}

		/// <summary>
		/// Dynamically translates one collection into another.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>Collection.</returns>
		[Obsolete("Superseded by Linq")]
		public static ICollection<T> Translate<S, T>(ICollection<S> source)
		{
			return new TranslatedCollection<S, T>(source);
		}

		/// <summary>
		/// Dynamically translates one IEnumerable into another.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="translator">The translator.</param>
		/// <returns>IEnumerable.</returns>
		[Obsolete("Superseded by Linq")]
		public static IEnumerable<T> Translate<S, T>(IEnumerable<S> source, IObjectTranslator<S, T> translator)
		{
			return new TranslatedEnumerable<S, T>(source, translator);
		}

		/// <summary>
		/// Dynamically translates one IEnumerable into another.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>IEnumerable.</returns>
		[Obsolete("Superseded by Linq")]
		public static IEnumerable<T> Translate<S, T>(IEnumerable<S> source)
		{
			return new TranslatedEnumerable<S, T>(source);
		}

		/// <summary>
		/// Dynamically translates one IEnumerator into another.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="translator">The translator.</param>
		/// <returns>IEnumerator.</returns>
		[Obsolete("Superseded by Linq")]
		public static IEnumerator<T> Translate<S, T>(IEnumerator<S> source, IObjectTranslator<S, T> translator)
		{
			return new TranslatedEnumerator<S, T>(source, translator);
		}

		/// <summary>
		/// Dynamically translates one IEnumerator into another.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>IEnumerator.</returns>
		[Obsolete("Superseded by Linq")]
		public static IEnumerator<T> Translate<S, T>(IEnumerator<S> source)
		{
			return new TranslatedEnumerator<S, T>(source);
		}

		/// <summary>
		/// Converts stream of type <typeparamref name="S"/> to stream of type <typeparamref name="T"/> using
		/// converter method on each item.
		/// </summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="converter">The converter.</param>
		/// <returns>Stream of type <typeparamref name="T"/></returns>
		[Obsolete("Superseded by Linq")]
		public static IEnumerable<T> Convert<S, T>(IEnumerable<S> collection, Converter<S, T> converter)
		{
			foreach (var item in collection)
			{
				yield return converter(item);
			}
		}

		#endregion

		#region are same

		/// <summary>
		///  Are all of the items in the enumeration the same type. 
		/// </summary>
		/// <typeparam name="T">The common type of the enumeration</typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="evaluator">The evaluator.</param>
		/// <returns>True if all of the items are the same.</returns>
		public static bool AreSame<T, V>(IEnumerable<T> collection, Converter<T, V> evaluator)
		{
			var firstValue = default(V);
			bool first = true;

			foreach (var item in collection)
			{
				var thisValue = evaluator(item);

				if (first)
				{
					firstValue = thisValue;
					first = false;
				}
				else if (!object.Equals(firstValue, thisValue))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Are all of the items in the enumeration the same type. 
		/// </summary>
		/// <typeparam name="T">The common type of the enumeration</typeparam>
		/// <param name="source">The source enumeration</param>
		/// <returns>True if all of the items are of the same type.</returns>
		public static bool AreSameType<T>(IEnumerable<T> source)
		{
			return AreSame<T, Type>(source, o => o == null ? null : o.GetType());
		}

		/// <summary>
		/// If all items are of the same type, it returns this type, else returns null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns>The common type for all items in the enumerations, else null (if all are different)</returns>
		public static Type GetSameType<T>(IEnumerable<T> source)
		{
			Type commonType = null;

			foreach (var item in source)
			{
				if (commonType == null)
				{
					commonType = item.GetType();
				}
				else if (commonType != item.GetType())
				{
					commonType = null;
					break;
				}
			}

			return commonType;
		}

		/// <summary>
		/// Are all of the items in the enumeration the specific type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="type">Type of the test.</param>
		/// <returns>True if all of the items in the enumeration are of type <paramref name="type"/></returns>
		public static bool AreAllOfType<T>(IEnumerable<T> source, Type type)
		{
			return source.All(o => o.GetType() == type);
		}

		#endregion

		#region reverse

		/// <summary>
		/// Creates reversed object translator.
		/// </summary>
		/// <typeparam name="S">Source type (becomes target).</typeparam>
		/// <typeparam name="T">Target type (becomes source).</typeparam>
		/// <param name="translator">The translator.</param>
		/// <returns>Object translator.</returns>
		public static IObjectTranslator<T, S> Reverse<S, T>(IObjectTranslator<S, T> translator)
		{
			return new ReversedTranslator<T, S>(translator);
		}

		/// <summary>
		/// Reverses the order of specified collection.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Reversed IEnumerable.</returns>
		public static IEnumerable<T> Reverse<T>(IEnumerable<T> collection)
		{
			var stack = new Stack<T>();
			foreach (var item in collection) stack.Push(item);

			while (stack.Count > 0)
			{
				yield return stack.Pop();
			}
		}

		/// <summary>
		/// Reverses the specified list.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="list">The list.</param>
		/// <returns>Reversed IEnumerable.</returns>
		public static IEnumerable<T> Reverse<T>(IList<T> list)
		{
			int count = list.Count;
			for (int i = count - 1;i >= 0;i--)
			{
				yield return list[i];
			}
		}

		/// <summary>
		/// Reverses the specified dictionary.
		/// </summary>
		/// <typeparam name="K">Key (becomes Value)</typeparam>
		/// <typeparam name="V">Value (becomes Key)</typeparam>
		/// <param name="map">The dictionary.</param>
		/// <returns>Reversed dictionary.</returns>
		public static IDictionary<V, ICollection<K>> Reverse<K, V>(IDictionary<K, V> map)
		{
			IDictionary<V, ICollection<K>> result = new Dictionary<V, ICollection<K>>();

			foreach (var kv in map)
			{
				ICollection<K> list;
				if (!result.TryGetValue(kv.Value, out list))
				{
					list = new List<K>();
					result[kv.Value] = list;
				}
				list.Add(kv.Key);
			}

			return result;
		}

		#endregion

		#region concatenate

		/// <summary>
		/// Concatenates the specified list of lists.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="listOfLists">The list of lists.</param>
		/// <returns>One list.</returns>
		public static IEnumerable<T> Concatenate<T>(IEnumerable<IEnumerable<T>> listOfLists)
		{
			foreach (var listOfItems in listOfLists)
			{
				foreach (var item in listOfItems)
				{
					yield return item;
				}
			}
		}

		/// <summary>
		/// Concatenates the specified list of lists.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="listOfLists">The list of lists.</param>
		/// <returns>One list.</returns>
		public static IEnumerable<T> Concatenate<T>(params IEnumerable<T>[] listOfLists)
		{
			return Concatenate<T>((IEnumerable<IEnumerable<T>>)listOfLists);
		}

		/// <summary>
		/// Concatenates items extracted from collection of containers.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="C">Type of container.</typeparam>
		/// <param name="containers">The containers.</param>
		/// <param name="extractFromContainer">Method with extracts from containers.</param>
		/// <example><code>
		/// // to get all files in specified folders
		/// IEnumerable&lt;DirectoryInfo&gt; folders = ...; // given list of folders...
		/// IEnumerable&lt;FileInfo&gt; files = Transforms.Concatenate&lt;FileInfo, DirectoryInfo&gt;(
		///     // list of 'containers' (folders)
		///     folders, 
		///     // how to extract 'items' (files) from 'containers'
		///     delegate (DirectoryInfo info) { return info.GetFiles(); }); 
		/// </code></example>
		/// <returns></returns>
		public static IEnumerable<T> Concatenate<T, C>(IEnumerable<C> containers, Func<C, IEnumerable<T>> extractFromContainer)
		{
			return Concatenate<T>(containers.Select(extractFromContainer));
		}

		#endregion

		#region Map

		/// <summary>
		/// Creates lists. Utility for Map.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns></returns>
		private static ICollection<T> ListFactory<T>() { return new List<T>(); }

		/// <summary>
		/// Splits specified items into multiple lists depending on result of <paramref name="keyBuilder"/>.
		/// </summary>
		/// <typeparam name="K">Key.</typeparam>
		/// <typeparam name="V">Type of item.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="keyBuilder">The key builder. Assigns a key to every item.</param>
		/// <param name="collectionFactory">The collection factory. Can be null.</param>
		/// <returns>Dictionary of collections.</returns>
		public static IDictionary<K, ICollection<V>> Map<K, V>(
				IEnumerable<V> items,
				Converter<V, K> keyBuilder,
				Func<ICollection<V>> collectionFactory)
		{
			if (collectionFactory == null) collectionFactory = ListFactory<V>;

			Dictionary<K, ICollection<V>> result = new Dictionary<K, ICollection<V>>();

			foreach (V item in items)
			{
				K key = keyBuilder(item);
				ICollection<V> collection;

				if (!result.TryGetValue(key, out collection))
				{
					collection = collectionFactory();
					result[key] = collection;
				}

				collection.Add(item);
			}

			return result;
		}

		#endregion

		#region Enumerate

		/// <summary>
		/// Enumerates the specified args. It is just a syntactic trick. Some functions don't allow
		/// params T[] but allow IEnumerable&lt;T&gt;. It's equivalent to new T[] { ... }.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="args">The args.</param>
		/// <returns>Enumetation of given params array.</returns>
		public static IEnumerable<T> Enumerate<T>(params T[] args)
		{
			return args;
		}

		/// <summary>
		/// Enumerates the other enumerable aynchronously buffering results.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="input">The input enumerable.</param>
		/// <returns>Enumerable.</returns>
		public static IEnumerable<T> EnumerateAsync<T>(IEnumerable<T> input)
		{
			return EnumerateAsync<T>(input, int.MaxValue);
		}

		/// <summary>
		/// Enumerates the other enumerable aynchronously buffering results.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="input">The input enumerable.</param>
		/// <param name="maximumSize">The maximum size of queue.</param>
		/// <returns>Enumerable.</returns>
		public static IEnumerable<T> EnumerateAsync<T>(IEnumerable<T> input, int maximumSize)
		{
			return new AsyncEnumerable<T>(input, maximumSize);
		}

		#endregion

		#region Range

		/// <summary>
		/// Generates all numbers from given range.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="stop">The stop.</param>
		/// <param name="step">The step (and direction).</param>
		/// <returns>Stream of numbers.</returns>
		public static IEnumerable<int> Range(int start, int stop, int step)
		{
			int direction = Math.Sign(step);
			int i = start;

			while (((direction > 0) && (i < stop)) || ((direction < 0) && (i > stop)))
			{
				yield return i;
				i += step;
			}
		}

		#endregion

		#region Move

		/// <summary>
		/// Moves the item at <c>indexFrom</c> in a list to the <c>indexTo</c> position.
		/// </summary>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <param name="list">The list.</param>
		/// <param name="indexFrom">The from index</param>
		/// <param name="indexTo">The to index</param>
		public static void Move<T>(IList<T> list, int indexFrom, int indexTo)
		{
			if (indexFrom == indexTo)
				return;

			// NOTE, There are no range checks, if the indexFrom is illegal the list will complain			
			lock (list)
			{
				// Allow any value > list.Count to add the item at the end of the list
				if (indexTo > list.Count)
					indexTo = list.Count;

				// Allow any value < 0 to add the item at the start of the list
				if (indexTo < 0)
					indexTo = 0;

				// Remove and store item...
				T item = list[indexFrom];
				list.RemoveAt(indexFrom);

				// Re-add item, handling the case that all items could've been shunted down by 1 place
				if (indexFrom < indexTo)
					list.Insert(indexTo - 1, item);
				else
					list.Insert(indexTo, item);
			}
		}

		#endregion

		#region Merge

		/// <summary>
		/// Merges multiple dictionaries into one. Latter dictionaries take precedence.
		/// NOTE, it does create new object.
		/// </summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="maps">The collection of dictionaries.</param>
		/// <returns>New merged dictionary.</returns>
		public static Dictionary<K, V> Merge<K, V>(IEnumerable<IDictionary<K, V>> maps)
		{
			Dictionary<K, V> result = new Dictionary<K, V>();
			MergeInto(result, maps);
			return result;
		}

		/// <summary>
		/// Merges multiple dictionaries into one. Latter dictionaries take precedence.
		/// NOTE, it does create new object.
		/// </summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="maps">The collection of dictionaries.</param>
		/// <returns>New merged dictionary.</returns>
		public static Dictionary<K, V> Merge<K, V>(params IDictionary<K, V>[] maps)
		{
			return Merge((IEnumerable<IDictionary<K, V>>)maps);
		}

		/// <summary>
		/// Merges multiple dictionaries into given one. Latter dictionaries take precedence.
		/// NOTE, it does NOT create new object. All values are inserted into <paramref name="target"/>.
		/// </summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="target">The target.</param>
		/// <param name="maps">The collection of dictionaries.</param>
		/// <returns>Target dictionary.</returns>
		public static IDictionary<K, V> MergeInto<K, V>(IDictionary<K, V> target, IEnumerable<IDictionary<K, V>> maps)
		{
			foreach (IDictionary<K, V> map in maps)
			{
				foreach (KeyValuePair<K, V> kv in map)
				{
					target[kv.Key] = kv.Value;
				}
			}
			return target;
		}

		/// <summary>
		/// Merges multiple dictionaries into given one. Latter dictionaries take precedence.
		/// NOTE, it does NOT create new object. All values are inserted into <paramref name="target"/>.
		/// </summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="target">The target.</param>
		/// <param name="maps">The collection of dictionaries.</param>
		/// <returns>Target dictionary.</returns>
		public static IDictionary<K, V> MergeInto<K, V>(IDictionary<K, V> target, params IDictionary<K, V>[] maps)
		{
			return MergeInto(target, (IEnumerable<IDictionary<K, V>>)maps);
		}

		#endregion

		#region Split

		/// <summary>
		/// Splits specified items into two collections.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="falseCollection">The collection for items not matching given filter.</param>
		/// <param name="trueCollection">The collection for items matching given filter.</param>
		/// <param name="filter">The filter.</param>
		public static void Split<T>(
				IEnumerable<T> items,
				ICollection<T> falseCollection, ICollection<T> trueCollection,
				Predicate<T> filter)
		{
			foreach (T item in items)
			{
				if (filter(item))
				{
					trueCollection.Add(item);
				}
				else
				{
					falseCollection.Add(item);
				}
			}
		}

		/// <summary>
		/// Splits specified items into multiple lists depending on result of <paramref name="keyBuilder"/>.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="C">Type of collection.</typeparam>
		/// <typeparam name="K">Key.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="keyBuilder">The key builder. Assigns a key to every item.</param>
		/// <returns>Dictionary of collections.</returns>
		public static IDictionary<K, C> Split<T, C, K>(
			IEnumerable<T> items, Converter<T, K> keyBuilder)
			where C:ICollection<T>
		{
			Dictionary<K, C> result = new Dictionary<K, C>();

			foreach (T item in items)
			{
				K key = keyBuilder(item);
				C collection;

				if (!result.TryGetValue(key, out collection))
				{
					collection = Activator.CreateInstance<C>();
					result[key] = collection;
				}

				collection.Add(item);
			}

			return result;
		}

		#endregion

		#region SplitBy

		/// <summary>
		/// Splits the list to several lists with maximum of count elements per list.
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="maximumNumberOfItems">The maximum number of items per list.</param>
		/// <returns>List of lists.</returns>
		public static IEnumerable<IList<T>> SplitBy<T>(IEnumerable<T> collection, int maximumNumberOfItems)
		{
			int length = 0;
			IList<T> list = null;

			foreach (T subject in collection)
			{
				if (list == null)
				{
					// have to create new one! previous one can be used
					list = new List<T>();
				}

				list.Add(subject);
				length++;

				if (length >= maximumNumberOfItems)
				{
					yield return list;
					list = null;
					length = 0;
				}
			}

			if (list != null)
			{
				yield return list;
			}
		}

		#endregion

		#region Monitor

		/// <summary>
		/// Creates a monitored version of specified list.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="list">The list.</param>
		/// <param name="eventHandler">The event handler.</param>
		/// <returns>List.</returns>
		public static IList<T> Monitor<T>(IList<T> list, MonitoredListEvent<T> eventHandler)
		{
			MonitoredList<T> result = new MonitoredList<T>(list);
			result.Notification += eventHandler;
			return result;
		}

		/// <summary>
		/// Creates a monitored version of specified collection.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="eventHandler">The event handler.</param>
		/// <returns>collection.</returns>
		public static ICollection<T> Monitor<T>(ICollection<T> collection, EventHandler<MonitoredCollectionEventArgs<T>> eventHandler)
		{
			MonitoredCollection<T> result = new MonitoredCollection<T>(collection);
			result.Notification += eventHandler;
			return result;
		}

		#endregion

		#region ReadOnly

		/// <summary>
		/// Creates read-only proxy of existing collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="other">The collection to be encapsulated.</param>
		/// <returns>Read-only proxy collection.</returns>
		public static ICollection<T> ReadOnly<T>(ICollection<T> other)
		{
			return new ReadOnlyCollection<T>(other);
		}

		/// <summary>
		/// Creates read-only proxy of existing collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="other">The collection to be encapsulated.</param>
		/// <returns>Read-only proxy collection.</returns>
		public static IList<T> ReadOnly<T>(IList<T> other)
		{
			return new ReadOnlyList<T>(other);
		}

		/// <summary>
		/// Creates read-only proxy of existing collection.
		/// </summary>
		/// <typeparam name="K">Any type (dictionary key).</typeparam>
		/// <typeparam name="V">Any type (dictionary value).</typeparam>
		/// <param name="other">The collection to be encapsulated.</param>
		/// <returns>Read-only proxy collection.</returns>
		public static IDictionary<K, V> ReadOnly<K, V>(IDictionary<K, V> other)
		{
			return new ReadOnlyDictionary<K, V>(other);
		}

		/// <summary>
		/// Enforces collection to be read-write. If collection is read-only it creates a read-write copy.
		/// Note: Of course, this read-write version may be a copy so changes to it won't be saved to original list.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Read-write version of given collection.</returns>
		public static ICollection<T> ReadWrite<T>(ICollection<T> collection)
		{
			return collection.IsReadOnly ? Clone(collection) : collection;
		}

		/// <summary>
		/// Clones the specified collection. Note: it does nt use collection's Clone method.
		/// It creates new ICollection object. If you need exeact clone use ICloneable interface.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Collection containing same items as given one.</returns>
		private static ICollection<T> Clone<T>(ICollection<T> collection)
		{
			return new List<T>(collection);
		}

		/// <summary>
		/// Enforces list to be read-write. If list is read-only it creates a read-write copy.
		/// Note: Of course, this read-write version may be a copy so changes to it won't be saved to original list.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="list">The list.</param>
		/// <returns>Read-write version of given collection.</returns>
		public static IList<T> ReadWrite<T>(IList<T> list)
		{
			return list.IsReadOnly ? Clone(list) : list;
		}

		/// <summary>
		/// Clones the specified list. Note: it does not use list's Clone method.
		/// It creates new IList object. If you need exeact clone use ICloneable interface.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="list">The list.</param>
		/// <returns>List containing same items as given one.</returns>
		private static IList<T> Clone<T>(IList<T> list)
		{
			return new List<T>(list);
		}

		/// <summary>
		/// Enforces dictionary to be read-write. If dictionary is read-only it creates a read-write copy.
		/// Note: Of course, this read-write version may be a copy so changes to it won't be saved to original dictionary.
		/// </summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <returns>Read-write version of given dictionary.</returns>
		public static IDictionary<K, V> ReadWrite<K, V>(IDictionary<K, V> dictionary)
		{
			return dictionary.IsReadOnly ? Clone(dictionary) : dictionary;
		}

		/// <summary>
		/// Clones the specified dictionary. Note: it does not use dictionary's Clone method.
		/// It creates new IDictionary object. If you need exeact clone use ICloneable interface.
		/// </summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <returns>Collection containing same items as given one.</returns>
		private static IDictionary<K, V> Clone<K, V>(IDictionary<K, V> dictionary)
		{
			return new Dictionary<K, V>(dictionary);
		}

		#endregion

		#region Filter, Find, Count, FindFirst

		/// <summary>
		/// Predicate which always returns true.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c></returns>
		public static bool AlwaysTrue<T>(T value)
		{
			return true;
		}

		/// <summary>
		/// Predicate which always returns false.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns><c>false</c></returns>
		public static bool AlwaysFalse<T>(T value)
		{
			return false;
		}

		/// <summary>
		/// Filters the specified collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection to filter.</param>
		/// <param name="expression">The criteria.</param>
		/// <returns>Filtered collection.</returns>
		public static IEnumerable<T> FindAll<T>(IEnumerable<T> collection, Predicate<T> expression)
		{
			foreach (T item in collection)
			{
				if (expression(item)) yield return item;
			}
		}

		/// <summary>
		/// Finds the first item in collection. 
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>
		/// First item found or <c>default(T)</c> if not found.
		/// </returns>
		public static T FindFirst<T>(IEnumerable<T> collection)
		{
			return FindFirst<T>(collection, AlwaysTrue<T>, default(T));
		}

		/// <summary>
		/// Finds the first item in collection mathing give criteria.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="expression">The criteria.</param>
		/// <returns>
		/// First item found or <c>default(T)</c> if not found.
		/// </returns>
		public static T FindFirst<T>(IEnumerable<T> collection, Predicate<T> expression)
		{
			return FindFirst<T>(collection, expression, default(T));
		}

		/// <summary>
		/// Finds the first item in collection matching give criteria.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="expression">The criteria.</param>
		/// <param name="sentinel">The sentinel.</param>
		/// <returns>
		/// First item found or <paramref name="sentinel"/> if not found.
		/// </returns>
		public static T FindFirst<T>(IEnumerable<T> collection, Predicate<T> expression, T sentinel)
		{
			foreach (T item in collection)
			{
				if (expression(item)) return item;
			}
			return sentinel;
		}

		/// <summary>
		/// Finds the index of the first item matching criteria.
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="expression">The criteria.</param>
		/// <returns>Index of first item matching criteria, or -1 if none found</returns>
		public static int FindFirstIndex<T>(IList<T> collection, Predicate<T> expression)
		{
			int count = collection.Count;
			for (int i = 0;i < count;i++)
			{
				if (expression(collection[i])) return i;
			}
			return -1;
		}

		/// <summary>
		/// Finds the index of the first item matching criteria.
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="expression">The criteria.</param>
		/// <returns>Index of first item matching criteria, or -1 if none found</returns>
		public static int FindFirstIndex<T>(T[] collection, Predicate<T> expression)
		{
			int count = collection.Length;
			for (int i = 0;i < count;i++)
			{
				if (expression(collection[i])) return i;
			}
			return -1;
		}

		/// <summary>
		/// Counts items in the specified collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="maximum">The maximum. Use <c>int.MaxValue</c> if you need to count all items.</param>
		/// <returns>Number of items in collection.</returns>
		public static int Count<T>(IEnumerable<T> collection, int maximum)
		{
			return Count(collection, null, maximum);
		}

		/// <summary>
		/// Counts items in the specified collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Number of items in collection.</returns>
		public static int Count<T>(IEnumerable<T> collection)
		{
			return Count(collection, int.MaxValue);
		}

		/// <summary>
		/// Counts items in the specified collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Number of items in collection.</returns>
		public static int Count<T>(ICollection<T> collection)
		{
			return collection.Count;
		}

		/// <summary>
		/// Counts items in the specified collection. May stop after certain amount (sometime you are interested it there 
		/// is 0, 1 or more items, not the exact number).
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="maximumCount">The maximum value.</param>
		/// <returns>Number of items in collection.</returns>
		public static int Count<T>(ICollection<T> collection, int maximumCount)
		{
			return Math.Min(collection.Count, maximumCount);
		}

		/// <summary>
		/// Counts items in the specified collection matching specified criteria.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="expression">The criteria.</param>
		/// <param name="maximumCount">The maximum. Use <c>int.MaxValue</c> if you need to count all items.</param>
		/// <returns>Number of items in collection matching given criteria.</returns>
		public static int Count<T>(IEnumerable<T> collection, Predicate<T> expression, int maximumCount)
		{
			int result = 0;
			foreach (T item in collection)
			{
				if (result >= maximumCount) return result;
				if (expression == null || expression(item)) result++;
			}
			return result;
		}

		/// <summary>
		/// Determines whether the specified collection is empty.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns><c>true</c> if the specified collection is empty; otherwise, <c>false</c>.</returns>
		public static bool IsEmpty<T>(IEnumerable<T> collection)
		{
			return Count(collection, 1) <= 0;
		}

		/// <summary>Determines whether the specified collection is empty.</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns><c>true</c> if the specified collection is empty; otherwise, <c>false</c>.</returns>
		public static bool IsEmpty<T>(ICollection<T> collection)
		{
			return Count(collection, 1) <= 0;
		}

		/// <summary>Tests if any item matches given criteria.</summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="expression">The expression.</param>
		/// <returns><c>true</c> if any item matches given criteria; <c>false</c> otherwise.</returns>
		public static bool TestAny<T>(IEnumerable<T> collection, Predicate<T> expression)
		{
			return Count<T>(collection, expression, 1) > 0;
		}

		/// <summary>
		/// Tests if all items match given criteria.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="expression">The criteria.</param>
		/// <returns><c>true</c> if all items match given criteria; <c>false</c> otherwise.</returns>
		public static bool TestAll<T>(IEnumerable<T> collection, Predicate<T> expression)
		{
			return !TestAny(collection, item => !expression(item));
		}

		/// <summary>
		/// Tests if collection contains unique items.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns><c>true</c> if all item are unique; <c>false</c> otherwise.</returns>
		public static bool TestUnique<T>(IEnumerable<T> collection)
		{
			return TestUnique<T, T>(collection, i => i);
		}

		/// <summary>
		/// Tests if collection contains unique items. Uses on-the-fly <paramref name="identity"/> 
		/// function if object does not identify iself.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <typeparam name="K">Identity type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="identity">The identity.</param>
		/// <returns>
		/// 	<c>true</c> if all item are unique; <c>false</c> otherwise.
		/// </returns>
		public static bool TestUnique<T, K>(IEnumerable<T> collection, Converter<T, K> identity)
		{
			HashSet<K> set = new HashSet<K>();
			foreach (T item in collection)
			{
				K key = identity(item);
				if (set.Contains(key))
				{
					return false;
				}
				set.Add(key);
			}
			return true;
		}

		#endregion

		#region not null collections

		/// <summary>
		/// Sentinel method. Converts enumerable to EmptyCollection if enumerable is <c>null</c>.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="enumerable">The collection.</param>
		/// <returns>Empty IEnumerable.</returns>
		public static IEnumerable<T> NotNull<T>(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return EmptyCollection<T>.Default;
			}
			else
			{
				return enumerable;
			}
		}

		/// <summary>
		/// Sentinel method. Converts collection to EmptyCollection if collection is <c>null</c>.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Empty collection.</returns>
		public static ICollection<T> NotNull<T>(ICollection<T> collection)
		{
			if (collection == null)
			{
				return EmptyCollection<T>.Default;
			}
			else
			{
				return collection;
			}
		}

		/// <summary>
		/// Enforces list to be not-null. If given list is <c>null</c> returns empty list (empty, but it exists).
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="list">The list.</param>
		/// <returns>List of items or empty list if given list was <c>null</c></returns>
		public static IList<T> NotNull<T>(IList<T> list)
		{
			if (list == null)
			{
				return EmptyList<T>.Default;
			}
			else
			{
				return list;
			}
		}

		/// <summary>
		/// Creates collection with one item. It's a special collection which can have only one item.
		/// It has smaller memory footprint then "real" collection "accidently" containing only one item.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="item">The item.</param>
		/// <returns>Collection with one item in it.</returns>
		public static ICollection<T> OneItemCollection<T>(T item)
		{
			return new OneItemCollection<T>(item);
		}

		/// <summary>
		/// Creates dictionary with one item.
		/// </summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>Dictionary with one item.</returns>
		public static IDictionary<K, V> OneItemDictionary<K, V>(K key, V value)
		{
			return new OneItemDictionary<K, V>(key, value);
		}

		#endregion

		#region AddRange (AddMany) Support

		/// <summary>
		/// Adds multiple items to collection. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The list.</param>
		/// <param name="itemsToAdd">The items to add.</param>
		/// <returns>The IList with many items added</returns>
		public static ICollection<T> AddRange<T>(ICollection<T> collection, IEnumerable<T> itemsToAdd)
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			if (itemsToAdd == null)
				throw new ArgumentNullException("itemsToAdd", "itemsToAdd is null.");

			foreach (T item in itemsToAdd) collection.Add(item);

			return collection;
		}

		/// <summary>
		/// Adds multiple items to list. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="itemsToAdd">The items to add.</param>
		/// <returns>The IList with many items added</returns>
		public static IList<T> AddRange<T>(IList<T> list, IEnumerable<T> itemsToAdd)
		{
			if (list == null)
				throw new ArgumentNullException("list", "list is null.");
			if (itemsToAdd == null)
				throw new ArgumentNullException("itemsToAdd", "itemsToAdd is null.");

			foreach (T item in itemsToAdd) list.Add(item);

			return list;
		}

		#endregion

		#region reduce

		/// <summary>
		/// Reduces the multiple elements into one.
		/// </summary>
		/// <typeparam name="T">Type of element</typeparam>
		/// <typeparam name="R">Type of result</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="initializer">The initializer.</param>
		/// <param name="action">The action.</param>
		/// <returns>Value returned by last call to <paramref name="action"/> or <paramref name="initializer"/></returns>
		/// <example><code>
		/// IEnumerable&lt;int&gt; values = ...;
		/// int sum = Transforms.Reduce(values, 0, delegate (int e, int r) { return r + e; });
		/// int min = Transforms.Reduce(values, int.MaxValue, delegate (int e, int r) { return Math.Min(e, r); });
		/// </code></example>
		public static R Reduce<T, R>(IEnumerable<T> collection, R initializer, Func<T, R, R> action)
		{
			R result = initializer;
			foreach (T item in collection)
			{
				result = action(item, result);
			}
			return result;
		}

		/// <summary>
		/// Merges items in a list. Assuming you have a function (<paramref name="reductor"/>) which
		/// returns merged item or <c>null</c> when items cannot be merged.
		/// Example: You have collection of points and merge function which can merge points into rectangle
		/// if they are adjacent. Let's assume you have 4 points forming a square. After first pass you will
		/// get two rectangles, you need second pass to merge them into one big square.
		/// NOTE, it assumes <c>null</c> means "items cannot be merged" so null cannot be used as "real value".
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="multiPass">if set to <c>true</c> multiple passes are executed.</param>
		/// <param name="reductor">The reductor.</param>
		/// <returns></returns>
		public static List<T> MergeItems<T>(IEnumerable<T> collection, bool multiPass, Func<T, T, T> reductor)
		{
			List<T> items = new List<T>(collection);
			List<T> result;

			while (true)
			{
				result = new List<T>();

				bool mergeDone = false;

				while (items.Count > 1)
				{
					int lastIndex = items.Count - 1;

					T main = items[lastIndex];
					items.RemoveAt(lastIndex);

					for (int i = items.Count - 1;i >= 0;i--)
					{
						T reduced = reductor(main, items[i]);
						if (reduced != null)
						{
							mergeDone = true;
							main = reduced;
							items.RemoveAt(i);
						}
					}

					result.Add(main);
				}

				foreach (T item in items) result.Add(item);

				if (!multiPass || !mergeDone || result.Count <= 1) break;

				items = result;
			}

			return result;
		}

		#endregion

		#region min and max

		/// <summary>
		/// Finds minimal element in collection.
		/// </summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <typeparam name="V">Comparable type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="evaluator">The evaluator.</param>
		/// <returns>Minimal item.</returns>
		public static KeyValuePair<T, V> WithMin<T, V>(IEnumerable<T> collection, Converter<T, V> evaluator)
			where V:IComparable<V>
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			if (evaluator == null)
				throw new ArgumentNullException("evaluator", "evaluator is null.");

			T result = default(T);
			V minimum = default(V);
			bool first = true;

			foreach (T item in collection)
			{
				V value = evaluator(item);
				if (first || value.CompareTo(minimum) < 0)
				{
					result = item;
					minimum = value;
					first = false;
				}
			}

			if (first)
			{
				throw new ArgumentException("Given collection is empty");
			}

			return new KeyValuePair<T, V>(result, minimum);
		}

		/// <summary>
		/// Finds maximal element in collection.
		/// </summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <typeparam name="V">Comparable type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="evaluator">The evaluator.</param>
		/// <returns>Maximal item.</returns>
		public static KeyValuePair<T, V> WithMax<T, V>(IEnumerable<T> collection, Converter<T, V> evaluator)
			where V:IComparable<V>
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			if (evaluator == null)
				throw new ArgumentNullException("evaluator", "evaluator is null.");

			T result = default(T);
			V minimum = default(V);
			bool first = true;

			foreach (T item in collection)
			{
				V value = evaluator(item);
				if (first || value.CompareTo(minimum) > 0)
				{
					result = item;
					minimum = value;
					first = false;
				}
			}

			if (first)
			{
				throw new ArgumentException("Given collection is empty");
			}

			return new KeyValuePair<T, V>(result, minimum);
		}

		#endregion

		#region Repeat

		/// <summary>Repeats the item specifed number of times.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="count">The count.</param>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		public static IEnumerable<T> Repeat<T>(int count, T element)
		{
			while (count > 0)
			{
				yield return element;
				count--;
			}
		}

		/// <summary>Repeats the factory returning an enumerable of results.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="count">The count.</param>
		/// <param name="factory">The factory.</param>
		/// <returns>Enumerable of item.</returns>
		public static IEnumerable<T> RepeatFactory<T>(int count, Func<int, T> factory)
		{
			for (int i = 0;i < count;i++)
			{
				yield return factory(i);
			}
		}

		#endregion

		#region Zip

		/// <summary>
		/// Performs a zip action on each of the items in the IEnumerations
		/// </summary>
		/// <typeparam name="A">The type of the left enumeration.</typeparam>
		/// <typeparam name="B">The type of the right enumeration.</typeparam>
		/// <param name="left">The left IEnumerable.</param>
		/// <param name="right">The right IEnumerable.</param>
		/// <param name="action">The action to zip A and B together</param>
		public static void ZipAction<A, B>(IEnumerable<A> left, IEnumerable<B> right, Action<A, B> action)
		{
			var leftEnum = left.GetEnumerator();
			var rightEnum = right.GetEnumerator();

			while (leftEnum.MoveNext() && rightEnum.MoveNext())
			{
				action(leftEnum.Current, rightEnum.Current);
			}
		}

		/// <summary>
		/// Performs a zip action on each of the items in the IList
		/// </summary>
		/// <typeparam name="A">The type of the left list.</typeparam>
		/// <typeparam name="B">The type of the right list.</typeparam>
		/// <param name="left">The left IList.</param>
		/// <param name="right">The right IList.</param>
		/// <param name="action">The action to zip A and B together</param>
		public static void ZipAction<A, B>(IList<A> left, IEnumerable<B> right, Action<A, B> action)
		{
			ZipAction((IEnumerable<A>)left, (IEnumerable<B>)right, action);
		}

		#endregion

		#region obsolete

		// These are obsolete methods which has been replaced
		// they are not removed yet, but code using them should be upgraded as soon as possible

		/// <summary>
		/// Makes new collection containing only unique item from given one.
		/// Note: It uses Set internally, so uniqueness is decided using <see cref="object.GetHashCode()"/> 
		/// and <see cref="object.Equals(object)"/> methods.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Collection of unique values from original collection.</returns>
		[Obsolete("Superseded by Linq")]
		public static ICollection<T> Unique<T>(IEnumerable<T> collection)
		{
			return new HashSet<T>(collection);
		}

		/// <summary>
		/// Returns sorted version of given stream of objects.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>List of sorted items.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Sorted<T>(IEnumerable<T> collection) where T:IComparable<T>
		{
			return collection.Sorted();
		}

		/// <summary>
		/// Returns sorted version of given stream of objects.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>List of sorted items.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Sorted<T>(IEnumerable<T> collection, Comparison<T> comparer)
		{
			return collection.Sorted(comparer);
		}

		/// <summary>
		/// Returns sorted version of given stream of objects.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>List of sorted items.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Sorted<T>(IEnumerable<T> enumerable, IComparer<T> comparer)
		{
			return enumerable.Sorted(comparer);
		}

		/// <summary>
		/// Returns sorted version of given stream of objects.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="C"></typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="comparable">The comparable valuer factory.</param>
		/// <returns>List of sorted items.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Sorted<T, C>(IEnumerable<T> enumerable, Func<T, C> comparable) where C:IComparable<C>
		{
			return enumerable.Sorted(comparable);
		}

		/// <summary>
		/// Sorts the specified collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Same collection which has been passed.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Sort<T>(IList<T> collection) where T:IComparable<T>
		{
			var array = collection.ToArray();
			Array.Sort(array);
			for (int i = 0;i < array.Length;i++) collection[i] = array[i];
			return collection;
		}

		/// <summary>
		/// Sorts the specified collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>Same collection which has been passed.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Sort<T>(IList<T> collection, Comparison<T> comparer)
		{
			collection.SortInPlace(comparer);
			return collection;
		}

		/// <summary>
		/// Sorts the specified collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>Same collection which has been passed.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Sort<T>(IList<T> collection, IComparer<T> comparer)
		{
			collection.SortInPlace(comparer);
			return collection;
		}

		/// <summary>
		/// Sorts the specified collection.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <typeparam name="C"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="comparable">The comparable value factory.</param>
		/// <returns>Same collection which has been passed.</returns>
		[Obsolete("Superseded by Linq")]
		public static IList<T> Sort<T, C>(IList<T> collection, Func<T, C> comparable) where C:IComparable<C>
		{
			collection.SortInPlace(comparable);
			return collection;
		}

		/// <summary>
		/// Translates the specified untyped source IList into a typed IList
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="source">The source IList object</param>
		/// <returns>The typed IList</returns>
		[Obsolete("Superseded by Linq. Use source.TypedAs<T>() instead")]
		public static IList<T> Translate<T>(IList source)
		{
			return source.TypedAs<T>();
		}

		/// <summary>
		/// Translates the specified source untyped ICollection into a typed ICollection
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="source">The source ICollection object</param>
		/// <returns>The typed IList</returns>
		[Obsolete("Superseded by Linq. Use source.TypedAs<T>() instead")]
		public static ICollection<T> Translate<T>(ICollection source)
		{
			return source.TypedAs<T>();
		}

		/// <summary>
		/// Translates the specified source untyped IEnumerable into a typed IEnumerable
		/// </summary>
		/// <typeparam name="T">Target Type</typeparam>
		/// <param name="source">The source IEnumerable object</param>
		/// <returns>The typed IEnumable</returns>
		[Obsolete("Superseded by Linq")]
		public static IEnumerable<T> Translate<T>(IEnumerable source)
		{
			return source.TypedAs<T>();
		}

		/// <summary>
		/// Splits specified items into multiple lists depending on result of <paramref name="keyBuilder"/>.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="K">Key.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="collectionFactory">The collection factory. Can be null.</param>
		/// <param name="keyBuilder">The key builder. Assigns a key to every item.</param>
		/// <returns>Dictionary of lists.</returns>
		[Obsolete("Superseded by Linq. Use source.Map instead.")]
		public static IDictionary<K, ICollection<T>> Split<T, K>(
			IEnumerable<T> items,
			Func<ICollection<T>> collectionFactory,
			Converter<T, K> keyBuilder)
		{
			return Map<K, T>(items, keyBuilder, collectionFactory);
		}

		/// <summary>
		/// Converts IEnumerable to IList.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>List.</returns>
		[Obsolete("Superseded by Linq")]
		public static List<T> ToList<T>(IEnumerable<T> collection)
		{
			return new List<T>(collection);
		}

		/// <summary>
		/// Converts ICollection to IList.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>List.</returns>
		[Obsolete("Superseded by Linq")]
		public static List<T> ToList<T>(ICollection<T> collection)
		{
			List<T> result = new List<T>(collection.Count);
			result.AddRange(collection);
			return result;
		}

		/// <summary>
		/// Converts IEnumerable to Array.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Array.</returns>
		[Obsolete("Superseded by Linq")]
		public static T[] ToArray<T>(IEnumerable<T> collection)
		{
			return ToArray((ICollection<T>)ToList(collection));
		}

		/// <summary>
		/// Converts IEnumerable to Array.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Array.</returns>
		[Obsolete("Superseded by Linq")]
		public static T[] ToArray<T>(ICollection<T> collection)
		{
			T[] result = new T[collection.Count];
			collection.CopyTo(result, 0);
			return result;
		}

		/// <summary>
		/// Converts params to Array.
		/// This method does almost nothing, but because it guesses element 
		/// type (unlike new operator) it allows you to type less.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The params.</param>
		/// <returns>Array</returns>
		[Obsolete("Superseded by Linq")]
		public static T[] MakeArray<T>(params T[] collection)
		{
			return collection;
		}

		#endregion
	}
}
