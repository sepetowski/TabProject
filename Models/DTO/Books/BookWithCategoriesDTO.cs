﻿using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Models.DTO.Books
{
    public class BookWithCategoriesDTO
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }

        public string? ImageUrl { get; set; } = null;

        public required bool isAvaible { get; set; }


        public required List<CategoryDTO> Categories { get; set; }
    }
}
