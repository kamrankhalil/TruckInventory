# TruckInventory

This is test assignment from NewTrax. A basic truck inventory app, having UI to add truck details and showing list of trucks.

## Tech stack & libraries used

This project uses .Net Standard library with Xamarin.Forms version 5.0.0.2515, works on both iOS and Android. It uses following libraries.

* Prism with DryIoc to support navigation across pages as well as registering and resolving dependencies.
* Sqlite to store Truck inventory in local database.
* Xamarin.CommunityToolkit for value converters.
* Xamarin.Essentials for taking photos from camera and library. 
* Xamarin.Forms.Visual.Material to support material design on UI controls. Works on both iOS and Android.

## Architecture

* Project follows MVVM architecture. I tried to create proper abstractions for every service involved in viewmodel logic, this increases code readability, maintainance as well as allows us to write unit tests on this code. 
* Navigation service is used from Prism library, its not difficult to create own navigation service from scratch but takes some time. 
* DatabaseRepository is created with generics, so CRUD operations can be performed on any type of Model without having to write methods separately. 
* Xaml views are created to make view side code separate and more readable. Data bindings along with triggers are used.
* Validation is added on fields, this follows standard enterprise app validation from Xamarin docs, I might have improvised/changed some things to my likings. 
* DisplayAlertService is created to show success, warning alerts or to take quick input from user, abstraction is created so this is unit testable. 
* Tried my level best to not add any magic string in the app, AppResources is created for all strings used. Currently its in English but we can easily add French version, just translated strings are needed. App running in french locale will automatically display translations.
* Reusable ItemTemplate is created for Header and Item view on list page. Bindable properties are created, any type of custom UI component can be created in similar way.

## Unit Tests

xunit along with Moq is used to create couple of unit tests. It just tests the main method (Save) in AddEditViewModel. Unit tests for other methods can be created in similar manner. Proper abstractions are created to support this. 

## Improvements I could do (If I had more time)

* Create Styles, manage colors separately and UI controls based on those styles to create better UI as well as cleaner code.
* Add "no items" label when there are no items in list, its a small change though.
* Although the UI adjusts itself on both portrait and landscape mode, the header labels get cramped. Maybe fix the header, specifically "Availability" label splits into two lines. But this will require changing label or reducing data on UI as there isn't much space to display everything.
* Delete file (as well) from device when removing image. 

## Screenshots

### Landscape List
![Screenshot_20220914-224653](https://user-images.githubusercontent.com/12691365/190302092-f8022b12-da0a-4b64-bf0e-de067989adab.jpg)

### Portait List
![Screenshot_20220914-224641](https://user-images.githubusercontent.com/12691365/190302168-9712b29d-efaa-4deb-9107-4d0c8e4cb8ce.jpg)

### Add Truck
![Screenshot_20220914-224721](https://user-images.githubusercontent.com/12691365/190302129-7a565c09-7e38-4769-8011-3cc1614fd436.jpg)
