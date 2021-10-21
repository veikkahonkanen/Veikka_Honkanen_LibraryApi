using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi.Models
{
    public class Customer : BaseModel
    {
        [Required]
        public long PersonId { get; set; }

        [Required]
        public Person Person { get; set; }

        [Required]
        public string Email { get; set; }

        public long PhoneNumber { get; set; }

        public string Address { get; set; }

        public long ZipCode { get; set; }

        public string City { get; set; }

        // Bonus 1
        public ICollection<Loan> Loans { get; set; }

        // Bonus 2, Bonus 1
        public bool IsLoanBanned { get; set; }
    }
}
