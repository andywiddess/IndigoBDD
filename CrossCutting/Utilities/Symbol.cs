using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// A symbol in a symbol table. This is used for external application components to pass into a rule evaluation a set of variables 
    /// which can be referenced by the evaluation engine, and by passing delegates for Get/Set will allow the rule enging to read and write
    /// to the symbols backing store.
    /// </summary>
    public class Symbol
    {
        public delegate void SetDelegate(object newValue);
        public delegate object GetDelegate();

        public string Name;
        public string FullName;
        public readonly SetDelegate SetValue;
        public readonly GetDelegate GetValue;
        public Type Type;


        private string category = "";
        /// <summary>
        /// An optional catagory for the symbol. Helps when constructing property grids etc.
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        private string description = "";
        public string Description
        {
            get
            {
                if (description == "")
                {
                    return this.FullName;
                }
                else
                {
                    return description;
                }
            }
            set { this.description = value; }
        }

        /// <summary>
        /// Returns TRUE if this is a non-object data type.
        /// </summary>
        public bool IsSimpleType
        {
            get
            {
                if (this.Type == typeof(string) || this.Type.IsValueType)
                {
                    return true;
                }
                return false;
            }
        }

        private bool isDirectlyEditable = true;
        /// <summary>
        /// Returns TRUE if the publisher of this symbol thinks the user can directly edit this value. If the value IsSimpleType and not IsLookupType
        /// and IsDirectlyEditable has not been specifically set to FALSE, then it is deemed that the user can directly edit an unconstrained value. 
        /// 
        /// It is useful for a publisher to set this to FALSE when they have a string value which needs some GUI support to edit, but otherwise is not 
        /// constrianed.
        /// </summary>
        public bool IsDirectlyEditable
        {
            get { return isDirectlyEditable && IsSimpleType && ! IsLookupType && IsIntelligible; }
            set { isDirectlyEditable = value; }
        }

        /// <summary>
        /// Where an author wants to publish a symbol whose name and description might change over time, but must be identifiable elsewhere (typically in the GUI) they
        /// can set an string identifier here which the consumer can use to identifty a specific symbol in the symbol table. This is not validated and is
        /// not used as a Key.
        /// </summary>
        public string SymbolID { get; set; }

        private bool isIntelligible = true;
        /// <summary>
        /// Returns TRUE (default) if this symbol value would be intelligible to a user. Where the symbol author believes the value would not be 
        /// intelligible, this should be set to FALSE. User interfaces using this symbol would then display some glyph to indicate an unreadable but 
        /// valid value.
        /// </summary>
        public bool IsIntelligible
        {
            get { return isIntelligible; }
            set { isIntelligible = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is mutable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is mutable; otherwise, <c>false</c>.
        /// </value>
        public bool IsMutable { get; set; }
        
        
        /// <summary>
        /// Delegat which, when called, will return a list of valid value for the Setter.
        /// </summary>
        public Indigo.CrossCutting.Utilities.CommonDelegates.DescribedObjectListDelegate ValidValues;

        /// <summary>
        /// REturns TRUE if the ValidValues delegate is set, and therefore this could be 
        /// looked up from that list of valid values.
        /// </summary>
        public bool IsLookupType
        {
            get
            {
                return ValidValues != null;
            }
        }

        /// <summary>
        /// Returns TRUE if the SetValue delegate is not set.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.SetValue == null;
            }
        }

        private Dictionary<string, object> extender { get; set; }
        /// <summary>
        /// Add an extended property to this symbol - this can be any unique key/value pair that the author of the symbol table wants to add. This is useful when 
        /// symbols are recycled back to the author through a delegate and they wish to store more data than just this symbol.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddExtendedProperty(string key, object value)
        {
            if(extender == null) extender = new Dictionary<string,object>();
            extender.Add(key, value);
        }

        /// <summary>
        /// Retrieve an extended property stored using AddExtendedProperty
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetExtendedProperty(string key)
        {
            if (extender == null) return null;
            if (!extender.ContainsKey(key)) return null;
            return extender[key];
        }

        /// <summary>
        /// Retrieve an extended property stored using AddExtendedProperty
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TReturn GetExtendedProperty<TReturn>(string key)
        {
            return (TReturn)GetExtendedProperty(key);
        }

        public Symbol(string name, string fullName, Type symbolType, GetDelegate getter, SetDelegate setter)
        {
            this.Name = name;
            this.FullName = fullName;
            this.Type = symbolType;
            this.SetValue = setter;
            this.GetValue = getter;
            if (this.Type != null && this.Type.IsInterface )
            {
                this.IsMutable = true;
            }
        }

        /// <summary>
        /// Create a symbol which is reflection based on class field passed.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        public Symbol(string name, string fullName, object instance, string propertyName) : this(name, fullName, instance, propertyName, "")
        {
            
        }


        /// <summary>
        /// Create a symbol which is reflection based on class field passed.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="category">The category.</param>
        public Symbol(string name, string fullName, object instance, string propertyName, string category)
        {
            this.Name = name;
            this.FullName = fullName;

            System.Reflection.PropertyInfo propertyInfo = instance.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                this.Type = propertyInfo.PropertyType;
                this.SetValue = delegate(object value) { propertyInfo.SetValue(instance, value, null); };
                this.GetValue = delegate() { return propertyInfo.GetValue(instance, null); };
                this.Category = category;
                if (this.Type.IsInterface )
                {
                    this.IsMutable = true;
                }
            }
            else
            {
                System.Reflection.FieldInfo fieldInfo = instance.GetType().GetField(propertyName);
                System.Diagnostics.Debug.Assert(fieldInfo != null);
                this.Type = fieldInfo.FieldType;
                this.SetValue = delegate(object value) { fieldInfo.SetValue(instance, value); };
                this.GetValue = delegate() { return fieldInfo.GetValue(instance); };
                this.Category = category;
                if (this.Type.IsInterface )
                {
                    this.IsMutable = true;
                }

            }
            
        }

        public Symbol(string name, string fullName, Type symbolType, GetDelegate getter, SetDelegate setter, string category)
            : this(name,fullName,symbolType,getter,setter)
        {
            this.Category = category;
        }

        public object Get()
        {
            if (this.GetValue != null) return this.GetValue();
            return null;
        }

        public TObject Get<TObject>()
            where TObject : class
        {
            if (this.GetValue != null) return this.GetValue() as TObject;
            return null;
        }

        public void Set(object newValue)
        {
            if (this.SetValue != null) this.SetValue(newValue);
        }

        /// <summary>
        /// Set this to provide validation for this symbol.
        /// </summary>
        public Indigo.CrossCutting.Utilities.IValid Validation = null;

        /// <summary>
        /// Returns TRUE if teh Validation delegate is set,
        /// </summary>
        public bool IsValidateable
        {
            get
            {
                return this.Validation != null;
            }
        }
    }
}
