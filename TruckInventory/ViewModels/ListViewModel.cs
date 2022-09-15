using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Navigation;
using TruckInventory.Database;
using TruckInventory.Models;
using TruckInventory.Resources;
using TruckInventory.Services;
using TruckInventory.Views;
using Xamarin.CommunityToolkit.ObjectModel;

namespace TruckInventory.ViewModels
{
    public class ListViewModel : ViewModelBase, IPageLifecycleAware, INavigationAware
    {
        private readonly IDatabaseRepository<Truck> truckRepository;
        private readonly INavigationService navigationService;
        private readonly IDisplayAlertService displayAlertService;

        public ListViewModel(IDatabaseRepository<Truck> truckRepository, INavigationService navigationService, IDisplayAlertService displayAlertService)
        {
            this.truckRepository = truckRepository;
            this.navigationService = navigationService;
            this.displayAlertService = displayAlertService;

            Trucks = new ObservableCollection<Truck>();

            AddTruckCommand = new AsyncCommand(() => ExecuteAddTruckCommand(), _ => !IsLoading);
            ItemActionCommand = new AsyncCommand<Truck>(truck => ExecuteItemActionCommand(truck), _ => !IsLoading);
        }

        private ObservableCollection<Truck> trucks;
        public ObservableCollection<Truck> Trucks
        {
            get
            {
                return trucks;
            }

            set
            {
                trucks = value;
                OnPropertyChanged();
            }
        }

        public IAsyncCommand AddTruckCommand { get; }
        public IAsyncCommand<Truck> ItemActionCommand { get; }

        private async Task ExecuteAddTruckCommand()
        {
            await navigationService.NavigateAsync($"NavigationPage/{nameof(AddEditPage)}", useModalNavigation:true);
        }

        private async Task ExecuteItemActionCommand(Truck truck)
        {
            var shouldDelete = await displayAlertService.ShowAlert(AppResources.ACTION, AppResources.EDIT_OR_DELETE_MESSAGE, AppResources.DELETE, AppResources.EDIT);

            if (shouldDelete)
            {
                var confirmDelete = await displayAlertService.ShowAlert(AppResources.DELETE, string.Format(AppResources.DELETE_CONFIRMATION_MESSAGE, truck.License), AppResources.YES, AppResources.NO);
                if(confirmDelete)
                {
                    var rowsUpdated = await truckRepository.RemoveItemAsync(truck.Id);
                    if(rowsUpdated > 0)
                    {
                        await LoadItems();
                        await displayAlertService.ShowAlert(AppResources.SUCCESS, AppResources.CHANGES_SAVED_MESSAGE, AppResources.OK);
                    }
                }
            }
            else
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add(nameof(Truck), truck);
                await navigationService.NavigateAsync($"NavigationPage/{nameof(AddEditPage)}", navigationParams, useModalNavigation: true);
            }
        }

        private async Task LoadItems()
        {
            try
            {
                IsLoading = true;

                Trucks = new ObservableCollection<Truck>(await truckRepository.GetItemsAsync());

            }
            finally
            {
                IsLoading = false;
            }
        }

        public void OnAppearing()
        {
            //empty
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadItems();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnDisappearing()
        {
            //empty
        }
    }
}
