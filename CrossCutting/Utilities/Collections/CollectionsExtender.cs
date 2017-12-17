using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Collections
{
    /// <summary>
    /// Class dedicated to extend .NET collections with some useful general purpose methods.
    /// </summary>
    public static class CollectionsExtender
    {
        #region SafeSyncRoot (experimental)

        private static readonly object m_GlobalSyncRoot = new object();

        /// <summary>Gets SyncRoot of the object.</summary>
        /// <param name="syncObject">The sync object.</param>
        /// <returns>SyncRoot.</returns>
        public static object SafeSyncRoot(this object syncObject)
        {
            if (syncObject == null)
            {
                return m_GlobalSyncRoot;
            }

            var root = syncObject as ISyncRoot;

            if (root == null)
            {
                return syncObject;
            }
            else
            {
                return root.SyncRoot;
            }
        }

        #endregion

        #region Async

        /// <summary>Creates asynchronous enumerable.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="queueSize">Size of the buffer queue.</param>
        /// <returns>Enumerable.</returns>
        public static AsyncEnumerable<T> AsyncEnumerable<T>(this IEnumerable<T> enumerable, int queueSize = int.MaxValue)
        {
            return new AsyncEnumerable<T>(enumerable, queueSize);
        }

        /// <summary>Creates async enumerator.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerator">The enumerator.</param>
        /// <param name="queueSize">Size of the buffer queue.</param>
        /// <returns>Enumerator.</returns>
        public static AsyncEnumerator<T> AsyncEnumerator<T>(this IEnumerator<T> enumerator, int queueSize = int.MaxValue)
        {
            return new AsyncEnumerator<T>(enumerator, queueSize);
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Executes same <paramref name="action"/> for all item in collection.
        /// NOTE: this method should be made obsolete as it conflicts with System.Interactive.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The collection.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// Split the Enumeration of T into a enumeration of enumeration of T, the size of the enum will be bucketSize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The e.</param>
        /// <param name="bucketSize">Size of the bucket.</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> e, int bucketSize)
        {
            return e.Where((a, i) => i % bucketSize == 0).Select((a, i) => e.Skip(bucketSize * i).Take(bucketSize));
        }

        /// <summary>
        /// Executes same <paramref name="action"/> for all item in collection providing ordering number.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="action">The action.</param>
        /// <returns>First not used index (last used + 1). Note, for empty enumerable it will be startIndex, 
        /// for enumerable with 1 element startIndex + 1.</returns>
        public static int IndexedForEach<T>(this IEnumerable<T> enumerable, int startIndex, Action<T, int> action)
        {
            foreach (var item in enumerable)
            {
                action(item, startIndex);
                startIndex++;
            }
            return startIndex;
        }

        /// <summary>Does the specified collection.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action.</param>
        /// <returns>Same collection.</returns>
        public static IEnumerable<T> TransparentForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>Indexes the specified enumerable.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>Enumerable of pairs consisting of index and value.</returns>
        public static IEnumerable<KeyValuePair<int, T>> Indexed<T>(this IEnumerable<T> enumerable, int startIndex = 0)
        {
            foreach (var item in enumerable)
            {
                yield return new KeyValuePair<int, T>(startIndex, item);
                startIndex++;
            }
        }

        /// <summary>Indexes the specified enumerable, reseting counter every time group changes.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="restart">The restart detector. Should return <c>true</c> if groups needs to be changed
        /// and <c>false</c> it item belongs to same group.</param>
        /// <param name="startIndex">The start index. Resets every time group changes.</param>
        /// <returns>Enumerable of pairs consisting of index and value.</returns>
        public static IEnumerable<KeyValuePair<int, T>> Indexed<T>(
            this IEnumerable<T> enumerable, Func<T, T, bool> restart, int startIndex = 0)
        {
            bool first = true;
            var last = default(T);
            int index = startIndex;

            foreach (var item in enumerable)
            {
                if (first)
                {
                    first = false;
                    // do not check restart(...) for first item
                }
                else if (restart(last, item))
                {
                    index = startIndex;
                }

                yield return new KeyValuePair<int, T>(index, item);
                last = item;
                index++;
            }
        }

        #endregion

        #region Reduce

        /// <summary>Reduces the specified collection into one item.</summary>
        /// <typeparam name="T">Type of items in collection.</typeparam>
        /// <typeparam name="R">Type of result.</typeparam>
        /// <param name="enumerable">The collection.</param>
        /// <param name="initial">The initial result value.</param>
        /// <param name="reductor">The reductor method.</param>
        /// <returns>Single value.</returns>
        public static R Reduce<T, R>(this IEnumerable<T> enumerable, R initial, Func<R, T, R> reductor)
        {
            var result = initial;

            foreach (var item in enumerable)
            {
                result = reductor(result, item);
            }

            return result;
        }

        #endregion

        #region Partition

        /// <summary>Partitions the specified collection.</summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <returns>Elements partitioned by key values.</returns>
        public static IDictionary<K, List<T>> Partition<T, K>(this IEnumerable<T> collection, Func<T, K> keyFactory)
        {
            return Partition(collection, keyFactory, () => new List<T>());
        }

        /// <summary>Partitions the specified collection.</summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>Elements partitioned by key values.</returns>
        public static IDictionary<K, List<V>> Partition<T, K, V>(
            this IEnumerable<T> collection,
            Func<T, K> keyFactory, Func<T, V> valueFactory)
        {
            return Partition(collection, keyFactory, valueFactory, () => new List<V>());
        }

        /// <summary>Partitions the specified collection.</summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="L">Type of list.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <param name="listFactory">The list factory.</param>
        /// <returns>Elements partitioned by key values.</returns>
        public static IDictionary<K, L> Partition<T, K, L>(
            this IEnumerable<T> collection,
            Func<T, K> keyFactory,
            Func<L> listFactory)
            where L : ICollection<T>
        {
            return Partition(collection, keyFactory, Patterns.Pass<T>, listFactory);
        }

        /// <summary>Partitions the specified collection.</summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <typeparam name="K">Type of keys.</typeparam>
        /// <typeparam name="V">Type of values.</typeparam>
        /// <typeparam name="L">Type of list.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <param name="listFactory">The list factory.</param>
        /// <returns>Elements partitioned by key values.</returns>
        public static IDictionary<K, L> Partition<T, K, V, L>(
            this IEnumerable<T> collection,
            Func<T, K> keyFactory,
            Func<T, V> valueFactory,
            Func<L> listFactory)
            where L : ICollection<V>
        {
            var result = new Dictionary<K, L>();

            foreach (var item in collection)
            {
                var key = keyFactory(item);
                var value = valueFactory(item);
                L list;

                if (!result.TryGetValue(key, out list))
                {
                    result[key] = list = listFactory();
                }

                list.Add(value);
            }

            return result;
        }

        /// <summary>Partitions the specified sequence into batches.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>Sequence of batches.</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(
            this IEnumerable<T> sequence, int batchSize)
        {
            if (batchSize <= 0) batchSize = 1;
            List<T> buffer = null;

            foreach (var item in sequence)
            {
                if (buffer != null && buffer.Count >= batchSize)
                {
                    yield return buffer;
                    buffer = null;
                }

                if (buffer == null)
                    buffer = new List<T>(batchSize);

                buffer.Add(item);
            }

            if (buffer != null && buffer.Count > 0)
                yield return buffer;
        }

        #endregion

        #region Count, IsEmpty

        /// <summary>Counts items in the specified collection matching specified criteria.</summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="enumerable">The collection.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="maximumCount">The maximum count.</param>
        /// <returns>Number of items in collection or <paramref name="maximumCount"/> matching given criteria.</returns>
        public static int Count<T>(this IEnumerable<T> enumerable, Predicate<T> criteria, int maximumCount = int.MaxValue)
        {
            int result = 0;

            foreach (var item in enumerable)
            {
                if (result >= maximumCount) break;
                if (criteria == null || criteria(item)) result++;
            }

            return result;
        }

        /// <summary>Counts items in the specified collection.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="enumerable">The collection.</param>
        /// <param name="maximumCount">The maximum count.</param>
        /// <returns>Number of items in collection or <paramref name="maximumCount"/>.</returns>
        public static int Count<T>(this IEnumerable<T> enumerable, int maximumCount)
        {
            return enumerable.Count(null, maximumCount);
        }

        /// <summary>Counts items in the specified collection. This implementation is provided to be consistent with 
        /// <see cref="Count{T}(IEnumerable{T},int)"/> only. <see cref="ICollection{T}"/> has 
        /// <see cref="ICollection{T}.Count"/> property anyway.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="maximumCount">The maximum count.</param>
        /// <returns>Number of items in collection or <paramref name="maximumCount"/>.</returns>
        public static int Count<T>(this ICollection<T> collection, int maximumCount)
        {
            return Math.Min(collection.Count, maximumCount);
        }

        /// <summary>Determines whether the specified enumerable is empty.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns><c>true</c> if the specified subject is empty; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count(1) <= 0;
        }

        /// <summary>Determines whether the specified collection is empty.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns><c>true</c> if the specified collection is empty; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            // it look identical to IEnumerable<T> version, but it uses different 
            // Count(...) method so it is quicker
            return collection.Count(1) <= 0;
        }

        #endregion

        #region AddMany

        /// <summary>Adds multiple items to collection.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="itemsToAdd">The items to add.</param>
        /// <returns>The same collection.</returns>
        public static TCollection AddMany<TCollection, TItem>(this TCollection collection, IEnumerable<TItem> itemsToAdd)
            where TCollection : ICollection<TItem>
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (itemsToAdd == null)
            {
                throw new ArgumentNullException("itemsToAdd");
            }

            foreach (var item in itemsToAdd)
            {
                collection.Add(item);
            }

            return collection;
        }

        /// <summary>Adds multiple items to collection.</summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="itemsToAdd">The following items to add.</param>
        /// <returns>The same collection.</returns>
        public static TCollection AddMany<TCollection, TItem>(this TCollection collection, TItem first, TItem second, params TItem[] itemsToAdd)
            where TCollection : ICollection<TItem>
        {
            collection.Add(first);
            collection.Add(second);
            return AddMany(collection, (IEnumerable<TItem>)itemsToAdd);
        }

        #endregion

        #region Untyped

        /// <summary>Creates untyped pass-through <see cref="IEnumerator"/>.</summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="enumerator">The enumerator.</param>
        /// <returns><see cref="IEnumerator"/>.</returns>
        public static IEnumerator Untyped<T>(this IEnumerator<T> enumerator)
        {
            return enumerator;
        }

        /// <summary>Creates untyped pass-through <see cref="IEnumerable"/>.</summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns><see cref="IEnumerable"/>.</returns>
        public static IEnumerable Untyped<T>(this IEnumerable<T> enumerable)
        {
            return enumerable;
        }

        /// <summary>Creates untyped pass-through <see cref="ICollection"/>.</summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns><see cref="ICollection"/></returns>
        public static ICollection Untyped<T>(this ICollection<T> collection)
        {
            if (collection == null) return null;
            var result = collection as ICollection;

            if (result == null)
            {
                result = new UntypedCollection<T>(collection);
            }

            return result;
        }

        /// <summary>Creates untyped pass-through <see cref="IList"/></summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns><see cref="IList"/></returns>
        public static IList Untyped<T>(this IList<T> list)
        {
            if (list == null) return null;
            var result = list as IList;

            if (result == null)
            {
                result = new UntypedList<T>(list);
            }

            return result;
        }

        /// <summary>
        /// Creates untyped pass-through <see cref="IDictionary"/>
        /// </summary>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of values.</typeparam>
        /// <param name="map">The dictionary.</param>
        /// <returns><see cref="IDictionary"/></returns>
        public static IDictionary Untyped<K, V>(this IDictionary<K, V> map)
        {
            if (map == null) return null;
            var result = map as IDictionary;

            if (result == null)
            {
                result = new UntypedDictionary<K, V>(map);
            }

            return result;
        }

        #endregion

        #region TypedAs

        /// <summary>Converts <see cref="IEnumerator"/> to typed <see cref="IEnumerator{T}"/>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="enumerator">The enumerator.</param>
        /// <returns>Typed enumerator.</returns>
        public static IEnumerator<T> TypedAs<T>(this IEnumerator enumerator)
        {
            if (enumerator == null) return null;
            var result = enumerator as IEnumerator<T>;

            if (result == null)
            {
                result = new TypedAsEnumerator<T>(enumerator);
            }

            return result;
        }

        /// <summary>Converts <see cref="IEnumerable"/> to typed <see cref="IEnumerable{T}"/>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>Typed enumerable.</returns>
        public static IEnumerable<T> TypedAs<T>(this IEnumerable enumerable)
        {
            if (enumerable == null) return null;
            var result = enumerable as IEnumerable<T>;

            if (result == null)
            {
                result = new TypedAsEnumerable<T>(enumerable);
            }

            return result;
        }

        /// <summary>Converts <see cref="ICollection"/> to typed <see cref="ICollection{T}"/>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>Typed collection.</returns>
        public static ICollection<T> TypedAs<T>(this ICollection collection)
        {
            if (collection == null) return null;
            var result = collection as ICollection<T>;

            if (result == null)
            {
                result = new TypedAsCollection<T>(collection);
            }

            return result;
        }

        /// <summary>Converts <see cref="IList"/> to typed <see cref="IList{T}"/>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Typed list.</returns>
        public static IList<T> TypedAs<T>(this IList list)
        {
            if (list == null) return null;
            var result = list as IList<T>;

            if (result == null)
            {
                result = new TypedAsList<T>(list);
            }

            return result;
        }

        /// <summary>Converts <see cref="IDictionary"/> to typed <see cref="IDictionary{K,V}"/>.</summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <typeparam name="V">Value type.</typeparam>
        /// <param name="map">The dictionary.</param>
        /// <returns>Typed dictionary.</returns>
        public static IDictionary<K, V> TypedAs<K, V>(this IDictionary map)
        {
            if (map == null) return null;
            var result = map as IDictionary<K, V>;

            if (result == null)
            {
                result = new TypedAsDictionary<K, V>(map);
            }

            return result;
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
        public static IDictionary<K, TV> Translate<K, SV, TV>(this IDictionary<K, SV> source, IObjectTranslator<SV, TV> translator)
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
        /// <param name="sourceToTarget">The source to target converter.</param>
        /// <param name="targetToSource">The target to source converter.</param>
        /// <returns>Dictionary.</returns>
        public static IDictionary<K, TV> Translate<K, SV, TV>(
            this IDictionary<K, SV> source,
            Func<SV, TV> sourceToTarget, Func<TV, SV> targetToSource)
        {
            return new TranslatedDictionary<K, SV, TV>(source, CallbackObjectTranslator.Make(sourceToTarget, targetToSource));
        }

        /// <summary>
        /// Dynamically translates one dictionary into another.
        /// </summary>
        /// <typeparam name="K">Key.</typeparam>
        /// <typeparam name="SV">Source value.</typeparam>
        /// <typeparam name="TV">Target value.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Dictionary</returns>
        public static IDictionary<K, TV> Translate<K, SV, TV>(this IDictionary<K, SV> source)
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
        public static IList<T> Translate<S, T>(this IList<S> source, IObjectTranslator<S, T> translator)
        {
            return new TranslatedList<S, T>(source, translator);
        }

        /// <summary>
        /// Dynamically translates one list into another.
        /// </summary>
        /// <typeparam name="S">Source type.</typeparam>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="sourceToTarget">The source to target converter.</param>
        /// <param name="targetToSource">The target to source converter.</param>
        /// <returns>List.</returns>
        public static IList<T> Translate<S, T>(
            this IList<S> source,
            Func<S, T> sourceToTarget, Func<T, S> targetToSource)
        {
            return new TranslatedList<S, T>(source, CallbackObjectTranslator.Make(sourceToTarget, targetToSource));
        }

        /// <summary>
        /// Dynamically translates one list into another.
        /// </summary>
        /// <typeparam name="S">Source type.</typeparam>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>List.</returns>
        public static IList<T> Translate<S, T>(this IList<S> source)
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
        public static ICollection<T> Translate<S, T>(this ICollection<S> source, IObjectTranslator<S, T> translator)
        {
            return new TranslatedCollection<S, T>(source, translator);
        }

        /// <summary>
        /// Dynamically translates one collection into another.
        /// </summary>
        /// <typeparam name="S">Source type.</typeparam>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="sourceToTarget">The source to target converter.</param>
        /// <param name="targetToSource">The target to source converter.</param>
        /// <returns>Collection.</returns>
        public static ICollection<T> Translate<S, T>(
            this ICollection<S> source, Func<S, T> sourceToTarget, Func<T, S> targetToSource)
        {
            return new TranslatedCollection<S, T>(source, CallbackObjectTranslator.Make(sourceToTarget, targetToSource));
        }

        /// <summary>
        /// Dynamically translates one collection into another.
        /// </summary>
        /// <typeparam name="S">Source type.</typeparam>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Collection.</returns>
        public static ICollection<T> Translate<S, T>(this ICollection<S> source)
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
        public static IEnumerable<T> Translate<S, T>(this IEnumerable<S> source, IObjectTranslator<S, T> translator)
        {
            return new TranslatedEnumerable<S, T>(source, translator);
        }

        /// <summary>
        /// Dynamically translates one IEnumerable into another.
        /// </summary>
        /// <typeparam name="S">Source type.</typeparam>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="sourceToTarget">The source to target converter.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<T> Translate<S, T>(this IEnumerable<S> source, Func<S, T> sourceToTarget)
        {
            return new TranslatedEnumerable<S, T>(source, CallbackObjectTranslator.Make(sourceToTarget, null));
        }

        /// <summary>
        /// Dynamically translates one IEnumerable into another.
        /// </summary>
        /// <typeparam name="S">Source type.</typeparam>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<T> Translate<S, T>(this IEnumerable<S> source)
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
        public static IEnumerator<T> Translate<S, T>(this IEnumerator<S> source, IObjectTranslator<S, T> translator)
        {
            return new TranslatedEnumerator<S, T>(source, translator);
        }

        /// <summary>
        /// Dynamically translates one IEnumerator into another.
        /// </summary>
        /// <typeparam name="S">Source type.</typeparam>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="sourceToTarget">The source to target converter.</param>
        /// <returns>IEnumerator.</returns>
        public static IEnumerator<T> Translate<S, T>(this IEnumerator<S> source, Func<S, T> sourceToTarget)
        {
            return new TranslatedEnumerator<S, T>(source, CallbackObjectTranslator.Make(sourceToTarget, null));
        }

        /// <summary>
        /// Dynamically translates one IEnumerator into another.
        /// </summary>
        /// <typeparam name="S">Source type.</typeparam>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>IEnumerator.</returns>
        public static IEnumerator<T> Translate<S, T>(this IEnumerator<S> source)
        {
            return new TranslatedEnumerator<S, T>(source);
        }

        #endregion

        #region NotNull (collections)

        /// <summary>
        /// Strips the nulls from a specific sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>The sequence without the nulls</returns>
        public static IEnumerable<T> StripNulls<T>(this IEnumerable<T> sequence)
        {
            foreach (T item in sequence)
                if (item != null)
                    yield return item;
        }

        /// <summary>
        /// Strips the nulls from a specific sequence.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>The sequence without the nulls</returns>
        [Obsolete("Use StripNulls(...) instead")]
        public static IEnumerable<T> RemoveNulls<T>(this IEnumerable<T> sequence)
        {
            return StripNulls(sequence);
        }

        /// <summary>Returns given array or dummy empty one if given one is <c>null</c>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns>Not-null array.</returns>
        public static T[] NotNull<T>(this T[] array)
        {
            return array ?? EmptyArray<T>.Instance;
        }

        /// <summary>Returns given <see cref="IEnumerator&lt;T&gt;"/> or dummy empty <see cref="EmptyEnumerator&lt;T&gt;"/> 
        /// if given one is <c>null</c>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="enumerator">The enumerator.</param>
        /// <returns>Not-null <see cref="IEnumerator&lt;T&gt;"/></returns>
        public static IEnumerator<T> NotNull<T>(this IEnumerator<T> enumerator)
        {
            return enumerator ?? EmptyEnumerator<T>.Default;
        }

        /// <summary>Returns given <see cref="IEnumerable&lt;T&gt;"/> or dummy <see cref="EmptyEnumerable&lt;T&gt;"/> 
        /// if given one is <c>null</c>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>Not-null <see cref="IEnumerable&lt;T&gt;"/></returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? EmptyEnumerable<T>.Default;
        }

        /// <summary>Returns given <see cref="ICollection&lt;T&gt;"/> or dummy empty <see cref="EmptyCollection&lt;T&gt;"/> 
        /// if given one is <c>null</c>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>Not-null <see cref="ICollection&lt;T&gt;"/></returns>
        public static ICollection<T> NotNull<T>(this ICollection<T> collection)
        {
            return collection ?? EmptyCollection<T>.Default;
        }

        /// <summary>Returns given <see cref="IList&lt;T&gt;"/> or dummy empty <see cref="EmptyList&lt;T&gt;"/> 
        /// if given one is <c>null</c>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Not-null <see cref="IList&lt;T&gt;"/></returns>
        public static IList<T> NotNull<T>(this IList<T> list)
        {
            return list ?? EmptyList<T>.Default;
        }

        /// <summary>Returns given <see cref="IDictionary&lt;K,V&gt;"/> or dummy empty <see cref="EmptyDictionary&lt;K,V&gt;"/> 
        /// if given one is <c>null</c>.</summary>
        /// <typeparam name="K">The type of the key.</typeparam>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <param name="map">The dictionary.</param>
        /// <returns>Not-null <see cref="IDictionary&lt;K,V&gt;"/></returns>
        public static IDictionary<K, V> NotNull<K, V>(this IDictionary<K, V> map)
        {
            return map ?? EmptyDictionary<K, V>.Default;
        }

        /// <summary>Returns given <see cref="ISet&lt;T&gt;"/> or dummy empty <see cref="EmptySet&lt;T&gt;"/> 
        /// if given one is <c>null</c>.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="set">The set.</param>
        /// <returns>Not-null <see cref="ISet&lt;T&gt;"/></returns>
        public static ISet<T> NotNull<T>(this ISet<T> set)
        {
            return set ?? EmptySet<T>.Default;
        }

        #endregion

        #region ReadOnly

        /// <summary>Returns read-only pass-through envelope for given collection.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>Read-only envelope for given collection.</returns>
        public static ReadOnlyCollection<T> ReadOnly<T>(this ICollection<T> collection)
        {
            return new ReadOnlyCollection<T>(collection);
        }

        /// <summary>Returns read-only pass-through envelope for given list.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Read-only envelope for given list.</returns>
        public static ReadOnlyList<T> ReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyList<T>(list);
        }

        /// <summary>Returns read-only pass-through envelope for given dictionary.</summary>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>Read-only envelope for given dictionary.</returns>
        public static ReadOnlyDictionary<K, V> ReadOnly<K, V>(this IDictionary<K, V> dictionary)
        {
            return new ReadOnlyDictionary<K, V>(dictionary);
        }

        /// <summary>Returns read-only pass-through envelope for given set.</summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="set">The set.</param>
        /// <returns>Read-only pass-through envelope for given set</returns>
        public static ReadOnlySet<T> ReadOnly<T>(this ISet<T> set)
        {
            return new ReadOnlySet<T>(set);
        }

        /// <summary>Makes the dictionary writable. If it's not it creates a copy, if it is writable it just passes it.</summary>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>A given dictionary or it's clone. If given dictionary was <c>null</c> the result is <c>null</c> as well.</returns>
        public static IDictionary<K, V> Writable<K, V>(this IDictionary<K, V> dictionary)
        {
            return
                dictionary == null ? null :
                dictionary.IsReadOnly ? new Dictionary<K, V>(dictionary) :
                dictionary;
        }

        #endregion

        #region Monitor

        /// <summary>Returns monitored pass-through collection for the specified one.</summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="handler">The handler.</param>
        /// <returns>Monitored collection.</returns>
        public static MonitoredCollection<T> Monitor<T>(
            this ICollection<T> collection,
            EventHandler<MonitoredCollectionEventArgs<T>> handler)
        {
            if (collection == null) return null;
            return new MonitoredCollection<T>(collection, handler);
        }

        #endregion

        #region WithMin, WithMax

        /// <summary>
        /// Finds minimal element in collection.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <typeparam name="V">Comparable type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns>Minimal item.</returns>
        public static KeyValuePair<T, V> WithMin<T, V>(this IEnumerable<T> collection, Converter<T, V> evaluator)
            where V : IComparable<V>
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (evaluator == null)
                throw new ArgumentNullException("evaluator");

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
        public static KeyValuePair<T, V> WithMax<T, V>(this IEnumerable<T> collection, Converter<T, V> evaluator)
            where V : IComparable<V>
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (evaluator == null)
                throw new ArgumentNullException("evaluator");

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

        #region EqualsToOther

        /// <summary>Tests if object is the equals to other using <see cref="IEqualsToOther{T}"/> interface.
        /// This method allows to remove all the boiler plate code from object (checking 'null', checking if
        /// they are 'equal by reference') so you can focus on important stuff.</summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="one">The object one.</param>
        /// <param name="two">The object two.</param>
        /// <returns>Result of comparison.</returns>
        public static bool ObjectEqualsToOther<T>(this T one, object two) where T : class, IEqualsToOther<T>
        {
            if (object.ReferenceEquals(one, two))
            {
                return true;
            }

            if (object.ReferenceEquals(two, null))
            {
                return false;
            }

            var other = two as T;

            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return one.EqualsToOther(other);
        }

        /// <summary>Tests if object is the equals to other using <see cref="IEqualsToOther{T}"/> interface.
        /// This method allows to remove all the boiler plate code from object (checking 'null', checking if
        /// they are 'equal by reference') so you can focus on important stuff.</summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="one">The object one.</param>
        /// <param name="two">The object two.</param>
        /// <returns>Result of comparison.</returns>
        public static bool ValueEqualsToOther<T>(this T one, object two) where T : struct, IEqualsToOther<T>
        {
            if (object.ReferenceEquals(two, null))
            {
                return false;
            }

            T other;

            try
            {
                other = (T)two;
            }
            catch (InvalidCastException)
            {
                return false;
            }

            return one.EqualsToOther(other);
        }

        /// <summary>Tests if object is the equals to other using <see cref="IEqualsToOther{T}"/> interface.
        /// This method allows to remove all the boiler plate code from object (checking 'null', checking if
        /// they are 'equal by reference') so you can focus on important stuff.</summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="one">The object one.</param>
        /// <param name="two">The object two.</param>
        /// <returns>Result of comparison.</returns>
        public static bool ObjectEqualsToOther<T>(this T one, T two) where T : class, IEqualsToOther<T>
        {

            if (object.ReferenceEquals(one, two))
            {
                return true;
            }

            if (object.ReferenceEquals(two, null))
            {
                return false;
            }

            return one.EqualsToOther(two);
        }

        /// <summary>Tests if object is the equals to other using <see cref="IEqualsToOther{T}"/> interface.
        /// This method allows to remove all the boiler plate code from object (checking 'null', checking if
        /// they are 'equal by reference') so you can focus on important stuff.</summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="one">The object one.</param>
        /// <param name="two">The object two.</param>
        /// <returns>Result of comparison.</returns>
        public static bool ValueEqualsToOther<T>(this T one, T two) where T : struct, IEqualsToOther<T>
        {
            return one.EqualsToOther(two);
        }

        #endregion

        #region ToSet, ToLinkedList, ToQueue

        /// <summary>Converts stream of items into set.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>Set of items.</returns>
        public static HashSet<T> ToSet<T>(this IEnumerable<T> items)
        {
            return new HashSet<T>(items);
        }

        /// <summary>Converts stream of items into sorted set.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>Sorted set of items.</returns>
        public static SortedSet<T> ToSortedSet<T>(this IEnumerable<T> items)
        {
            return new SortedSet<T>(items);
        }

        /// <summary>Converts stream of items into linked list.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>Linked list of items.</returns>
        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> items)
        {
            return new LinkedList<T>(items);
        }

        /// <summary>Converts collection of items into queue.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>Queue of items.</returns>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> items)
        {
            return new Queue<T>(items);
        }

        /// <summary>Builds dictionary using collection of key/value pairs.
        /// Note, it will throw exception if keys are not distinct.</summary>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of value.</typeparam>
        /// <param name="pairs">The pairs.</param>
        /// <returns>Dictionary.</returns>
        public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> pairs)
        {
            var result = new Dictionary<K, V>();
            foreach (var kv in pairs) result.Add(kv.Key, kv.Value);
            return result;
        }

        #endregion

        #region Concat

        /// <summary>Appends one item to enumerable.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="item">The item.</param>
        /// <param name="tail">if set to <c>true</c> item is appended on the end, it gets inserted at the begining otherwise.</param>
        /// <returns>New enumerable with item appended.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T item, bool tail = true)
        {
            if (!tail) yield return item;
            foreach (var i in enumerable) yield return i;
            if (tail) yield return item;
        }

        /// <summary>Concats multiple enumerations to form one enumeration consisting of all the items in given enumerables.</summary>
        /// <example><![CDATA[
        /// var a1 = new int[] { 1, 2, 3, 4 };
        /// var a2 = new int[] { 5, 6, 7, 8 };
        /// var ax = new [] { a1, a2, a1 };
        /// var r = ax.Flatten(); // 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4
        /// ]]></example>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerables">The enumerables.</param>
        /// <returns>Flattened list of all items.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> enumerables)
        {
            foreach (var enumerable in enumerables)
            {
                foreach (var item in enumerable)
                {
                    yield return item;
                }
            }
        }

        #endregion

        #region Sorted

        /// <summary>Returns sorted version the specified enumerable.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>Sorted version the specified enumerable</returns>
        public static IList<T> Sorted<T>(this IEnumerable<T> enumerable)
            where T : IComparable<T>
        {
            var buffer = enumerable.ToList();
            buffer.Sort();
            return buffer;
        }

        /// <summary>Returns sorted version the specified enumerable.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>Sorted version the specified enumerable</returns>
        public static IList<T> Sorted<T>(this IEnumerable<T> enumerable, Comparison<T> comparer)
        {
            var buffer = enumerable.ToList();
            buffer.Sort(comparer);
            return buffer;
        }

        /// <summary>Returns sorted version the specified enumerable.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>Sorted version the specified enumerable</returns>
        public static IList<T> Sorted<T>(this IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            var buffer = enumerable.ToList();
            buffer.Sort(comparer);
            return buffer;
        }

        /// <summary>Returns sorted version the specified enumerable.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparable">The comparable.</param>
        /// <returns>Sorted version the specified enumerable</returns>
        public static IList<T> Sorted<T, C>(this IEnumerable<T> enumerable, Func<T, C> comparable)
            where C : IComparable<C>
        {
            var buffer = enumerable.ToList();
            buffer.Sort((a, b) => comparable(a).CompareTo(comparable(b)));
            return buffer;
        }

        /// <summary>Sorts the specified array.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <typeparam name="C">Type of comparable part.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="comparable">The comparable object factory.</param>
        public static void Sort<T, C>(this T[] array, Func<T, C> comparable) where C : IComparable<C>
        {
            Array.Sort(array, (a, b) => comparable(a).CompareTo(comparable(b)));
        }

        /// <summary>Sorts the specified list.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <typeparam name="C">Type of comparable part.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="comparable">The comparable object factory.</param>
        public static void Sort<T, C>(this List<T> list, Func<T, C> comparable) where C : IComparable<C>
        {
            list.Sort((a, b) => comparable(a).CompareTo(comparable(b)));
        }

        /// <summary>Sorts the IList in place.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The collection.</param>
        public static void SortInPlace<T>(this IList<T> collection)
        {
            var array = collection.ToArray();
            Array.Sort(array);
            for (int i = 0; i < array.Length; i++) collection[i] = array[i];
        }

        /// <summary>Sorts the IList in place.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="comparer">The comparer.</param>
        public static void SortInPlace<T>(this IList<T> collection, Comparison<T> comparer)
        {
            var array = collection.ToArray();
            Array.Sort(array, comparer);
            for (int i = 0; i < array.Length; i++) collection[i] = array[i];
        }

        /// <summary>Sorts the IList in place.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="comparer">The comparer.</param>
        public static void SortInPlace<T>(this IList<T> collection, IComparer<T> comparer)
        {
            var array = collection.ToArray();
            Array.Sort(array, comparer);
            for (int i = 0; i < array.Length; i++) collection[i] = array[i];
        }

        /// <summary>Sorts the IList in place.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="comparable">The comparable compound of item.</param>
        public static void SortInPlace<T, C>(this IList<T> collection, Func<T, C> comparable) where C : IComparable<C>
        {
            var array = collection.ToArray();
            array.Sort(comparable);
            for (int i = 0; i < array.Length; i++) collection[i] = array[i];
        }

        #endregion

        #region TryGetValueOrCreate

        /// <summary>
        /// Tries to get value from dictionary. 
        /// If it cannot be found creates one and (optionally) puts it back to dictionary.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <typeparam name="V">Value type.</typeparam>
        /// <param name="map">The map.</param>
        /// <param name="key">The key.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="add">if set to <c>true</c> value if added back to dictionary, 
        /// if <c>false</c> value is created, returned but not added.</param>
        /// <returns>Found or created value.</returns>
        public static V TryGetValueOrCreate<K, V>(this IDictionary<K, V> map, K key, Func<K, V> factory, bool add = true)
        {
            V result;

            if (object.ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key", "Key cannot be 'null'");
            }

            if (!map.TryGetValue(key, out result))
            {
                result = factory(key);
                if (add)
                {
                    map.Add(key, result);
                }
            }

            return result;
        }

        /// <summary>Tries to get value from dictionary. If it cannout be found returns default value.</summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <typeparam name="V">Value type.</typeparam>
        /// <param name="map">The map.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Found or default value.</returns>
        public static V TryGetValue<K, V>(this IDictionary<K, V> map, K key, V defaultValue = default(V))
        {
            V result;
            if (object.ReferenceEquals(key, null) || !map.TryGetValue(key, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        #endregion

        #region Map

        /// <summary>Maps the specified items.</summary>
        /// <typeparam name="DKV">The type of the dictionary.</typeparam>
        /// <typeparam name="O">Type of objects.</typeparam>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of value.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="mapFactory">The map factory.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <param name="emptyMapFactory">The empty map factory.</param>
        /// <returns></returns>
        private static DKV Map<DKV, O, K, V>(
            IEnumerable<O> items,
            Func<DKV> mapFactory,
            Func<O, K> keyFactory, Func<O, V> valueFactory,
            Func<DKV> emptyMapFactory = null)
            where DKV : class, IDictionary<K, V>
        {
            DKV result = null;

            foreach (var item in items)
            {
                if (result == null)
                {
                    result = mapFactory();
                }

                var key = keyFactory(item);
                var value = valueFactory(item);
                try
                {
                    result.Add(key, value);
                }
                catch (ArgumentNullException e)
                {
                    throw new ArgumentNullException(String.Format("Cannot map item '{0}' with null key", item), e);
                }
                catch (ArgumentException e)
                {
                    if (object.Equals(result[key], value))
                    {
                        // silence exception - duplicate but identical
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("Cannot map non unique item '{0}'", item), e);
                    }
                }
            }

            if (object.ReferenceEquals(result, null))
            {
                return (emptyMapFactory ?? mapFactory)(); // create smallest possible dictionary, maybe it will never grow?
            }
            else
            {
                return result;
            }
        }

        private static Dictionary<K, V> EmptyDictionaryFactory<K, V>() { return new Dictionary<K, V>(0); }

        /// <summary>
        /// Maps specified items. For every value in collection, generates a key and a value for it and adds to dictionary.
        /// </summary>
        /// <typeparam name="O">Type of original objects.</typeparam>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of value.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="keyBuilder">The key builder.</param>
        /// <param name="valueBuilder">The value builder.</param>
        /// <returns>Dictionary.</returns>
        public static Dictionary<K, V> Map<O, K, V>(
                this IEnumerable<O> items, Func<O, K> keyBuilder, Func<O, V> valueBuilder)
        {
            return Map(
                items,
                Patterns.New<Dictionary<K, V>>,
                keyBuilder,
                valueBuilder,
                EmptyDictionaryFactory<K, V>);
        }

        /// <summary>
        /// Maps specified items. For every value in collection, generates a key for it and adds to dictionary. 
        /// </summary>
        /// <typeparam name="K">Key.</typeparam>
        /// <typeparam name="V">Value.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <returns>Dictionary.</returns>
        public static Dictionary<K, V> Map<K, V>(this IEnumerable<V> items, Func<V, K> keyFactory)
        {
            return Map(items, keyFactory, Patterns.Pass<V>);
        }

        /// <summary>
        /// Maps specified items. For every value in collection, generates a key and a value for it and adds to dictionary.
        /// </summary>
        /// <typeparam name="O">Type of object.</typeparam>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of values.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>Sorted dictionary.</returns>
        public static SortedDictionary<K, V> SortedMap<O, K, V>(this IEnumerable<O> items, Func<O, K> keyFactory, Func<O, V> valueFactory)
        {
            return Map(
                items,
                Patterns.New<SortedDictionary<K, V>>,
                keyFactory,
                valueFactory,
                null);
        }

        /// <summary>
        /// Maps specified items. For every value in collection, generates a key and a value for it and adds to dictionary.
        /// </summary>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of values.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <returns>Sorted dictionary.</returns>
        public static SortedDictionary<K, V> SortedMap<K, V>(this IEnumerable<V> items, Func<V, K> keyFactory)
        {
            return SortedMap(items, keyFactory, Patterns.Pass<V>);
        }

        #endregion

        #region CopyTo

        /// <summary>Copies values from enumerable to array.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The enumerable.</param>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <param name="length">The length. Note, this function will not exeed the array length.</param>
        public static void CopyTo<T>(this IEnumerable<T> collection, T[] array, int index = 0, int length = int.MaxValue)
        {
            length = Math.Min(length, array.Length - index);
            foreach (var item in collection)
            {
                if (length <= 0) break;
                array[index] = item;
                index++;
                length--;
            }
        }

        #endregion

        #region Where

        /// <summary>Finds indices where condition is satisfied.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Enumeration of indices.</returns>
        public static IEnumerable<int> IndicesWhere<T>(this IList<T> collection, Predicate<T> filter)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (filter(collection[i])) yield return i;
            }
        }

        #endregion

        #region Set Intersection

        /// <summary>
        /// Provides an intersection between two sets, this doesn't modify either of the input sets
        /// but returns a new set with the intersected values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="comparer">The optional equality comparer.</param>
        /// <returns>A new set containing the intersected items/></returns>
        public static ISet<T> Intersection<T>(this ISet<T> left, ISet<T> right, IEqualityComparer<T> comparer = null)
        {
            // TODO:MAK why it is a SortedSet not a HashSet?
            return new SortedSet<T>(left.Intersect(right, comparer));
        }

        #endregion

        #region Unique

        /// <summary>Gets only unique items from collection.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>Collection of unique items.</returns>
        public static IEnumerable<T> Unique<T>(this IEnumerable<T> collection)
        {
            return collection.ToSet();
        }

        /// <summary>Gets items with unique key from collection. </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="keyFactory">The key factory.</param>
        /// <param name="useLast">if set to <c>true</c> used last occurrence of duplicated key, 
        /// otherwise uses the first one..</param>
        /// <returns>Collection of items with unique keys.</returns>
        public static IEnumerable<T> Unique<T, K>(this IEnumerable<T> collection, Func<T, K> keyFactory, bool useLast = false)
        {
            var keys = new Dictionary<K, T>();
            foreach (var item in collection)
            {
                var key = keyFactory(item);
                if (keys.ContainsKey(key) && !useLast) continue;
                keys[key] = item;
            }
            return keys.Values;
        }

        /// <summary>Determines whether the specified collection contains unique items.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns><c>true</c> if the specified collection is unique; otherwise, <c>false</c>.</returns>
        public static bool IsUnique<T>(this IEnumerable<T> collection)
        {
            var values = new HashSet<T>();

            foreach (var item in collection)
            {
                if (values.Contains(item)) return false;
                values.Add(item);
            }

            return true;
        }

        #endregion

        #region Synchronized

        /// <summary>Creates an IBindingList which fires events synchronized to target.</summary>
        /// <param name="source">The source IBindingList (source of events).</param>
        /// <param name="target">The target control (target of events).</param>
        /// <param name="async">if set to <c>true</c> events will be called asynchronously to <c>this</c> thread
        /// (fire and forget), <c>false</c> if you want to wait for return (nicer but, prone to deadlock).</param>
        /// <param name="useOriginalSender">if set to <c>true</c> passes original sender. It can be quite dangerous because 
        /// if you want to unregister from sender inside handler, it wont work (as you are not actually registered to original 
        /// sender).</param>
        /// <returns>Sunchronized IBIndingList.</returns>
        public static SynchronizedBindingList Synchronized(
            this IBindingList source,
            ISynchronizeInvoke target, bool async = false,
            bool useOriginalSender = false)
        {
            if (source == null) return null;
            return new SynchronizedBindingList(source, target, async, useOriginalSender);
        }

        /// <summary>Creates an IBindingList which fires events synchronized to target.</summary>
        /// <param name="source">The source IBindingList (source of events). Please note, this parameter does not have to
        /// be both <see cref="IList{T}"/>, but it will be much quicker if it is.</param>
        /// <param name="target">The target control (target of events).</param>
        /// <param name="async">if set to <c>true</c> events will be called asynchronously to <c>this</c> thread
        /// (fire and forget), <c>false</c> if you want to wait for return (nicer but, prone to deadlock).</param>
        /// <param name="useOriginalSender">if set to <c>true</c> passes original sender. It can be quite dangerous because 
        /// if you want to unregister from sender inside handler, it wont work (as you are not actually registered to original 
        /// sender).</param>
        /// <returns>Sunchronized IBIndingList.</returns>
        public static SynchronizedBindingList<T> Synchronized<T>(
            this IBindingList source,
            ISynchronizeInvoke target, bool async = false,
            bool useOriginalSender = false)
        {
            if (source == null) return null;
            return new SynchronizedBindingList<T>(source, target, async, useOriginalSender);
        }

        #endregion

        // IMP:MAK TranslatedTo
        // IMP:MAK OneItemCollection
        // IMP:MAK Concat IEnumerable
    }
}
