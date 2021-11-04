using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi.DataTransferObjects.Outgoing
{
    public class PersonDtoOut : BaseDto
    {
        public long? PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class PersonDtoOutProfile : Profile
    {
        public PersonDtoOutProfile()
        {
            CreateMap<Person, PersonDtoOut>()
                .ForMember(personDto => personDto.PersonId, l => l.MapFrom(person => person.Id))
                .ReverseMap();
        }
    }
}
