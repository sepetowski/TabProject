﻿namespace TabProjectServer.Models.Domain
{
    public class User
    {
        public required Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required byte[] PasswordHash { get; set; } = new byte[32];
        public required byte[] PasswordSalt { get; set; } = new byte[32];
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }


        public required Role Role { get; set; }
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
