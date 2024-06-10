namespace TabProjectServer.Models.DTO.Authors
{
    public class GetAllAuthorsResDTO
    {
        public required List<AuthorDTO> Authors {  get; set; }= new();
        public required int Amount { get; set; }
    }
}
