using Microsoft.EntityFrameworkCore;
using TabProjectServer.Models.Domain;

namespace TabProjectServer.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions dbContextOptions):base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");


            modelBuilder.Entity<Book>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                


            modelBuilder.Entity<Book>()
                .HasMany(x => x.Categories)
                .WithMany(y => y.Books)
                .UsingEntity(j => j.ToTable("BookCategory"));


            modelBuilder.Entity<Loan>()
                .HasOne(x => x.User)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Loan>()
                .HasOne(x => x.Book)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.BookId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.User)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.Book)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.BookId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            var roles = new List<Role>
            {
                new()
                {
                      Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                      UserRole=UserRole.Admin,
                      RoleKey="7k7usefUN4kfBIVJ7XnR4NYTC61ioWOB"

                },

                new()
                {
                      Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                      UserRole=UserRole.User
                },
            };

            modelBuilder.Entity<Role>().HasData(roles);
        }

    }
}
