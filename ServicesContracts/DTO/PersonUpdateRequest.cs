using Entities;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ServicesContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "write real email")]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public bool? ReceiveNewsLetters { get; set; }

        public Person toPerson()
        {
            return new Person()
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Address = this.Address,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                CountryId = this.CountryId,
                ReceiveNewsLetters = this.ReceiveNewsLetters

            };
        }

        public PersonResponse toPersonResponse()
        {
            return new PersonResponse()
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Address = this.Address,
                DateOfBirth = this.DateOfBirth,
                Age = this.DateOfBirth.HasValue ? DateTime.Now.Year - this.DateOfBirth.Value.Year : null,
                CountryId = this.CountryId,
                Gender = this.Gender.Value.ToString(),
            };
        }
    } 
}

   

