using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi.Models
{
    public class Literature : BaseModel
    {
        [Required]
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string LiteratureType { get; set; }

        [Required]
        public ICollection<Author> Authors { get; set; }

        [Required]
        public Publisher Publisher { get; set; }

        public long YearOfRelease { get; set; }

        public string Description { get; set; }

        [Required]
        public string Language { get; set; }

        public string LanguageOfOriginalWork { get; set; }

        public string PlaceOfPublication { get; set; }

        [Required]
        public ICollection<Subject> Subjects { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        public string AdditionalInformation { get; set; }

        public string Isbn { get; set; }

        // Bonus 1
        public bool IsLent { get; set; }
    }
}
