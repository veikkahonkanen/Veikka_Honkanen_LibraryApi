using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi.DataTransferObjects.Outgoing
{
    public class LiteratureDtoOut : BaseDto
    {
        public string? Title { get; set; }

        public string? Subtitle { get; set; }

        public string? LiteratureType { get; set; }

        public ICollection<AuthorDtoOut> Authors { get; set; } = new List<AuthorDtoOut>();

        public PublisherDtoOut? Publisher { get; set; }

        public long? YearOfRelease { get; set; }

        public string? Description { get; set; }

        public string? Language { get; set; }

        public string? LanguageOfOriginalWork { get; set; }

        public string? PlaceOfPublication { get; set; }

        public ICollection<SubjectDtoOut> Subjects { get; set; } = new List<SubjectDtoOut>();

        public string? Manufacturer { get; set; }

        public string? AdditionalInformation { get; set; }

        public string? Isbn { get; set; }

        // Bonus 1
        public bool? IsLent { get; set; }
    }

    public class LiteratureDtoOutProfile : Profile
    {
        public LiteratureDtoOutProfile()
        {
            CreateMap<Literature, LiteratureDtoOut>().ReverseMap();
        }
    }
}
