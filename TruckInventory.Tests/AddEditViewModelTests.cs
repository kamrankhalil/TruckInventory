using System;
using Moq;
using Prism.Navigation;
using TruckInventory.Database;
using TruckInventory.Models;
using TruckInventory.Services;
using TruckInventory.ViewModels;
using Xunit;

namespace TruckInventory.Tests
{
    public class AddEditViewModelTests
    {
        private readonly Mock<INavigationService> navigationService = new Mock<INavigationService>();
        private readonly Mock<IDisplayAlertService> displayAlertService = new Mock<IDisplayAlertService>();
        private readonly Mock<IDatabaseRepository<Truck>> truckRepository = new Mock<IDatabaseRepository<Truck>>();

        public AddEditViewModel ViewModel { get; }

        public AddEditViewModelTests()
        {
            ViewModel = new AddEditViewModel(truckRepository.Object, displayAlertService.Object, navigationService.Object);
        }

        [Fact]
        public void ShouldNotSaveItemToDatabase_WhenDataIsInValid()
        {
            //Arrange
            ViewModel.License.Value = "ABC";
            ViewModel.Manufacturer.Value = "Caterpillar";
            ViewModel.IsAvailable.Value = true;
            
            //Act
            ViewModel.SaveCommand.Execute(null);

            //Asserts
            truckRepository.Verify(x => x.SaveItemAsync(ViewModel.Truck), Times.Never);
            displayAlertService.Verify(x => x.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            navigationService.Verify(x => x.GoBackAsync(), Times.Never);
        }

        [Fact]
        public void ShouldSaveItemToDatabase_WhenDataIsValid()
        {
            //Arrange
            ViewModel.License.Value = "ABC123";
            ViewModel.Manufacturer.Value = "Caterpillar";
            ViewModel.IsAvailable.Value = true;
            //Setup database method
            truckRepository.Setup(x => x.SaveItemAsync(It.IsAny<Truck>())).ReturnsAsync(1);

            //Act
            ViewModel.SaveCommand.Execute(null);

            //Asserts (Although we should have separate tests for each assert, for simplicity they're added in one test)

            //Should save item to database
            truckRepository.Verify(x => x.SaveItemAsync(ViewModel.Truck), Times.Once);

            //Should show success alert
            displayAlertService.Verify(x => x.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            //Should navigate back to previous page
            navigationService.Verify(x => x.GoBackAsync(), Times.Once);
        }
    }
}

