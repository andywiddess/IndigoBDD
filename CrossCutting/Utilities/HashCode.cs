using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    public class HashCode
    {
        #region static fields

        // quite large (~2^28) prime number
        private const int factor = 0x19d699a5;
        // quasi-random number
        private const int initial = 0x775c835a;

        #endregion

        #region HashMany

        /// <summary>
        /// Calculates hash for specified objects. Objects can be null.
        /// It calculates hash to objects (plural) not for a single IEnumerable object.
        /// Unfortunatelly <c>null</c> collection and empty collection return same hash (0).
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>Hash of collection of objects.</returns>
        public static int HashMany(IEnumerable objects)
        {
            if (objects != null)
            {
                unchecked
                {
                    int result = 0;
                    foreach (object o in objects)
                        result = (result * factor) + Hash(o);
                    return result;
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region Hash

        /// <summary>
        /// Calculates hash for specified object. Object can be null.
        /// </summary>
        /// <param name="objectA">The object.</param>
        /// <returns>Hash code.</returns>
        public static int Hash(object objectA)
        {
            unchecked
            {
                return (objectA == null) ? initial : objectA.GetHashCode();
            }
        }

        /// <summary>
        /// Calculates hash for specified objects. Objects can be null.
        /// </summary>
        /// <param name="objectA">The object A.</param>
        /// <param name="objectB">The object B.</param>
        /// <returns>Hash code.</returns>
        public static int Hash(object objectA, object objectB)
        {
            unchecked
            {
                int result = 0;
                result = (result * factor) + Hash(objectA);
                result = (result * factor) + Hash(objectB);
                return result;
            }
        }

        /// <summary>
        /// Calculates hash for specified objects. Objects can be null.
        /// </summary>
        /// <param name="objectA">The object A.</param>
        /// <param name="objectB">The object B.</param>
        /// <param name="objectC">The object C.</param>
        /// <returns>Hash code.</returns>
        public static int Hash(object objectA, object objectB, object objectC)
        {
            unchecked
            {
                int result = 0;
                result = (result * factor) + Hash(objectA);
                result = (result * factor) + Hash(objectB);
                result = (result * factor) + Hash(objectC);
                return result;
            }
        }

        /// <summary>
        /// Calculates hash for specified objects. Objects can be null.
        /// </summary>
        /// <param name="objectA">The object A.</param>
        /// <param name="objectB">The object B.</param>
        /// <param name="objectC">The object C.</param>
        /// <param name="objectD">The object D.</param>
        /// <returns>Hash code.</returns>
        public static int Hash(object objectA, object objectB, object objectC, object objectD)
        {
            unchecked
            {
                int result = 0;
                result = (result * factor) + Hash(objectA);
                result = (result * factor) + Hash(objectB);
                result = (result * factor) + Hash(objectC);
                result = (result * factor) + Hash(objectD);
                return result;
            }
        }

        /// <summary>
        /// Calculates hash for specified objects. Objects can be null.
        /// </summary>
        /// <param name="objectA">The object A.</param>
        /// <param name="objectB">The object B.</param>
        /// <param name="objectC">The object C.</param>
        /// <param name="objectD">The object D.</param>
        /// <param name="objectE">The object E.</param>
        /// <returns>Hash code.</returns>
        public static int Hash(object objectA, object objectB, object objectC, object objectD, object objectE)
        {
            unchecked
            {
                int result = 0;
                result = (result * factor) + Hash(objectA);
                result = (result * factor) + Hash(objectB);
                result = (result * factor) + Hash(objectC);
                result = (result * factor) + Hash(objectD);
                result = (result * factor) + Hash(objectE);
                return result;
            }
        }

        /// <summary>
        /// Calculates hash for specified objects. Objects can be null.
        /// </summary>
        /// <param name="objectA">The object A.</param>
        /// <param name="objectB">The object B.</param>
        /// <param name="objectC">The object C.</param>
        /// <param name="objectD">The object D.</param>
        /// <param name="objectE">The object E.</param>
        /// <param name="objects">More objects.</param>
        /// <returns>Hash code.</returns>
        public static int Hash(object objectA, object objectB, object objectC, object objectD, object objectE, params object[] objects)
        {
            unchecked
            {
                int result = 0;
                result = (result * factor) + Hash(objectA);
                result = (result * factor) + Hash(objectB);
                result = (result * factor) + Hash(objectC);
                result = (result * factor) + Hash(objectD);
                result = (result * factor) + Hash(objectE);
                if (objects != null)
                {
                    int length = objects.Length;
                    for (int i = 0; i < length; i++)
                        result = (result * factor) + Hash(objects[i]);
                }
                return result;
            }
        }

        #endregion
    }
}
