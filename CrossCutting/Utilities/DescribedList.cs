using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Simple class wrappering a List of object. This overrides the standard ToString to synthesize a name based on the list content. Useful when presenting data
    /// in a property grid.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [System.Runtime.Serialization.CollectionDataContract]
    public class DescribedList<TItem> : List<TItem>
    {
        public override string ToString()
        {
            if (this.Count == 0) return "Empty";
            StringBuilder sb = new StringBuilder();
            foreach (TItem item in this)
            {
                sb.Append(item.ToString());
                sb.Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
            }
            sb.Remove(sb.Length - System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.Length, System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.Length);
            return sb.ToString();

        }
    }
}
