namespace CS_ClothesStore.Models.DTOs
{
    public class ChangePasswordDTO
    {
        public int Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
