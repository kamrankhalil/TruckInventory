using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Navigation;
using TruckInventory.Database;
using TruckInventory.Models;
using TruckInventory.Resources;
using TruckInventory.Services;
using TruckInventory.Validation;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace TruckInventory.ViewModels
{
    public class AddEditViewModel : ViewModelBase, IPageLifecycleAware, INavigationAware
    {
        private readonly IDatabaseRepository<Truck> truckRepository;
        private readonly IDisplayAlertService displayAlertService;
        private readonly INavigationService navigationService;
        private Truck truck;
        private Truck truckParam;

        public List<string> ManufacturerList => Enum.GetNames(typeof(Make)).ToList();

        public ValidatableObject<string> Manufacturer { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> License { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<bool> IsAvailable { get; set; } = new ValidatableObject<bool>();

        public ValidatableObject<DateTime> PurchaseDate { get; set; } = new ValidatableObject<DateTime>();

        public ValidatableObject<string> Photo { get; set; } = new ValidatableObject<string>();

        //Minimum purchase date is set to 01 January, 2000 as mentioned in requirements.
        public DateTime MinimumPurchaseDate => new DateTime(2000, 01, 01);

        public IAsyncCommand AddPhotoCommand { get; }
        public IAsyncCommand SaveCommand { get; }
        public IAsyncCommand DeletePhotoCommand { get; }
        public IAsyncCommand CancelCommand { get; }

        public Truck Truck
        {
            get
            {
                return truck;
            }

            set
            {
                truck = value;
                OnPropertyChanged();
            }
        }

        public AddEditViewModel(IDatabaseRepository<Truck> truckRepository, IDisplayAlertService displayAlertService, INavigationService navigationService)
        {
            this.truckRepository = truckRepository;
            this.displayAlertService = displayAlertService;
            this.navigationService = navigationService;

            Truck = new Truck();

            AddValidationRules();

            AddPhotoCommand = new AsyncCommand(() => ExecuteAddPhotoCommand(), _ => !IsLoading);
            SaveCommand = new AsyncCommand(() => ExecuteSaveCommand(), _ => !IsLoading);
            DeletePhotoCommand = new AsyncCommand(() => ExecuteDeletePhotoCommand(), _ => !IsLoading);
            CancelCommand = new AsyncCommand(() => ExecuteCancelCommand(), _ => !IsLoading);
        }

        private void AddValidationRules()
        {
            Manufacturer.Validations.Add(new RequiredRule<string> { ValidationMessage = AppResources.MANUFACTURER_REQUIRED_MESSAGE });
            License.Validations.Add(new RequiredRule<string> { ValidationMessage = AppResources.ID_REQUIRED_MESSAGE });
            License.Validations.Add(new LicenseValidationRule<string> { ValidationMessage = AppResources.LICENSE_INVALID_FORMAT_MESSAGE });
        }

        private bool IsFormValid()
        {
            //Calling validate separately on both to return errors on UI, otherwise statement will just end after first false validation.
            //Validation is intentionally added only these 2 fields.

            var isManufacturerValid = Manufacturer.Validate();
            var isLicenseValid = License.Validate();

            return isManufacturerValid && isLicenseValid;
        }

        private bool IsDataChanged()
        {
            var hasNewValidData = Truck.Id == default(int) && IsFormValid();

            var existingDataChanged = Truck.Id != default(int) &&
                (License.Value != Truck.License || Manufacturer.Value != Truck.ManufacturerAsString ||
                    IsAvailable.Value != Truck.IsAvailable || PurchaseDate.Value != Truck.PurchaseDate ||
                    Photo.Value != Truck.Photo);

            return hasNewValidData || existingDataChanged;
        }

        private async Task ExecuteAddPhotoCommand()
        {
            FileResult fileResult;

            var useCamera = await displayAlertService.ShowAlert(AppResources.PHOTO, AppResources.CAMERA_OR_LIBRARY_CONFIRMATION, AppResources.CAMERA, AppResources.LIBRARY);

            //Pick from camera or library based on user's input on display alert.
            //Using Xamarin.Essentials library here to pick/capture photos.
            try
            {
                if (useCamera)
                {
                    fileResult = await MediaPicker.CapturePhotoAsync();
                }
                else
                {
                    fileResult = await MediaPicker.PickPhotoAsync();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if(fileResult == null || string.IsNullOrWhiteSpace(fileResult.FullPath))
            {
                return;
            }

            var newFile = Path.Combine(FileSystem.CacheDirectory, fileResult.FileName);
            using (var stream = await fileResult.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            Photo.Value = newFile;
            OnPropertyChanged(nameof(Truck));
        }

        private async Task ExecuteSaveCommand()
        {
            if(IsFormValid())
            {
                //Set model value from validatable objects
                PopulateModelFromValidatables();

                var rowsUpdated = await truckRepository.SaveItemAsync(Truck);

                if(rowsUpdated > 0)
                {
                    await displayAlertService.ShowAlert(AppResources.SUCCESS, AppResources.CHANGES_SAVED_MESSAGE, AppResources.OK);
                    await navigationService.GoBackAsync();
                }
            }
        }

        private async Task ExecuteDeletePhotoCommand()
        {
            var deleteConfirmation = await displayAlertService.ShowAlert(AppResources.DELETE, AppResources.PHOTO_DELETE_CONFIRMATION, AppResources.YES, AppResources.NO);
            if(deleteConfirmation)
            {
                Photo.Value = string.Empty;

                //Not deleting actual file from the device for now (To reduce the scope of assignment).
            }
        }

        private async Task ExecuteCancelCommand()
        {
            if (IsDataChanged())
            {
                var cancelConfirmation = await displayAlertService.ShowAlert(AppResources.ALERT, AppResources.UNSAVED_CHANGES_MESSAGE, AppResources.YES, AppResources.NO);
                if(cancelConfirmation)
                {
                    await navigationService.GoBackAsync();
                }
            }
            else
            {
                await navigationService.GoBackAsync();
            }
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            truckParam = parameters.GetValue<Truck>(nameof(Truck));
            if (truckParam != null)
            {
                Truck = truckParam;
                PopulateValidatableObjects();
            }
        }

        private void PopulateValidatableObjects()
        {
            License.Value = Truck.License;
            Manufacturer.Value = Truck.ManufacturerAsString;
            IsAvailable.Value = Truck.IsAvailable;
            PurchaseDate.Value = Truck.PurchaseDate;
            Photo.Value = Truck.Photo;
        }

        private void PopulateModelFromValidatables()
        {
            Truck.License = License.Value;
            Truck.ManufacturerAsString = Manufacturer.Value;
            Truck.IsAvailable = IsAvailable.Value;
            Truck.PurchaseDate = PurchaseDate.Value;
            Truck.Photo = Photo.Value;
        }

        public void OnAppearing()
        {
            //empty
        }

        public void OnDisappearing()
        {
            //empty
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //empty
        }
    }
}
