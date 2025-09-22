using CsvHelper;
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
        private readonly PersonDbContext _db;
        private readonly ICountriesService _CountriesService;

        private PersonResponse personToResponse(Person person)
        {
            PersonResponse response = person.toPersonResponse();
            response.Country = _CountriesService.GetCountrybyId(person.CountryId)?.Result.Name;
            return response;
        }

        public PersonService(PersonDbContext personDb, ICountriesService countriesService)
        {
            _db = personDb;
            _CountriesService = countriesService;

        }


        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            ValidationHelper.ValidateObject(personAddRequest);

            Person person = personAddRequest.toPerson();

            person.Id = Guid.NewGuid();
            _db.Sp_InsertPerson(person);
            return personToResponse(person);
        }
        public async Task<PersonResponse?> GetPersonById(Guid? id)
        {
            if(id == null || id == Guid.Empty) throw new ArgumentNullException("Invalid Id");

            Person? person = _db.Persons.FirstOrDefault(temp => temp.Id == id);
            if (person == null) return null;
            return personToResponse(person);

        }
        public async Task<List<PersonResponse>> GetAllPeople()
        {
            return  _db.SP_GetAllPersons().Select(temp => temp.toPersonResponse()).ToList();
        }
        public async Task<List<PersonResponse>> GetFilteredpeople(string? searchBy, string? searchString)
        {
            List<PersonResponse> filteredPeople = await GetAllPeople();

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
        public async Task<List<PersonResponse>> GetSortedpeople(List<PersonResponse> allPeople, string sortBy, SortOrderEnum sortOreder)
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
        public async Task<PersonResponse?> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null) throw new ArgumentNullException("PersonUpdateRequest is null");

            ValidationHelper.ValidateObject(personUpdateRequest);

            Person? existingPerson = _db.Persons.FirstOrDefault(p => p.Id == personUpdateRequest.Id);
            if (existingPerson == null) throw new ArgumentException("preson ID is not found");

            existingPerson.Name = personUpdateRequest.Name;
            existingPerson.Email = personUpdateRequest.Email;
            existingPerson.Address = personUpdateRequest.Address;
            existingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            existingPerson.Address = personUpdateRequest.Address;
            existingPerson.Gender = personUpdateRequest.Gender.ToString();
            existingPerson.CountryId = personUpdateRequest.CountryId;
            existingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            await _db.SaveChangesAsync();


            return personToResponse(existingPerson);

        }
        public async Task<bool> DeletePerson(Guid? id)
        {
            if (id == null) throw new ArgumentNullException("id is null");

            Person? person = _db.Persons.FirstOrDefault(t => t.Id == id);
            if (person == null) return false;

            _db.Persons.Remove(_db.Persons.First(temp => temp.Id == person.Id));
            _db.SaveChanges();
            return true;


        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memoryStream);  
            CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture,leaveOpen:true);
            csvWriter.WriteHeader<PersonResponse>();
            csvWriter.NextRecord();
            List<PersonResponse> people =  _db.SP_GetAllPersons().Select(temp => temp.toPersonResponse()).ToList();
            await csvWriter.WriteRecordsAsync(people);
            memoryStream.Position = 0;
            return memoryStream;

        }
         
    }
}
