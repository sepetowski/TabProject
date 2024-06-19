namespace TabProjectServer.Models.DTO.Reservations
{
    public class ReservationDTO
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string Username { get; set; }
        public required Guid BookId { get; set; }
        public required string BookTitle { get; set; }
        public string? ImageUrl { get; set; } = null;
        public required string BookAuthorName { get; set; }
        public required string BookAuthorSurnameName { get; set; }
        public required DateTime ReservationDate { get; set; }
        public required bool IsActive { get; set; }
    }
}
