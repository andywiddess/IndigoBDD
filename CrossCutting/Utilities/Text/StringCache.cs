using System.Collections.Generic;
using System.Linq;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// StringCache. Helps to keep number of string allocated low.
	/// Note, when you do <code>a = "1"; b = 1.ToString();</code> the string "1" is actually
	/// allocated twice in memory. StringCache helps to keep the numbers as low as possible.
	/// It is never purged though, so it should be used only for short lived tasks.
	/// </summary>
	public class StringCache
	{
		#region struct Entry

		/// <summary>Dictionary entry.</summary>
		protected struct Entry
		{
			/// <summary>Entry index.</summary>
			public int Index;
			/// <summary>Entry value.</summary>
			public string Value;
		}

		#endregion

		#region private fields

		/// <summary>Counter.</summary>
		private int m_Counter; // = 0;

		/// <summary>Dictionary.</summary>
		private readonly Dictionary<string, Entry> m_Map = new Dictionary<string, Entry>();

        #endregion

        #region public interface

        /// <summary>Gets the cached version of given string.</summary>
        public string this[string value] => StringOf(value);

        /// <summary>Gets all entries.</summary>
        public IEnumerable<KeyValuePair<int, string>> Index => m_Map.Select(kv => new KeyValuePair<int, string>(kv.Value.Index, kv.Key));

        /// <summary>Gets the values.</summary>
        public IEnumerable<string> Values => m_Map.Keys;

	    /// <summary>Gets the index of given string.</summary>
		/// <param name="value">The value.</param>
		/// <returns>Index of string.</returns>
		public int IndexOf(string value) => Find(value).Index;

	    /// <summary>Gets the cached version of given string.</summary>
		/// <param name="value">The value.</param>
		/// <returns>Cached version given value.</returns>
		public string StringOf(string value) => Find(value).Value;

	    /// <summary>Clears the cache.</summary>
		public void Clear()
		{
			m_Map.Clear();
			m_Counter = 0;
		}

		#endregion

		#region private implementation

		/// <summary>Finds entry for the specified value.</summary>
		/// <param name="value">The value.</param>
		/// <returns>Entry for given value.</returns>
		private Entry Find(string value)
		{
			Entry result;
			if (!m_Map.TryGetValue(value, out result))
			{
				result = new Entry { Index = m_Counter, Value = value };
				m_Map.Add(value, result);
				m_Counter++;
			}
			return result;
		}

		#endregion
	}
}
