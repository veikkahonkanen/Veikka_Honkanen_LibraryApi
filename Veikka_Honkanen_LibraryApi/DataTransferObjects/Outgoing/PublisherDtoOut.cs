using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi.DataTransferObjects.Outgoing
{
    public class PublisherDtoOut : BaseDto
    {
        public string? Name { get; set; }
    }

    public class PublisherDtoOutProfile : Profile
    {
        public PublisherDtoOutProfile()
        {
            CreateMap<Publisher, PublisherDtoOut>().ReverseMap();
        }
    }
}
