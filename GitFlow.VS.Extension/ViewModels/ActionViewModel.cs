using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GitFlowVS.Extension.Annotations;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace GitFlowVS.Extension.ViewModels
{
    public class ActionViewModel : INotifyPropertyChanged
    {
        private Visibility showStartFeature;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand StartFeatureDropDownCommand { get; private set; }
        public ICommand CancelStartFeatureCommand { get; private set; }

        public ActionViewModel()
        {
            ShowStartFeature = Visibility.Collapsed;
            StartFeatureDropDownCommand = new DropDownLinkCommand(p => StartFeatureDropDown(), p => CanShowStartFeatureDropDown());
            CancelStartFeatureCommand = new CommandHandler(CancelStartFeature, true);
        }

        private void CancelStartFeature()
        {
            ShowStartFeature = Visibility.Collapsed;
        }


        private bool CanShowStartFeatureDropDown()
        {
            return true;
        }

        private void StartFeatureDropDown()
        {
            ShowStartFeature = Visibility.Visible;
        }

        public Visibility ShowStartFeature
        {
            get { return showStartFeature; }
            set
            {
                if (value == showStartFeature) return;
                showStartFeature = value;
                OnPropertyChanged();
            }
        }
    }

    public class CommandHandler : ICommand
    {
        private readonly Action action;
        private readonly bool canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }
    }
}
