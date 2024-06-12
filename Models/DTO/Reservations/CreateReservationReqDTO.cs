using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Reservations
{
    public class CreateReservationReqDTO
    {
        [Required]
        public  Guid UserId { get; set; }
        [Required]
        public  Guid BookId { get; set; }
    }
}
