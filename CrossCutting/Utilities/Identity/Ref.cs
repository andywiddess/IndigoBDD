using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Identity
{
    [DebuggerDisplay("{DebugString}")]
    public class Ref<T> 
        : IUId<T>, 
          IEquatable<T> where T : IHaveUniversalIdentity
    {
        #region Members
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the debug string.
        /// </summary>
        /// <value>
        /// The debug string.
        /// </value>
        public string DebugString
        {
            get
            {
                return "Ref<{0}> {1}".Frmt(typeof(T).Name, Id.ToString());
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Ref{T}"/> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public Ref(Guid uid)
        {
            this.Id = uid;
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Performs an implicit conversion from <see cref="Ref{T}"/> to <see cref="Guid"/>.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Guid(Ref<T> uid)
        {
            return uid.Id;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Guid"/> to <see cref="Ref{T}"/>.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Ref<T>(Guid guid)
        {
            if (guid != Guid.Empty)
                return new Ref<T>(guid);
            else
                return null;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Nullable{Guid}"/> to <see cref="Ref{T}"/>.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Ref<T>(Guid? guid)
        {
            if (guid.HasValue && guid.Value != Guid.Empty)
                return new Ref<T>(guid.Value);
            else
                return null;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Ref<T> a, IUId<T> b)
        {
            if (object.ReferenceEquals(a, b))
                return true;
            else if (((object)a) == null || ((object)b) == null)
                return false;
            else
                return a.Id == b.Id;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Ref<T> a, IUId<T> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Parses the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public static Ref<T> Parse(string uid)
        {
            if (uid != null)
                return (Ref<T>)Guid.Parse(uid);
            else
                return null;
        }
        #endregion

        #region Public Overrides
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as IUId<T>;

            if (other == null)
                return false;
            else
                return this.Id == other.Id;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(T other)
        {
            return other != null && (object.ReferenceEquals(this, other) || this.Id == other.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return typeof(T).GetHashCode() ^ Id.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// Enumerable Extensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// To the refs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me">Me.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<IUId<T>> ToRefs<T>(this IEnumerable<Guid> me)
            where T : IHaveUniversalIdentity
        {
            return me.Select(x => new Ref<T>(x));
        }

        /// <summary>
        /// To the iu ids.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me">Me.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static List<IUId<T>> ToIUIds<T>(this List<T> me)
            where T : IHaveUniversalIdentity
        {
            return me.Cast<IUId<T>>().ToList();
        }
    }
}
