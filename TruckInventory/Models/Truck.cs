using System;
using SQLite;

namespace TruckInventory.Models
{
    public class Truck : BaseModel
    {
        public string License { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string Photo { get; set; }

        [Ignore]
        public Make Manufacturer { get; set; }

        public string ManufacturerAsString
        {
            get
            {
                return this.Manufacturer.ToString();
            }
            set
            {
                Manufacturer = (Make)Enum.Parse(typeof(Make), value, true);
            }
        }
    }

    public enum Make
    {
        Belaz, Caterpillar, Komatsu
    }
}
