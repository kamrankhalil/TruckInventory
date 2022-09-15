using System;
using System.Threading.Tasks;

namespace TruckInventory.Services
{
    public class DisplayAlertService : IDisplayAlertService
    {
        public async Task ShowAlert(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> ShowAlert(string title, string message, string accept, string cancel)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
