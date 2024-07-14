namespace CodeZoneTask_MVC_.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<StoreItem> StoreItems { get; set; } 
    }
}
