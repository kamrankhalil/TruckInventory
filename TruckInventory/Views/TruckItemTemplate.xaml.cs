using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace TruckInventory.Views
{
    public partial class TruckItemTemplate : Grid
    {
        public static readonly BindableProperty MakeProperty =
                    BindableProperty.Create(nameof(Make), typeof(string), typeof(TruckItemTemplate), default(string));

        public static readonly BindableProperty LicenseProperty =
            BindableProperty.Create(nameof(License), typeof(string), typeof(TruckItemTemplate), default(string));

        public static readonly BindableProperty IsAvailableProperty =
            BindableProperty.Create(nameof(IsAvailable), typeof(string), typeof(TruckItemTemplate), default(string));

        public static readonly BindableProperty PurchaseDateProperty =
            BindableProperty.Create(nameof(PurchaseDate), typeof(string), typeof(TruckItemTemplate), default(string));

        public static readonly BindableProperty ActionProperty =
            BindableProperty.Create(nameof(Action), typeof(string), typeof(TruckItemTemplate), default(string));

        public static readonly BindableProperty IsHeaderProperty =
            BindableProperty.Create(nameof(IsHeader), typeof(bool), typeof(TruckItemTemplate), default(bool));

        public static readonly BindableProperty ActionCommandProperty =
            BindableProperty.Create(nameof(ActionCommand), typeof(ICommand), typeof(TruckItemTemplate), default(ICommand));

        public TruckItemTemplate()
        {
            InitializeComponent();
        }

        public string Make
        {
            get
            {
                return (string)GetValue(MakeProperty);
            }

            set
            {
                SetValue(MakeProperty, value);
            }
        }

        public string License
        {
            get
            {
                return (string)GetValue(LicenseProperty);
            }

            set
            {
                SetValue(LicenseProperty, value);
            }
        }


        public string IsAvailable
        {
            get
            {
                return (string)GetValue(IsAvailableProperty);
            }

            set
            {
                SetValue(IsAvailableProperty, value);
            }
        }

        public string PurchaseDate
        {
            get
            {
                return (string)GetValue(PurchaseDateProperty);
            }

            set
            {
                SetValue(PurchaseDateProperty, value);
            }
        }

        public string Action
        {
            get
            {
                return (string)GetValue(ActionProperty);
            }

            set
            {
                SetValue(ActionProperty, value);
            }
        }

        public bool IsHeader
        {
            get
            {
                return (bool)GetValue(IsHeaderProperty);
            }

            set
            {
                SetValue(IsHeaderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets EditCommand
        /// </summary>
        public ICommand ActionCommand
        {
            get
            {
                return (ICommand)GetValue(ActionCommandProperty);
            }

            set
            {
                SetValue(ActionCommandProperty, value);
            }
        }
    }
}
