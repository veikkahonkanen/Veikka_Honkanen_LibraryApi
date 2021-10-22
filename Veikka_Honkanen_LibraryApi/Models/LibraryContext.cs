using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi.Models
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public DbSet<Literature> Literatures { get; set; }

        public DbSet<Customer> Customers { get; set; }
    }
}
