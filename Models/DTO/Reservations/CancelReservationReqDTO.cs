using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Reservations
{
    public class CancelReservationReqDTO
    {
        [Required]
        public Guid Id { get; set; }
    }
}
