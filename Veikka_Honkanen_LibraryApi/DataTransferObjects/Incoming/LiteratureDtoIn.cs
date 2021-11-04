using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi.DataTransferObjects.Incoming
{
    public class LiteratureDtoIn : BaseDto
    {
        public string? Title { get; set; }

        public string? Subtitle { get; set; }

        public string? LiteratureType { get; set; }

        public ICollection<Author>? Authors { get; set; }

        public Publisher? Publisher { get; set; }

        public long? YearOfRelease { get; set; }

        public string? Description { get; set; }

        public string? Language { get; set; }

        public string? LanguageOfOriginalWork { get; set; }

        public string? PlaceOfPublication { get; set; }

        public ICollection<Subject>? Subjects { get; set; }

        public string? Manufacturer { get; set; }

        public string? AdditionalInformation { get; set; }

        public string? Isbn { get; set; }
    }

    public class LiteratureDtoInProfile : Profile
    {
        public LiteratureDtoInProfile()
        {
            CreateMap<LiteratureDtoIn, Literature>().ReverseMap();
        }
    }
}
