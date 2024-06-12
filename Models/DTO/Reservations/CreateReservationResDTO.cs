namespace TabProjectServer.Models.DTO.Reservations
{
    public class CreateReservationResDTO
    {
        public required Guid Id { get; set; }
        public required DateTime ReservationDate { get; set; }
    }
}
