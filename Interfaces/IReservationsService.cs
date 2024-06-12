using TabProjectServer.Models.DTO.Reservations;

namespace TabProjectServer.Interfaces
{
    public interface IReservationsService
    {

        Task<CreateReservationResDTO?> CreateReservationAsync(CreateReservationReqDTO req);
        Task<CancelReservationResDTO?> CancelReservationAsync(CancelReservationReqDTO req);
        Task<GetAllReservationsResDTO> GetAllReservationsAsync();
        Task<GetUserReservationsResDTO?> GetUserReservationsAsync(Guid id);
       
    }
}
