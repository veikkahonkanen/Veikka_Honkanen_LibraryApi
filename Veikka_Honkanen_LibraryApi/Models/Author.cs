using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi.Models
{
    public class Author : BaseModel
    {
        [Required]
        public long PersonId { get; set; }

        [Required]
        public Person Person { get; set; }
    }
}
