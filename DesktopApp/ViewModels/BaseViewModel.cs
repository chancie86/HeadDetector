using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using FaceDetector.Annotations;

namespace FaceDetector.ViewModels
{
    public abstract class BaseViewModel
        : INotifyPropertyChanged
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
