using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi.DataTransferObjects.Incoming
{
    public class CustomerDtoIn : BaseDto
    {
        public long? PersonId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public long? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public long? ZipCode { get; set; }

        public string? City { get; set; }

        // Bonus 1
        public ICollection<Loan>? Loans { get; set; }

        // Bonus 2, Bonus 1
        public bool? IsLoanBanned { get; set; }
    }

    public class CustomerDtoInProfile : Profile
    {
        public CustomerDtoInProfile()
        {
            CreateMap<CustomerDtoIn, Customer>()
                .ForPath(customer => customer.Person.FirstName, s => s.MapFrom(customerDto => customerDto.FirstName))
                .ForPath(customer => customer.Person.LastName, s => s.MapFrom(customerDto => customerDto.LastName));
        }
    }
}
