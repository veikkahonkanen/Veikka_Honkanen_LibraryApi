using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public override int SaveChanges()
        {
            AddTimeStamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimeStamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimeStamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            if (entities.Any())
            {
                return;
            }

            var now = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                var baseModel = (BaseModel)entity.Entity;

                if (entity.State == EntityState.Added)
                {
                    baseModel.CreatedAt = now;
                }

                baseModel.UpdatedAt = now;
            }
        }
    }
}
