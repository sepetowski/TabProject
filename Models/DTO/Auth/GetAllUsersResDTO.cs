namespace TabProjectServer.Models.DTO.Auth
{
    public class GetAllUsersResDTO
    {
        public required List<UserDTO> Users { get; set; }
        public required int Amount { get; set; }
    }
}
