using Entities;
using Services.Helpers;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Services
{
    public class PersonService : IPeopleServices
    {
        private readonly List<Person> _people;
        private readonly ICountriesService _CountriesService;

        private PersonResponse personToResponse(Person person)
        {
            PersonResponse response = person.toPersonResponse();
            response.Country = _CountriesService.GetCountrybyId(person.CountryId)?.Name;
            return response;
        }
        public PersonService(bool initialize = true)
        {
            _people = new List<Person>();
            _CountriesService = new CountriesService();
            if (initialize)
            {
                _people.Add(new Person()
                {
                    Id = Guid.Parse("A1B2C3D4-E5F6-47A8-90B1-C2D3E4F5A6B7"),
                    Name = "John Doe",
                    Email = "jon@example.com",
                    DateOfBirth = new DateTime(2000, 10, 1),
                    Address = "123 Main",
                    Gender = "Male",
                    CountryId = Guid.Parse("599760DD-C1CE-4E52-BEEA-C0CBDA96792C"), // United States
                });
                _people.Add(new Person()
                {
                    Id = Guid.Parse("B1C2D3E4-F5A6-47A8-90B1-C2D3E4F5A6B8"),
                    Name = "Jane",
                    Email = "jane@example.com",
                    DateOfBirth = new DateTime(2005, 9, 28),
                    Address = "123 Main",
                    Gender = "Female",
                    CountryId = Guid.Parse("7FEA2114-3916-413C-A880-2A091888A677") // United Kingdom,
                });
                _people.Add(new Person()
                {
                    Id = Guid.Parse("C1D2E3F4-A5B6-47A8-90B1-C2D3E4F5A6B9"),
                    Name = "Adam",
                    Email = "Adam@expamle.com",
                    DateOfBirth = new DateTime(1995, 5, 15),
                    Address = "123 Main",
                    Gender = "Male",
                    CountryId = Guid.Parse("8EFB04EB-A6BA-43F6-A51D-BE8A97346549"), // Egypt
                    ReceiveNewsLetters = true
                });
                _people.Add(new Person()
                {
                    Id = Guid.Parse("D1E2F3A4-B5C6-47A8-90B1-C2D3E4F5A6C0"),
                    Name = "Eve",
                    Email = "eve@example.com",
                    ReceiveNewsLetters = false,
                    DateOfBirth = new DateTime(2010, 12, 20),
                    Address = "456 Elm St",
                    CountryId = Guid.Parse("26675BF8-3B0A-4A1C-9E9C-44AC090E7645"), // Canada
                    Gender = "Female"
                });
                _people.Add(new Person()
                {
                    Id = Guid.Parse("E1F2A3B4-C5D6-47A8-90B1-C2D3E4F5A6D1"),
                    Name = "Charlie",
                    Email = "charlie@example.com",
                    DateOfBirth = new DateTime(1988, 3, 30),
                    Address = "789 Oak St",
                    CountryId = Guid.Parse("249D2D84-4F6B-46A3-B123-059B31121920"), // Australia
                    ReceiveNewsLetters = true,
                    Gender = "Male"
                });
            }
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            ValidationHelper.ValidateObject(personAddRequest);

            Person person = personAddRequest.toPerson();

            person.Id = Guid.NewGuid();
            _people.Add(person);
            return personToResponse(person);
        }
        public PersonResponse? GetPersonById(Guid? id)
        {
            if(id == null || id == Guid.Empty) throw new ArgumentNullException("Invalid Id");

            Person? person = _people.FirstOrDefault(temp => temp.Id == id);
            if (person == null) return null;
            return personToResponse(person);

        }
        public List<PersonResponse> GetAllPeople()
        {
            return _people.Select(person => personToResponse(person)).ToList();
        }
        public List<PersonResponse> GetFilteredpeople(string? searchBy, string? searchString)
        {
            List<PersonResponse> filteredPeople = GetAllPeople();

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return filteredPeople;
            }
            switch (searchBy)
            {
                case nameof(PersonResponse.Name): filteredPeople = filteredPeople.Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case nameof(PersonResponse.Email): filteredPeople = filteredPeople.Where(p => !string.IsNullOrEmpty(p.Email) && p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth): filteredPeople = filteredPeople.Where(p => p.DateOfBirth.HasValue && p.DateOfBirth.Value.ToString("yyyy-MM-dd").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    filteredPeople = filteredPeople.Where(p => !string.IsNullOrEmpty(p.Gender) && p.Gender.Equals(searchString, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    filteredPeople = filteredPeople.Where(p => !string.IsNullOrEmpty(p.Address) && p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Country):
                    filteredPeople = filteredPeople.Where(p => !string.IsNullOrEmpty(p.Country) && p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;


                default: throw new ArgumentException("Invalid searchBy parameter.");
            }

            return filteredPeople;

        }
        public List<PersonResponse> GetSortedpeople(List<PersonResponse> allPeople, string sortBy, SortOrderEnum sortOreder)
        {
            if (string.IsNullOrEmpty(sortBy)) return allPeople;

            return (sortOreder == SortOrderEnum.ASC) ? sortBy switch
            {
                nameof(PersonResponse.Name) => allPeople.OrderBy(p => p.Name).ToList(),
                nameof(PersonResponse.Email) => allPeople.OrderBy(p => p.Email).ToList(),
                nameof(PersonResponse.DateOfBirth) => allPeople.OrderBy(p => p.DateOfBirth).ToList(),
                nameof(PersonResponse.Country) => allPeople.OrderBy(p => p.Country).ToList(),
                nameof(PersonResponse.Age) => allPeople.OrderBy(p => p.Age).ToList(),
                nameof(PersonResponse.Address) => allPeople.OrderBy(p => p.Address).ToList(),
                nameof(PersonResponse.Gender) => allPeople.OrderBy(p => p.Gender).ToList(),
                _ => allPeople,


            }
            : sortBy switch
            {
                nameof(PersonResponse.Name) => allPeople.OrderByDescending(p => p.Name).ToList(),
                nameof(PersonResponse.Email) => allPeople.OrderByDescending(p => p.Email).ToList(),
                nameof(PersonResponse.DateOfBirth) => allPeople.OrderByDescending(p => p.DateOfBirth).ToList(),
                nameof(PersonResponse.Country) => allPeople.OrderByDescending(p => p.Country).ToList(),
                nameof(PersonResponse.Age) => allPeople.OrderByDescending(p => p.Age).ToList(),
                nameof(PersonResponse.Address) => allPeople.OrderByDescending(p => p.Address).ToList(),
                nameof(PersonResponse.Gender) => allPeople.OrderByDescending(p => p.Gender).ToList(),
                _ => allPeople,
            };
        }
        public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null) throw new ArgumentNullException("PersonUpdateRequest is null");

            ValidationHelper.ValidateObject(personUpdateRequest);

            Person? existingPerson = _people.FirstOrDefault(p => p.Id == personUpdateRequest.Id);
            if (existingPerson == null) throw new ArgumentException("preson ID is not found");

            existingPerson.Name = personUpdateRequest.Name;
            existingPerson.Email = personUpdateRequest.Email;
            existingPerson.Address = personUpdateRequest.Address;
            existingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            existingPerson.Address = personUpdateRequest.Address;
            existingPerson.Gender = personUpdateRequest.Gender.ToString();
            existingPerson.CountryId = personUpdateRequest.CountryId;
            existingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            //DeletePerson(existingPerson.Id);
            //_people.Add(existingPerson);


            return personToResponse(existingPerson);

        }
        public bool DeletePerson(Guid? id)
        {
            if (id == null) throw new ArgumentNullException("id is null");

            Person? person = _people.FirstOrDefault(t => t.Id == id);
            if (person == null) return false;

            _people.Remove(person);
            return true;


        }
    }
}
