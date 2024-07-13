using System.ComponentModel.DataAnnotations;

namespace CodeZoneTask_MVC_.ViewModels
{
    public class StoreViewModel
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Store name must be between 2 and 100 characters")]
        public string Name { get; set; }
    }
}
