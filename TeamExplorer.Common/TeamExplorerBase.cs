using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;

namespace TeamExplorer.Common
{
    /// <summary>
    /// Team Explorer plugin common base class.
    /// </summary>
    public class TeamExplorerBase : IDisposable, INotifyPropertyChanged
    {
        #region Members

        private bool m_contextSubscribed = false;

        #endregion

        /// <summary>
        /// Get/set the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get { return m_serviceProvider; }
            set
            {
                // Unsubscribe from Team Foundation context changes
                if (m_serviceProvider != null)
                {
                    UnsubscribeContextChanges();
                }

                m_serviceProvider = value;
                
                // Subscribe to Team Foundation context changes
                if (m_serviceProvider != null)
                {
                    SubscribeContextChanges();
                }
            }
        }
        private IServiceProvider m_serviceProvider;

        /// <summary>
        /// Get the requested service from the service provider.
        /// </summary>
        public T GetService<T>()
        {
            Debug.Assert(ServiceProvider != null, "GetService<T> called before service provider is set");
            if (ServiceProvider != null)
            {
                return (T)ServiceProvider.GetService(typeof(T));
            }

            return default(T);
        }

        /// <summary>
        /// Show a notification in the Team Explorer window.
        /// </summary>
        public Guid ShowNotification(string message, NotificationType type)
        {
            ITeamExplorer teamExplorer = GetService<ITeamExplorer>();
            if (teamExplorer != null)
            {
                Guid guid = Guid.NewGuid();
                teamExplorer.ShowNotification(message, type, NotificationFlags.None, null, guid);
                return guid;
            }

            return Guid.Empty;
        }

        #region IDisposable

        /// <summary>
        /// Dispose.
        /// </summary>
        public virtual void Dispose()
        {
            UnsubscribeContextChanges();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Team Foundation Context

        /// <summary>
        /// Subscribe to context changes.
        /// </summary>
        protected void SubscribeContextChanges()
        {
            Debug.Assert(ServiceProvider != null, "ServiceProvider must be set before subscribing to context changes");
            if (ServiceProvider == null || m_contextSubscribed)
            {
                return;
            }

            ITeamFoundationContextManager tfContextManager = GetService<ITeamFoundationContextManager>();
            if (tfContextManager != null)
            {
                tfContextManager.ContextChanged += ContextChanged;
                m_contextSubscribed = true;

            }
        }

        /// <summary>
        /// Unsubscribe from context changes.
        /// </summary>
        protected void UnsubscribeContextChanges()
        {
            if (ServiceProvider == null || !m_contextSubscribed)
            {
                return;
            }

            ITeamFoundationContextManager tfContextManager = GetService<ITeamFoundationContextManager>();
            if (tfContextManager != null)
            {
                tfContextManager.ContextChanged -= ContextChanged;
            }
        }

        /// <summary>
        /// ContextChanged event handler.
        /// </summary>
        protected virtual void ContextChanged(object sender, ContextChangedEventArgs e)
        {
        }

        /// <summary>
        /// Get the current Team Foundation context.
        /// </summary>
        protected ITeamFoundationContext CurrentContext
        {
            get
            {
                ITeamFoundationContextManager tfContextManager = GetService<ITeamFoundationContextManager>();
                if (tfContextManager != null)
                {
                    return tfContextManager.CurrentContext;
                }

                return null;
            }
        }

        #endregion
    }
}
