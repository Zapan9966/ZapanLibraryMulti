using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;

namespace ZapanControls.Controls.Calendar.Common
{
    /// <summary>
    /// Implements the INotifyPropertyChanged interface and 
    /// exposes a RaisePropertyChanged method for derived 
    /// classes to raise the PropertyChange event.  The event 
    /// arguments created by this class are cached to prevent 
    /// managed heap fragmentation.
    /// </summary>
    [Serializable]
    public abstract class BindableObject : INotifyPropertyChanged
    {
        #region Data

        private static readonly Dictionary<string, PropertyChangedEventArgs> eventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
        private static readonly object syncLock = new object();

        #endregion // Data

        #region Constructors

        protected BindableObject()
        { }

        #endregion // Constructors

        #region Public Members

        /// <summary>
        /// Raised when a public property of this object is set.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns an instance of PropertyChangedEventArgs for 
        /// the specified property name.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to create event args for.
        /// </param>	
        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("propertyName cannot be null or empty.");

            PropertyChangedEventArgs args;
            lock (syncLock)
            {
                if (!eventArgCache.TryGetValue(propertyName, out args))
                    eventArgCache.Add(propertyName, args = new PropertyChangedEventArgs(propertyName));
            }

            return args;
        }

        #endregion // Public Members

        #region Protected Members

        /// <summary>
        /// Derived classes can override this method to
        /// execute logic after a property is set. The 
        /// base implementation does nothing.
        /// </summary>
        /// <param name="propertyName">
        /// The property which was changed.
        /// </param>
        protected virtual void AfterPropertyChanged(string propertyName)
        {
        }

        /// <summary>
        /// Attempts to raise the PropertyChanged event, and 
        /// invokes the virtual AfterPropertyChanged method, 
        /// regardless of whether the event was raised or not.
        /// </summary>
        /// <param name="propertyName">
        /// The property which was changed.
        /// </param>
        protected void RaisePropertyChanged(string propertyName)
        {
            VerifyProperty(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                // Get the cached event args.
                PropertyChangedEventArgs args = GetPropertyChangedEventArgs(propertyName);

                // Raise the PropertyChanged event.
                handler(this, args);
            }

            AfterPropertyChanged(propertyName);
        }

        #endregion // Protected Members

        #region Private Helpers

        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            // Thanks to Rama Krishna Vavilala for the tip to use TypeDescriptor here, instead of manual
            // reflection, so that custom properties are honored too.
            // http://www.codeproject.com/KB/WPF/podder1.aspx?msg=2381272#xx2381272xx

            bool propertyExists = TypeDescriptor.GetProperties(this).Find(propertyName, false) != null;
            if (!propertyExists)
            {
                // The property could not be found,
                // so alert the developer of the problem.

                string msg = $"{ propertyName} is not a public property of {this.GetType().FullName}";
                Debug.Fail(msg);
            }
        }

        #endregion // Private Helpers
    }
}
