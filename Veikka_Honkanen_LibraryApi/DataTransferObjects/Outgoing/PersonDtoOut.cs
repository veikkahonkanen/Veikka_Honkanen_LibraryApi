using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi.DataTransferObjects.Outgoing
{
    public class PersonDtoOut : BaseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
