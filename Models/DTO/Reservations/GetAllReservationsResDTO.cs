using TabProjectServer.Models.DTO.Loan;

namespace TabProjectServer.Models.DTO.Reservations
{
    public class GetAllReservationsResDTO
    {
        public required List<ReservationDTO> Reservations { get; set; }
        public required int Amount { get; set; }
    }
}
