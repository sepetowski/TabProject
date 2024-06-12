namespace TabProjectServer.Models.DTO.Reservations
{
    public class CancelReservationResDTO
    {
        public required Guid Id { get; set; }
        public required DateTime ReservationDate { get; set; }
       
    }
}
