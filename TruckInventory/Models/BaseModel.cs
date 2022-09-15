using System;
using SQLite;

namespace TruckInventory.Models
{
    public class BaseModel
    {
        //Internal Id for database
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
