using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// A table of symbols - a kind of runtime Reflection but does not directly map to a class structure.
    /// The underlying dictionary is of type string/symbol but if you need to access the data as a simple dictionary of string/object, call 
    /// GetObjectDictionary which will return a IDictionary wrapper accessing this instance.
    /// 
    /// </summary>
    public class SymbolTable : 
        Dictionary<string, Symbol>
    {

        /// <summary>
        /// When set, this will reutrn the object instance that yielded the symbol table. Warning: There may be no owner for free standing symbol tables.
        /// </summary>
        public object Owner = null;


        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="symbolTable">The symbol table.</param>
        public void AddRange(SymbolTable symbolTable)
        {
            foreach(string key in symbolTable.Keys)
            {
                this.Add(key, symbolTable[key]);
            }
        }

        /// <summary>
        /// Adds the specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void Add(Symbol symbol)
        {
            this.Add(symbol.Name, symbol);
        }

        /// <summary>
        /// Get the symbols in this table by category name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Symbol>> GetSymbolsByCategory()
        {
            Dictionary<string, List<Symbol>> categories = new Dictionary<string, List<Symbol>>();
            foreach (Symbol symbol in this.Values)
            {
                if (!categories.ContainsKey(symbol.Category))
                {
                    categories.Add(symbol.Category,new List<Symbol>());
                }
                categories[symbol.Category].Add(symbol);
            }
            return categories;
        }

        /// <summary>
        /// Gets the object dictionary.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetObjectDictionary()
        {
            return new SymbolTableObjectDictionary(this);
        }

        public class SymbolTableObjectDictionary : IDictionary<string, object>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SymbolTableObjectDictionary"/> class.
            /// </summary>
            /// <param name="symbolTable">The symbol table.</param>
            public SymbolTableObjectDictionary(SymbolTable symbolTable)
            {
                this.symbolTable = symbolTable;
            }

            private SymbolTable symbolTable = null;


            /// <summary>
            /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
            /// </summary>
            /// <param name="key">The object to use as the key of the element to add.</param>
            /// <param name="value">The object to use as the value of the element to add.</param>
            /// <exception cref="System.NotImplementedException"></exception>
            public void Add(string key, object value)
            {
                throw new NotImplementedException();
            }

            public ICollection<string> Keys
            {
                get { return symbolTable.Keys; }
            }

            public bool TryGetValue(string key, out object value)
            {
                Symbol symbol = null;
                if (symbolTable.TryGetValue(key, out symbol))
                {
                    value = symbol.Get();
                    return true;
                }
                value = null;
                return false;
            }

            public ICollection<object> Values
            {
                get
                {
                    List<object> values = new List<object>();
                    foreach (Symbol symbol in symbolTable.Values)
                    {
                        values.Add(symbol.Get());
                    }
                    return values;
                }
            }

            public object this[string key]
            {
                get
                {
                    return symbolTable[key].Get();
                }
                set
                {
                    symbolTable[key].Set(value);
                }
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
            /// <exception cref="System.NotImplementedException"></exception>
            public void Add(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
            /// <returns>
            /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
            /// </returns>
            /// <exception cref="System.NotImplementedException"></exception>
            public bool Contains(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Copies to.
            /// </summary>
            /// <param name="array">The array.</param>
            /// <param name="arrayIndex">Index of the array.</param>
            /// <exception cref="System.NotImplementedException"></exception>
            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </summary>
            /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
            /// <returns>
            /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </returns>
            /// <exception cref="System.NotImplementedException"></exception>
            public bool Remove(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                return new SymbolTableObjectDictionaryEnumerator(this.symbolTable);
            }

            /// <summary>
            /// Enumerator translating the symbol table string/object settings into a KeyValuePair
            /// </summary>
            private class SymbolTableObjectDictionaryEnumerator : IEnumerator<KeyValuePair<string, object>>
            {
                public Indigo.CrossCutting.Utilities.SymbolTable symbolTable = null;

                int currentPosition = -1;

                /// <summary>
                /// Initializes a new instance of the <see cref="SymbolTableObjectDictionaryEnumerator"/> class.
                /// </summary>
                /// <param name="symbolTable">The symbol table.</param>
                public SymbolTableObjectDictionaryEnumerator(Indigo.CrossCutting.Utilities.SymbolTable symbolTable)
                {
                    this.symbolTable = symbolTable;
                }

                public KeyValuePair<string, object> Current
                {
                    get
                    {
                        string key = this.symbolTable.Keys.ElementAt(currentPosition);
                        return new KeyValuePair<string, object>(
                            key,
                            this.symbolTable[key].Get());
                    }
                }

                public void Dispose()
                {
                    // Nothing to do.
                }

                object System.Collections.IEnumerator.Current
                {
                    get 
                    {
                        string key = this.symbolTable.Keys.ElementAt(currentPosition); 
                        return this.symbolTable[key].Get();
                    }
                }

                public bool MoveNext()
                {
                    currentPosition++;
                    return currentPosition < this.symbolTable.Keys.Count;
                }

                public void Reset()
                {
                    currentPosition = 0;
                }

            }


            public bool ContainsKey(string key)
            {
                return symbolTable.ContainsKey(key);
            }

            /// <summary>
            /// Not implemented -cannot remove an item from teh symbol table.
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }


            /// <summary>
            /// Not implemented -cannot clear a symbol table through this collection wrapper.
            /// </summary>
            public void Clear()
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return symbolTable.Count; }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                foreach (string key in this.Keys)
                {
                    yield return this.symbolTable[key].Get();
                }
            }

            
        }
    }
}
