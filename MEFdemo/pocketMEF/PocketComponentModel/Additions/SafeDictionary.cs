#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

#endregion // Using

namespace System.ComponentModel.Composition
{
    #region Documentation
    /// <summary>
    /// Safe dictionary 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    #endregion // Documentation
    public class SafeDictionary<TKey, TValue>: IDictionary<TKey, TValue>
    {
        #region Constants

        public readonly object SYNC_OBJECT = new object ();

        #endregion // Constants

        #region Private / Protected Fields

        private readonly object SYNC_ROOT = new object();
        private Dictionary<TKey, TValue> m_aDictionary;

        #endregion // Private / Protected Fields

        #region Constructors

        #region ()

        #region Documentation
        /// <summary>
        /// Constructor
        /// </summary>
        #endregion // Documentation
        public SafeDictionary ()
        {
            m_aDictionary = new Dictionary<TKey, TValue>();
        }

        #endregion // ()

        #region ( IDictionary<TKey,TValue> dictionary )

        #region Documentation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary"></param>
        #endregion // Documentation
        public SafeDictionary ( IDictionary<TKey, TValue> dictionary )
        {
            m_aDictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        #endregion // ( IDictionary<TKey,TValue> dictionary )

        #endregion // Constructors

        #region IDictionary<TKey,TValue> Implementation

        #region Add

        #region Documentation
        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        #endregion // Documentation
        public void Add ( TKey key, TValue value )
        {
            lock (SYNC_ROOT)
            {
                m_aDictionary.Add (key, value);
            }
        }

        #endregion // Add

        #region Contains Key

        #region Documentation
        /// <summary>
        /// Determines whether the System.Collections.Generic.Dictionary<TKey,TValue>
        /// contains the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        #endregion // Documentation
        public bool ContainsKey ( TKey key )
        {
            bool contains;
            lock (SYNC_ROOT)
            {
                contains = m_aDictionary.ContainsKey (key);
            }
            return contains;
        }

        #endregion // Contains Key

        #region Keys

        #region Documentation
        /// <summary>
        /// Gets a collection containing the keys in the System.Collections.Generic.Dictionary<TKey,TValue>
        /// </summary>
        #endregion // Documentation
        public ICollection<TKey> Keys
        {
            get
            {
                ICollection<TKey> aKeys = null;

                lock (SYNC_ROOT)
                {
                    aKeys = (m_aDictionary as IDictionary<TKey, TValue>).Keys;
                }

                return aKeys;
            }
        }

        #endregion // Keys

        #region Remove

        #region Documentation
        /// <summary>
        /// Removes the value with the specified key from the System.Collections.Generic.Dictionary<TKey,TValue>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        #endregion // Documentation
        public bool Remove ( TKey key )
        {
            bool hasRemoved;
            lock (SYNC_ROOT)
            {
                hasRemoved = m_aDictionary.Remove (key);
            }
            return hasRemoved;
        }

        #endregion // Remove

        #region Try Get Value

        #region Documentation
        /// <summary>
        /// Removes the value with the specified key from the System.Collections.Generic.Dictionary<TKey,TValue>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        #endregion // Documentation
        public bool TryGetValue ( TKey key, out TValue value )
        {
            bool hasRemoved;
            lock (SYNC_ROOT)
            {
                hasRemoved = m_aDictionary.TryGetValue (key, out value);
            }
            return hasRemoved;
        }

        #endregion // Try Get Value

        #region Values

        #region Documentation
        /// <summary>
        /// Gets a collection containing the values in the System.Collections.Generic.Dictionary<TKey,TValue>
        /// </summary>
        #endregion // Documentation
        public ICollection<TValue> Values
        {
            get
            {
                ICollection<TValue> values;
                lock (SYNC_ROOT)
                {
                    values = m_aDictionary.Values;
                }
                return values;
            }
        }

        #endregion // Values

        #region this [TKey key]

        #region Documentation
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        #endregion // Documentation
        public TValue this[TKey key]
        {
            get
            {
                TValue val;
                lock (SYNC_ROOT)
                {
                    val = m_aDictionary[key];
                };
                return val;
            }
            set
            {
                lock (SYNC_ROOT)
                {
                    m_aDictionary[key] = value;
                }
            }
        }

        #endregion // this [TKey key]

        #endregion // IDictionary<TKey,TValue> Implementation

        #region ICollection<KeyValuePair<TKey,TValue>> Implementation

        #region Add

        #region Documentation
        /// <summary>
        /// Adds the specified key and value to the dictionary
        /// </summary>
        /// <param name="item"></param>
        #endregion // Documentation
        public void Add ( KeyValuePair<TKey, TValue> item )
        {
            lock (SYNC_ROOT)
            {
                m_aDictionary.Add (item.Key, item.Value);
            }
        }

        #endregion // Add

        #region Clear

        #region Documentation
        /// <summary>
        /// Removes all keys and values from the System.Collections.Generic.Dictionary<TKey,TValue>
        /// </summary>
        #endregion // Documentation
        public void Clear ()
        {
            lock (SYNC_ROOT)
            {
                m_aDictionary.Clear ();
            }
        }

        #endregion // Clear

        #region Contains

        #region Documentation
        /// <summary>
        /// Determines whether the System.Collections.Generic.Dictionary<TKey,TValue>
        /// contains the specified key
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        #endregion // Documentation
        bool ICollection<KeyValuePair<TKey,TValue>>.Contains ( KeyValuePair<TKey, TValue> item )
        {
            bool val;
            lock (SYNC_ROOT)
            {
                val = (m_aDictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains (item);
            }
            return val;
        }

        #endregion // Contains

        #region Copy To

        #region Documentation
        /// <summary>
        /// Copies the System.Collections.Generic.Dictionary<TKey,TValue>.KeyCollection
        ///     elements to an existing one-dimensional System.Array, starting at the specified
        ///     array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        #endregion // Documentation
        void ICollection<KeyValuePair<TKey,TValue>>.CopyTo ( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
        {
            lock (SYNC_ROOT)
            {
                (m_aDictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo (array, arrayIndex);
            }
        }

        #endregion // Copy To

        #region Count

        #region Documentation
        /// <summary>
        /// Gets the items count
        /// </summary>
        #endregion // Documentation
        public int Count
        {
            get
            {
                int count;

                lock (SYNC_ROOT)
                {
                    count = m_aDictionary.Count;
                }

                return count;
            }
        }

        #endregion // Count

        #region Is Read Only

        #region Documentation
        /// <summary>
        /// Gets whether the collection is readonly
        /// </summary>
        #endregion // Documentation
        bool ICollection<KeyValuePair<TKey,TValue>>.IsReadOnly
        {
            get
            {
                return (m_aDictionary as ICollection<KeyValuePair<TKey, TValue>>).IsReadOnly;
            }
        }

        #endregion // Is Read Only

        #region Remove

        #region Documentation
        /// <summary>
        /// Removes the value with the specified key from the System.Collections.Generic.Dictionary<TKey,TValue>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        #endregion // Documentation
        bool ICollection<KeyValuePair<TKey,TValue>>.Remove ( KeyValuePair<TKey, TValue> item )
        {
            bool val;
            lock (SYNC_ROOT)
            {
                val = (m_aDictionary as ICollection<KeyValuePair<TKey, TValue>>).Remove (item);
            }
            return val;
        }

        #endregion // Remove

        #endregion // ICollection<KeyValuePair<TKey,TValue>> Implementation

        #region IEnumerable<KeyValuePair<TKey,TValue>> Implementation

        #region Get Enumerator

        #region Documentation
        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns></returns>
        #endregion // Documentation
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey,TValue>>.GetEnumerator ()
        {
            return (m_aDictionary as IEnumerable<KeyValuePair<TKey, TValue>>).GetEnumerator ();
        }

        #endregion // Get Enumerator

        #endregion // IEnumerable<KeyValuePair<TKey,TValue>> Implementation

        #region IEnumerable Implementation

        #region Get Enumerator

        #region Documentation
        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns></returns>
        #endregion // Documentation
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return (m_aDictionary as System.Collections.IEnumerable).GetEnumerator ();
        }

        #endregion // Get Enumerator

        #endregion // IEnumerable Implementation
    }
}
