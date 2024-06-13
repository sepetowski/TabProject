using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TabProjectServer.Models.DTO.Books
{
    public class UpdateBookReqDTO
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string BookDescripton { get; set; }
  
        [Required]
        public int NumberOfPage { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }

   
        public IFormFile? ImageFile { get; set; }

        [Required]
        public int AvailableCopies { get; set; }

        public List<Guid>? CategoriesIds { get; set; }
    }
}
