using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi.Models
{
    public class Publisher : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
