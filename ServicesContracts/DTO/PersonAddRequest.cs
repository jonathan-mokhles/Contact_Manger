using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Entities;
using ServicesContracts.Enums;

namespace ServicesContracts.DTO
{
    public  class PersonAddRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required (ErrorMessage ="Email is required")]
        [EmailAddress (ErrorMessage ="write real email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public bool ReceiveNewsLetters { get; set; } = true;

        public Person toPerson()
        {
            return new Person()
            {
                Name = this.Name,
                Email = this.Email,
                Address = this.Address,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                CountryId = this.CountryId,
                ReceiveNewsLetters = this.ReceiveNewsLetters

            };
        }
    }
}
