using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Structures
{
    /// <summary>
    /// Splay tree Implementation of the Self Balancing Tree
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SplayTree<TKey, TValue> 
        : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        #region Members
        /// <summary>
        /// Root Node
        /// </summary>
        private SplayTreeNode root;

        /// <summary>
        /// Number of Items
        /// </summary>
        private int count;

        /// <summary>
        /// Version Number
        /// </summary>
        private int version = 0;
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            this.Set(key, value, throwOnExisting: true);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Set(item.Key, item.Value, throwOnExisting: true);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public void Clear()
        {
            this.root = null;
            this.count = 0;
            this.version++;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            if (this.count == 0)
                return false;

            this.Splay(key);

            return key.CompareTo(this.root.Key) == 0;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (this.count == 0)
                return false;

            this.Splay(item.Key);

            return item.Key.CompareTo(this.root.Key) == 0 &&
                   (object.ReferenceEquals(this.root.Value, item.Value) || (!object.ReferenceEquals(item.Value, null) && item.Value.Equals(this.root.Value)));
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public bool Remove(TKey key)
        {
            if (this.count == 0)
                return false;

            this.Splay(key);

            if (key.CompareTo(this.root.Key) != 0)
                return false;

            if (this.root.LeftChild == null)
            {
                this.root = this.root.RightChild;
            }
            else
            {
                var swap = this.root.RightChild;
                this.root = this.root.LeftChild;
                this.Splay(key);
                this.root.RightChild = swap;
            }

            this.version++;
            this.count--;

            return true;
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.count == 0)
            {
                value = default(TValue);
                return false;
            }

            this.Splay(key);

            if (key.CompareTo(this.root.Key) != 0)
            {
                value = default(TValue);
                return false;
            }

            value = this.root.Value;
            return true;
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (this.count == 0)
                return false;

            this.Splay(item.Key);

            if (item.Key.CompareTo(this.root.Key) == 0 && 
                (object.ReferenceEquals(this.root.Value, item.Value) || (!object.ReferenceEquals(item.Value, null) && item.Value.Equals(this.root.Value))))
                return false;

            if (this.root.LeftChild == null)
            {
                this.root = this.root.RightChild;
            }
            else
            {
                var swap = this.root.RightChild;
                this.root = this.root.LeftChild;
                this.Splay(item.Key);
                this.root.RightChild = swap;
            }

            this.version++;
            this.count--;
            return true;
        }

        /// <summary>
        /// Trims the specified depth.
        /// </summary>
        /// <param name="depth">The depth.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void Trim(int depth)
        {
            if (depth < 0)
                throw new ArgumentOutOfRangeException("depth", "The trim depth must not be negative.");

            if (this.count == 0)
                return;

            if (depth == 0)
            {
                this.Clear();
            }
            else
            {
                var prevCount = this.count;
                this.count = this.Trim(this.root, depth - 1);
                if (prevCount != this.count)
                    this.version++;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.AsList(node => new KeyValuePair<TKey, TValue>(node.Key, node.Value)).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new TiedList<KeyValuePair<TKey, TValue>>(this, this.version, this.AsList(node => new KeyValuePair<TKey, TValue>(node.Key, node.Value))).GetEnumerator();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        public int Count
        {
            get 
            { 
                return this.count; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get 
            { 
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// The key was not found in the tree.
        /// or
        /// The key was not found in the tree.
        /// </exception>
        public TValue this[TKey key]
        {
            get
            {
                if (this.count == 0)
                    throw new KeyNotFoundException("The key was not found in the tree.");

                this.Splay(key);

                if (key.CompareTo(this.root.Key) != 0)
                    throw new KeyNotFoundException("The key was not found in the tree.");

                return this.root.Value;
            }      
            set
            {
                this.Set(key, value, throwOnExisting: false);
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<TKey> Keys
        {
            get 
            { 
                return new TiedList<TKey>(this, this.version, this.AsList(node => node.Key));
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<TValue> Values
        {
            get
            { 
                return new TiedList<TValue>(this, this.version, this.AsList(node => node.Value)); 
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="throwOnExisting">if set to <c>true</c> [throw on existing].</param>
        /// <exception cref="System.ArgumentException"></exception>
        private void Set(TKey key, TValue value, bool throwOnExisting)
        {
            if (this.count == 0)
            {
                this.version++;
                this.root = new SplayTreeNode(key, value);
                this.count = 1;
                return;
            }

            this.Splay(key);

            var c = key.CompareTo(this.root.Key);
            if (c == 0)
            {
                if (throwOnExisting)
                    throw new ArgumentException("An item with the same key already exists in the tree.");

                this.version++;
                this.root.Value = value;
                return;
            }

            var n = new SplayTreeNode(key, value);
            if (c < 0)
            {
                n.LeftChild = this.root.LeftChild;
                n.RightChild = this.root;
                this.root.LeftChild = null;
            }
            else
            {
                n.RightChild = this.root.RightChild;
                n.LeftChild = this.root;
                this.root.RightChild = null;
            }

            this.root = n;
            this.count++;
            this.Splay(key);
            this.version++;
        }

        /// <summary>
        /// Splays the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        private void Splay(TKey key)
        {
            SplayTreeNode l;
            SplayTreeNode r;
            SplayTreeNode t;
            SplayTreeNode y;
            SplayTreeNode header;

            l = r = header = new SplayTreeNode(default(TKey), default(TValue));
            t = this.root;

            while (true)
            {
                var c = key.CompareTo(t.Key);
                if (c < 0)
                {
                    if (t.LeftChild == null) 
                        break;

                    if (key.CompareTo(t.LeftChild.Key) < 0)
                    {
                        y = t.LeftChild;
                        t.LeftChild = y.RightChild;
                        y.RightChild = t;
                        t = y;

                        if (t.LeftChild == null) 
                            break;
                    }

                    r.LeftChild = t;
                    r = t;
                    t = t.LeftChild;
                }
                else if (c > 0)
                {
                    if (t.RightChild == null) 
                        break;

                    if (key.CompareTo(t.RightChild.Key) > 0)
                    {
                        y = t.RightChild;
                        t.RightChild = y.LeftChild;
                        y.LeftChild = t;
                        t = y;

                        if (t.RightChild == null) 
                            break;
                    }

                    l.RightChild = t;
                    l = t;
                    t = t.RightChild;
                }
                else
                {
                    break;
                }
            }

            l.RightChild = t.LeftChild;
            r.LeftChild = t.RightChild;
            t.LeftChild = header.RightChild;
            t.RightChild = header.LeftChild;

            this.root = t;
        }

        /// <summary>
        /// Trims the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        private int Trim(SplayTreeNode node, int depth)
        {
            if (depth == 0)
            {
                node.LeftChild = null;
                node.RightChild = null;
                return 1;
            }
            else
            {
                int count = 1;

                if (node.LeftChild != null)
                    count += Trim(node.LeftChild, depth - 1);

                if (node.RightChild != null)
                    count += Trim(node.RightChild, depth - 1);

                return count;
            }
        }

        /// <summary>
        /// Ases the list.
        /// </summary>
        /// <typeparam name="TEnumerator">The type of the enumerator.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        private IList<TEnumerator> AsList<TEnumerator>(Func<SplayTreeNode, TEnumerator> selector)
        {
            if (this.root == null)
                return new TEnumerator[0];

            var result = new List<TEnumerator>(this.count);
            PopulateList(this.root, result, selector);

            return result;
        }

        /// <summary>
        /// Populates the list.
        /// </summary>
        /// <typeparam name="TEnumerator">The type of the enumerator.</typeparam>
        /// <param name="node">The node.</param>
        /// <param name="list">The list.</param>
        /// <param name="selector">The selector.</param>
        private void PopulateList<TEnumerator>(SplayTreeNode node, List<TEnumerator> list, Func<SplayTreeNode, TEnumerator> selector)
        {
            if (node.LeftChild != null) 
                PopulateList(node.LeftChild, list, selector);

            list.Add(selector(node));

            if (node.RightChild != null) 
                PopulateList(node.RightChild, list, selector);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Node Implementation
        private sealed class SplayTreeNode
        {
            #region Members
            /// <summary>
            /// Key
            /// </summary>
            public readonly TKey Key;

            /// <summary>
            /// T Value
            /// </summary>
            public TValue Value;

            /// <summary>
            /// Left Node
            /// </summary>
            public SplayTreeNode LeftChild;

            /// <summary>
            /// Right Node
            /// </summary>
            public SplayTreeNode RightChild;
            #endregion

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the splay tree class.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public SplayTreeNode(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }
            #endregion
        }
        #endregion

        #region Tied To List Implementation
        [DebuggerDisplay("Count = {Count}")]
        private sealed class TiedList<T> 
            : IList<T>
        {
            #region Members
            /// <summary>
            /// Tree
            /// </summary>
            private readonly SplayTree<TKey, TValue> tree;

            /// <summary>
            /// Tree Node Version
            /// </summary>
            private readonly int version;

            /// <summary>
            /// Backing List
            /// </summary>
            private readonly IList<T> backingList;
            #endregion

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the Splay tree class.
            /// </summary>
            /// <param name="tree">The tree.</param>
            /// <param name="version">The version.</param>
            /// <param name="backingList">The backing list.</param>
            /// <exception cref="System.ArgumentNullException">
            /// tree
            /// or
            /// backingList
            /// </exception>
            public TiedList(SplayTree<TKey, TValue> tree, int version, IList<T> backingList)
            {
                if (tree == null)
                    throw new ArgumentNullException("tree");

                if (backingList == null)
                    throw new ArgumentNullException("backingList");

                this.tree = tree;
                this.version = version;
                this.backingList = backingList;
            }
            #endregion

            #region Public Methods
            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
            /// <returns>
            /// The index of item if found in the list; otherwise, -1.
            /// </returns>
            /// <exception cref="System.InvalidOperationException"></exception>
            public int IndexOf(T item)
            {
                if (this.tree.version != this.version) 
                    throw new InvalidOperationException("The collection has been modified.");

                return this.backingList.IndexOf(item);
            }

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which item should be inserted.</param>
            /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
            /// <exception cref="System.NotSupportedException"></exception>
            public void Insert(int index, T item)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the item to remove.</param>
            /// <exception cref="System.NotSupportedException"></exception>
            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
            /// <exception cref="System.NotSupportedException"></exception>
            public void Add(T item)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <exception cref="System.NotSupportedException"></exception>
            public void Clear()
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
            /// <returns>
            /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
            /// </returns>
            /// <exception cref="System.InvalidOperationException"></exception>
            public bool Contains(T item)
            {
                if (this.tree.version != this.version) 
                    throw new InvalidOperationException("The collection has been modified.");

                return this.backingList.Contains(item);
            }

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
            /// <exception cref="System.InvalidOperationException"></exception>
            public void CopyTo(T[] array, int arrayIndex)
            {
                if (this.tree.version != this.version)
                    throw new InvalidOperationException("The collection has been modified.");

                this.backingList.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
            /// <returns>
            /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </returns>
            /// <exception cref="System.NotSupportedException"></exception>
            public bool Remove(T item)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            /// </returns>
            /// <exception cref="System.InvalidOperationException"></exception>
            public IEnumerator<T> GetEnumerator()
            {
                if (this.tree.version != this.version) 
                    throw new InvalidOperationException("The collection has been modified.");

                foreach (var item in this.backingList)
                {
                    yield return item;

                    if (this.tree.version != this.version)
                        throw new InvalidOperationException("The collection has been modified.");
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets the item at the specified index.
            /// </summary>
            /// <value>
            /// </value>
            /// <param name="index">The index.</param>
            /// <returns></returns>
            /// <exception cref="System.InvalidOperationException"></exception>
            /// <exception cref="System.NotSupportedException"></exception>
            public T this[int index]
            {
                get
                {
                    if (tree.version != version) 
                        throw new InvalidOperationException("The collection has been modified.");

                    return backingList[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
            public int Count
            {
                get 
                { 
                    return tree.count; 
                }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
            /// </summary>
            /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
            public bool IsReadOnly
            {
                get 
                { 
                    return true;
                }
            }
            #endregion
        }
        #endregion
    }
}