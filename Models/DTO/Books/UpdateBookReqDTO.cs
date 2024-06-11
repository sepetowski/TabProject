﻿using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Books
{
    public class UpdateBookReqDTO
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string BookDescripton { get; set; }
  
        [Required]
        public int NumberOfPage { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public List<Guid> CategoriesIds { get; set; }
    }
}
