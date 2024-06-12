namespace TabProjectServer.Models.Domain
{
    public class Reservation
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required Guid BookId { get; set; }
        public required DateTime ReservationDate { get; set; }
        public bool IsActive { get; set; } = true;

        public required User User { get; set; }
        public required Book Book { get; set; }
    }
}
