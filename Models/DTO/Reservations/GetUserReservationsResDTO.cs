namespace TabProjectServer.Models.DTO.Reservations
{
    public class GetUserReservationsResDTO
    {
        public required List<ReservationDTO> Reservations { get; set; }
        public required int Amount { get; set; }
    }
}
