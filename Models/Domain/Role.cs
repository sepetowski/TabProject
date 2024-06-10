namespace TabProjectServer.Models.Domain
{
    public class Role
    {
        public required Guid Id { get; set; }
        public UserRole UserRole { get; set; } 
        public string? RoleKey { get; set; }

    }

        public enum UserRole
    {
        Admin,
        User
    }
}
