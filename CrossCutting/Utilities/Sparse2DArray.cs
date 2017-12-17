using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Class used as a backing store for grids where the data needs serializing to a session server. This acts as a 2d array but is internally stored as a dictionary allowing
    /// it to store data sparsely - an item with no data inside it will be eliminated from the dictionary and will take no storage space.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public class Sparse2DArray<TData> : IEnumerable<Sparse2DArray<TData>.ArrayEntry>
        where TData : class
    {

        public struct ArrayEntry
        {
            public TData Data;
            public int X;
            public int Y;
        }

        [DataMember]
        Dictionary<string, TData> store = new Dictionary<string, TData>();

        /// <summary>
        /// Access the sparse array via an indexer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public TData this[int x, int y]
        {
            get
            {
                string key = this.getKey(x, y);
                if (this.store.ContainsKey(key))
                {
                    return this.store[key];
                }
                return null;
            }
            set
            {
                string key = this.getKey(x, y);
                if (value == null)
                {
                    if (this.store.ContainsKey(key))
                    {
                        this.store.Remove(key);
                    }
                }
                else
                {
                    if (!this.store.ContainsKey(key))
                    {
                        this.store.Add(key, value);
                    }
                    else
                    {
                        this.store[key] = value;
                    }

                }
            }
        }

        private string getKey(int x, int y)
        {
            return x.ToString() + ":" + y.ToString();
        }

        public void Clear()
        {
            this.store.Clear();
        }

        public int Count
        {
            get
            {
                return this.store.Count;
            }
        }



        public IEnumerator<Sparse2DArray<TData>.ArrayEntry> GetEnumerator()
        {
            foreach (string key in store.Keys)
            {
                Sparse2DArray<TData>.ArrayEntry entry = new Sparse2DArray<TData>.ArrayEntry();
                entry.Data = store[key];
                string[] indexes = key.Split(':');
                entry.X = int.Parse(indexes[0]);
                entry.Y = int.Parse(indexes[1]);

                yield return entry;
            }
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (string key in store.Keys)
            {
                Sparse2DArray<TData>.ArrayEntry entry = new Sparse2DArray<TData>.ArrayEntry();
                entry.Data = store[key];
                string[] indexes = key.Split(':');
                entry.X = int.Parse(indexes[0]);
                entry.Y = int.Parse(indexes[1]);

                yield return entry;
            }
        }

       

    }
}
