using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Stores reference to object. Reference can be weak (not frozen) or strong (frozen).
    /// It can also be frozen is a scope using <c>using (reference.FreezeScope())</c>.
    /// </summary>
    /// <typeparam name="T">Any class.</typeparam>
    public class Reference<T>
        where T : class
    {
        #region class FreezeReferenceScope<T>

        /// <summary>
        /// FreezeScope helper.
        /// </summary>
        private class FreezeReferenceScope : IDisposable
        {
            #region fields

            private readonly Reference<T> m_Reference;

            #endregion

            #region constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="Reference&lt;T&gt;.FreezeReferenceScope"/> class.
            /// </summary>
            /// <param name="reference">The reference.</param>
            public FreezeReferenceScope(Reference<T> reference)
            {
                if (reference == null)
                    throw new ArgumentNullException("reference", "reference is null.");

                m_Reference = reference;
                m_Reference.Freeze();
            }

            #endregion

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (m_Reference != null)
                {
                    m_Reference.Unfreeze();
                }
            }

            #endregion
        }

        #endregion

        #region fields

        private T m_Strong;
        private WeakReference m_Weak;
        private int m_Frozen;
        private int m_Hashcode;

        #endregion

        #region properties

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public T Target
        {
            get { return m_Strong != null ? m_Strong : (T)m_Weak.Target; }
        }

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Reference&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="freeze">if set to <c>true</c> [freeze].</param>
        public Reference(T subject, bool freeze = false)
        {
            m_Weak = new WeakReference(subject);
            m_Hashcode = subject.GetHashCode();

            if (freeze)
            {
                m_Strong = subject;
                m_Frozen = 1;
            }
            else
            {
                m_Strong = null;
                m_Frozen = 0;
            }
        }

        #endregion

        #region freeze, unfreeze

        /// <summary>
        /// Freezes referenced object. It uses freeze counter so unfreezing doesn't necessarily means making it weak.
        /// Note: method may return <c>null</c> anyway if object is already disposed.
        /// </summary>
        /// <returns>Referenced object.</returns>
        public T Freeze()
        {
            m_Frozen++;
            T reference = (T)m_Weak.Target;

            if (m_Frozen > 1)
            {
                m_Strong = reference;
            }

            return reference;
        }

        /// <summary>
        /// Freezes referenced object. It uses freeze counter so unfreezing doesn't necessarily means making it weak.
        /// Note: method may return <c>null</c> anyway if object is already disposed.
        /// </summary>
        /// <returns>Referenced object.</returns>
        public T Unfreeze()
        {
            m_Frozen--;

            if (m_Frozen <= 0)
            {
                m_Strong = null;
            }

            return (T)m_Weak.Target;
        }

        /// <summary>
        /// Freezes referenced object in a scope of <c>using (reference.FreezeScope())</c>.
        /// </summary>
        /// <returns>Scope <see cref="IDisposable"/> object.</returns>
        public IDisposable FreezeScope()
        {
            return new FreezeReferenceScope(this);
        }

        #endregion
    }
}
