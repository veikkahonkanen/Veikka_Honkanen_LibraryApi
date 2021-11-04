using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi.DataTransferObjects.Outgoing
{
    public class AuthorDtoOut : BaseDto
    {
        public long? PersonId { get; set; }
        public PersonDtoOut? Person { get; set; }
    }
}
