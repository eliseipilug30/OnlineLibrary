using System.Data.Entity;

using OnlineLibrary.Models;

namespace OnlineLibrary.Data
{
    public class OnlineLibraryContext : DbContext
    {
        public OnlineLibraryContext() : base("OnlineLibraryContext")
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}
