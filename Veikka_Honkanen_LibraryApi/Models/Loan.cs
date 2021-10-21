using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi.Models
{
    public class Loan : BaseModel
    {
        [Required]
        public long CustomerId { get; set; }

        [Required]
        public long LiteratureId { get; set; }

        [Required]
        public DateTime DateBorrowed { get; set; }

        public DateTime DateDue { get; set; }
    }
}
