namespace restaurant_backend.Models.DTOs.MenuDTOS
{
    public class AddMenuItemRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public string ImageUrl { get; set; }
        public string Category { get; set; }

        
    }
}
