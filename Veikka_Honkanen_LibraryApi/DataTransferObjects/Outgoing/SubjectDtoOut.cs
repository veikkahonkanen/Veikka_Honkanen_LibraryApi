using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi.DataTransferObjects.Outgoing
{
    public class SubjectDtoOut : BaseDto
    {
        public string? Name { get; set; }
    }

    public class SubjectDtoOutProfile : Profile
    {
        public SubjectDtoOutProfile()
        {
            CreateMap<Subject, SubjectDtoOut>().ReverseMap();
        }
    }
}
