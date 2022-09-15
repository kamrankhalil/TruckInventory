using System;
using System.Threading.Tasks;

namespace TruckInventory.Services
{
    public interface IDisplayAlertService
    {
        Task ShowAlert(string title, string message, string cancel);

        Task<bool> ShowAlert(string title, string message, string accept, string cancel);
    }
}
