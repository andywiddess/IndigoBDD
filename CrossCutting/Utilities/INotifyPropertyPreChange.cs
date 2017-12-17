using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    public interface INotifyPropertyPreChange
    {
        event PropertyPreChangeEventHandler PropertyPreChange;
    }

    public delegate void PropertyPreChangeEventHandler(object sender, PropertyPreChangeEventArgs e);

    public class PropertyPreChangeEventArgs : EventArgs
    {
        public string PropertyName;

        public PropertyPreChangeEventArgs(string propertyName)
        {
            this.PropertyName = propertyName;
        }
    }
}
