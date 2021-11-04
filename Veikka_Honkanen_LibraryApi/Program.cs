using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<LibraryContext>();

                // Generate sample data to populate the DB
                GenerateLiteratureData(context);

                GenerateCustomerData(context);

                context.SaveChanges();
            }

            host.Run();
        }

        /// <summary>
        /// Generates publisher, author, subject and book data
        /// </summary>
        /// <param name="context">Database context</param>
        private static void GenerateLiteratureData(LibraryContext context)
        {
            if (context.Literatures.Any())
            {
                return;
            }

            // Generate data for the first book

            var publisher1 = new Publisher()
            {
                Name = "G. P. Putnam's Sons"
            };

            var publisherEntityEntry1 = context.Publishers.Add(publisher1);

            var person1 = new Person()
            {
                FirstName = "Jack",
                LastName = "Douglas"
            };

            var personEntityEntry1 = context.Persons.Add(person1);

            var author1 = new Author()
            {
                PersonId = personEntityEntry1.Entity.Id,
                Person = personEntityEntry1.Entity
            };

            var authorEntityEntry1 = context.Authors.Add(author1);

            var subject1 = new Subject()
            {
                Name = "Wolves"
            };

            var subjectEntityEntry1 = context.Subjects.Add(subject1);

            var book1 = new Literature()
            {
                Title = "The Jewish-Japanese Sex and Cook Book and How to Raise Wolves",
                Subtitle = "",
                LiteratureType = "Humoristic fiction",
                Authors = new List<Author>() { authorEntityEntry1.Entity },
                Publisher = publisherEntityEntry1.Entity,
                YearOfRelease = 1972,
                Description = "",
                Language = "English",
                LanguageOfOriginalWork = "English",
                PlaceOfPublication = "Virginia",
                Subjects = new List<Subject>() { subjectEntityEntry1.Entity },
                Manufacturer = "Putnam",
                AdditionalInformation = "",
                Isbn = "978-0399110436",
                IsLent = false
            };

            context.Literatures.Add(book1);

            // Generate data for the second book

            var publisher2 = new Publisher()
            {
                Name = "Arthur A. Levine Books"
            };

            var publisherEntityEntry2 = context.Publishers.Add(publisher2);

            var person2 = new Person()
            {
                FirstName = "J.K.",
                LastName = "Rowling"
            };

            var personEntityEntry2 = context.Persons.Add(person2);

            var author2 = new Author()
            {
                PersonId = personEntityEntry2.Entity.Id,
                Person = personEntityEntry2.Entity
            };

            var authorEntityEntry2 = context.Authors.Add(author2);

            var subject2 = new Subject()
            {
                Name = "Magic"
            };

            var subjectEntityEntry2 = context.Subjects.Add(subject2);

            var book2 = new Literature()
            {
                Title = "Harry Potter and the Order of the Phoenix",
                Subtitle = "",
                LiteratureType = "Fantasy",
                Authors = new List<Author>() { authorEntityEntry2.Entity },
                Publisher = publisherEntityEntry2.Entity,
                YearOfRelease = 2003,
                Description = "Dark times have come to Hogwarts. After the Dementors’ attack on his cousin Dudley," +
                "Harry Potter knows that Voldemort will stop at nothing to find him. There are many who deny the Dark Lord’s return " +
                "but Harry is not alone: a secret order gathers at Grimmauld Place to fight against the Dark forces. " +
                "Harry must allow Professor Snape to teach him how to protect himself from Voldemort’s savage assaults on his mind. " +
                "But they are growing stronger by the day and Harry is running out of time. These new editions of the classic and " +
                "internationally bestselling, multi-award-winning series feature instantly pick-up-able new jackets by Jonny Duddle, " +
                "with huge child appeal, to bring Harry Potter to the next generation of readers. It’s time to PASS THE MAGIC ON …",
                Language = "English",
                LanguageOfOriginalWork = "English",
                PlaceOfPublication = "New Jersey",
                Subjects = new List<Subject>() { subjectEntityEntry2.Entity },
                Manufacturer = "Arthur A. Levine Books",
                AdditionalInformation = "Reading age: 9 - 12 years",
                Isbn = "978-0439358064",
                IsLent = false
            };

            context.Literatures.Add(book2);
        }

        /// <summary>
        /// Generates customer data
        /// </summary>
        /// <param name="context">Database context</param>
        private static void GenerateCustomerData(LibraryContext context)
        {
            if (context.Customers.Any())
            {
                return;
            }

            // Generate data for the first customer

            var person1 = new Person()
            {
                FirstName = "Tuomas",
                LastName = "Reijonen"
            };

            var personEntityEntry1 = context.Persons.Add(person1);

            var customer1 = new Customer()
            {
                PersonId = personEntityEntry1.Entity.Id,
                Person = personEntityEntry1.Entity,
                Email = "xamk-ture@smurfmail.com",
                PhoneNumber = 123456789,
                Address = "Tapperistonkatu 3",
                ZipCode = 12345,
                City = "Smurffila",
                Loans = new List<Loan>(),
                IsLoanBanned = false
            };

            context.Customers.Add(customer1);

            // Generate data for the second customer

            var person2 = new Person()
            {
                FirstName = "Veikka",
                LastName = "Honkanen"
            };

            var personEntityEntry2 = context.Persons.Add(person2);

            var customer2 = new Customer()
            {
                PersonId = personEntityEntry2.Entity.Id,
                Person = personEntityEntry2.Entity,
                Email = "veikkahonkanen@smurfmail.com",
                PhoneNumber = 987654321,
                Address = "Nammerheimintie 30",
                ZipCode = 54321,
                City = "Smurffila",
                Loans = new List<Loan>(),
                IsLoanBanned = false
            };

            context.Customers.Add(customer2);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
