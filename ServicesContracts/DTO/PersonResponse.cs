using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using ServicesContracts.Enums;

namespace ServicesContracts.DTO
{
    public class PersonResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public bool ReceiveNewsLetters { get; set; } = true;

        public override bool Equals(object? obj)
        {

            if (obj == null || obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;

            return (this.Id == person.Id && this.Name == person.Name && this.Email == person.Email && this.Address == person.Address && this.CountryId == person.CountryId && this.ReceiveNewsLetters == person.ReceiveNewsLetters && this.Gender == person.Gender && this.DateOfBirth == person.DateOfBirth);


        }

        public PersonUpdateRequest toPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Address = this.Address,
                DateOfBirth = this.DateOfBirth,
                CountryId = this.CountryId,
                Gender = this.Gender == "Male" ? GenderOptions.Male : GenderOptions.Female,
                ReceiveNewsLetters = this.ReceiveNewsLetters
            };
        }
    }
    public static class PersonResponseExtension
    {
        public static PersonResponse toPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                Age = person.DateOfBirth.HasValue ? DateTime.Now.Year - person.DateOfBirth.Value.Year : null,
                Gender = person.Gender,
                CountryId = person.CountryId,
                ReceiveNewsLetters = person.ReceiveNewsLetters


            };
        }
    }
}
