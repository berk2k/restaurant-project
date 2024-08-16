namespace restaurant_backend.Models.DTOs.TableDTOS
{
    public class UpdateTableRequestDTO
    {
        public int NewCapacity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
