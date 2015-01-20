using System;
using System.Drawing;
using Microsoft.TeamFoundation.Controls;

namespace Inmeta.TeamExplorer.Extensions.Common
{
    /// <summary>
    /// Team Explorer base navigation item class.
    /// </summary>
    public class TeamExplorerBaseNavigationItem : TeamExplorerBase, ITeamExplorerNavigationItem
    {
        private Image image;
        private string text;
        private bool isVisible = true;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TeamExplorerBaseNavigationItem(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        #region ITeamExplorerNavigationItem

        /// <summary>
        /// Get/set the item text.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; RaisePropertyChanged("Text"); }
        }

        /// <summary>
        /// Get/set the item image.
        /// </summary>
        public System.Drawing.Image Image
        {
            get { return image; }
            set { image = value; RaisePropertyChanged("Image"); }
        }


        /// <summary>
        /// Get/set the IsVisible flag.
        /// </summary>
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; RaisePropertyChanged("IsVisible"); }
        }

        /// <summary>
        /// Invalidate the item state.
        /// </summary>
        public virtual void Invalidate()
        {
        }

        /// <summary>
        /// Execute the item action.
        /// </summary>
        public virtual void Execute()
        {
        }

        #endregion
    }
}
