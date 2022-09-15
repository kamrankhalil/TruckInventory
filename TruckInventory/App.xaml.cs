using System;
using Prism;
using Prism.Ioc;
using TruckInventory.Database;
using TruckInventory.Models;
using TruckInventory.Services;
using TruckInventory.ViewModels;
using TruckInventory.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckInventory
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(ListPage)}");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ListPage, ListViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPage, AddEditViewModel>();
            containerRegistry.Register<IDatabaseRepository<Truck>, DatabaseRepository<Truck>>();
            containerRegistry.Register<IDisplayAlertService, DisplayAlertService>();
        }
    }
}
