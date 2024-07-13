using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CodeZoneTask_MVC_.Models;

namespace CodeZoneTask_MVC_.ViewModels
{
    public class StockIncreaseViewModel
    {
        [Required(ErrorMessage = "Please select a store.")]

        public int? StoreId { get; set; }
        [Required(ErrorMessage = "Please select an item.")]

        public int? ItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Quantity must be bigger than 0")]
        public int Quantity { get; set; }

        public int Transaction { get; set; }

        public List<Store>? Stores { get; set; }

        public List<Item>? Items { get; set; }
    }
}
