using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// a list of fairly generic common delegates.
    /// </summary>
    public static class CommonDelegates
    {
        public delegate object CreateNewObjectDelegate(Type objectType);
        public delegate void VoidDelegate();
        public delegate bool BoolDelegate();
        public delegate void VoidStringDelegate(string parameter);
        public delegate List<object> ObjectListDelegate();
        public delegate List<DescribedObject<Object>> DescribedObjectListDelegate();
        // Does the item signified by this string exist ?
        public delegate bool StringExistsDelegate(string item);

    }
}
