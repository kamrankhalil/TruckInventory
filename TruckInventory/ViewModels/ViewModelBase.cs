using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TruckInventory.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                }
            }
        }
    }
}
