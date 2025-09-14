using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using Xunit.Abstractions;

namespace TestProject
{
    public class PersonServiceTest
    {
        private readonly IPeopleServices _peopleServices;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ICountriesService _countriesService;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _peopleServices = new PersonService();
            _testOutputHelper = testOutputHelper;
            _countriesService = new CountriesService(false);
        }


        List<PersonResponse> Add3Person()
        {
            CountryResponse countryResponse = _countriesService.AddCountry(new CountryAddRequest() { countryName = "USA" });
            CountryResponse countryResponse1 = _countriesService.AddCountry(new CountryAddRequest() { countryName = "UK" });

            List<PersonResponse> personResponses = new List<PersonResponse>();

            PersonAddRequest? personAddRequest1 = new PersonAddRequest()
            {
                Name = "John Doe",
                Email = "jon@example.com",
                DateOfBirth = new DateTime(2000, 10, 1),
                Address = "123 Main",
                Gender = Gender.Male,
                CountryId = countryResponse.Id,
            };
            personResponses.Add(_peopleServices.AddPerson(personAddRequest1));
            PersonAddRequest? personAddRequest2 = new PersonAddRequest()
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                DateOfBirth = new DateTime(2005, 9, 28),
                Address = "123 Main",
                Gender = Gender.Female,
                CountryId = countryResponse1.Id,
            };
            personResponses.Add(_peopleServices.AddPerson(personAddRequest2));
            PersonAddRequest? personAddRequest3 = new PersonAddRequest()
            {
                Name = "Adam Smith",
                Email = "Adam@expamle.com",
                DateOfBirth = new DateTime(1995, 5, 15),
                Address = "123 Main",
                Gender = Gender.Male,
                CountryId = countryResponse.Id,
                ReceiveNewsLetters = true

            };
            personResponses.Add(_peopleServices.AddPerson(personAddRequest3));

            return personResponses;
        }


        #region AddPerson

        [Fact]
        public void AddPerson_NullPersonAddRequest()
        {
            PersonAddRequest? personAddRequest = null;
            Assert.Throws<ArgumentNullException>(() => _peopleServices.AddPerson(personAddRequest));
        }

        [Fact]
        public void AddPerson_NullPersonName()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                Name = null
            };
            Assert.Throws<ArgumentException>(() => _peopleServices.AddPerson(personAddRequest));
        }

        [Fact]
        public void AddPerson_properName()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                Name = "John Doe",
                Email = "jon@doe.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main",
                Gender = Gender.Male,
                ReceiveNewsLetters = true,
                CountryId = Guid.NewGuid()

            };

            PersonResponse response = _peopleServices.AddPerson(personAddRequest);

            //Assert.Contains(response, _peopleServices.GetAllPeople());
            Assert.Equal("John Doe", response.Name);
            Assert.NotEqual(Guid.Empty, response.Id);
        }
        #endregion

        #region GetPersonById
        [Fact]
        public void GetPersonById_NullGuid()
        {
            Guid? id = null;
            Assert.Throws<ArgumentNullException>(() => _peopleServices.GetPersonById(id));
        }
        [Fact]
        public void GetPersonById_ProperGuid()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                Name = "John Doe",
                Email = "jon@example.com"
            };
            PersonResponse response = _peopleServices.AddPerson(personAddRequest);

            _testOutputHelper.WriteLine($"Person Id: {response.Id}");
            _testOutputHelper.WriteLine($"Person Name: {response.Name}");

            PersonResponse? Actualresponse = _peopleServices.GetPersonById(response.Id);

            Assert.Equal(response, Actualresponse);
        }
        #endregion

        #region GetAllPeople
        [Fact]
        public void GetAllPeople_EmptyList()
        {
            Assert.Empty(_peopleServices.GetAllPeople());
        }
        [Fact]
        public void GetAllPeople_ProperList()
        {
            List<PersonResponse> Expectedpeople = Add3Person();

            List<PersonResponse> Actualpeople = _peopleServices.GetAllPeople();

            foreach (var person in Expectedpeople)
            {
                Assert.Contains(person, Actualpeople);

            }
        }
        #endregion

        #region GetFilteredPeople
        [Fact]
        public void GetFilteredPeople_EmptySearchText()
        {
            List<PersonResponse> Expectedpeople = Add3Person();

            List<PersonResponse> Actualpeople = _peopleServices.GetFilteredpeople(nameof(Person.Name), "");

            foreach (var person in Expectedpeople)
            {
                Assert.Contains(person, Actualpeople);

            }
        }
        [Fact]
        public void GetFilteredPeople_ProperList()
        {
            List<PersonResponse> Expectedpeople = Add3Person();

            List<PersonResponse> Actualpeople = _peopleServices.GetFilteredpeople(nameof(Person.Name), "John");


            Assert.Contains(Expectedpeople.First(), Actualpeople);


        }
        #endregion

        #region GetSortedPeople
        [Fact]
        public void GetSortedPeople_PersonNameDescOrder()
        {
            List<PersonResponse> Expectedpeople = Add3Person();

            Expectedpeople = Expectedpeople.OrderByDescending(temp => temp.Name).ToList();

            List<PersonResponse> people = _peopleServices.GetAllPeople();
            List<PersonResponse> Sortedpeople = _peopleServices.GetSortedpeople(people, nameof(Person.Name), SortOrderEnum.DESC);

            for (int i = 0; i < Expectedpeople.Count; i++)
            {
                Assert.Equal(Expectedpeople[i], Sortedpeople[i]);
            }
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public void UpdatePerson_NullPersonUpdateRequest()
        {
            PersonUpdateRequest? personUpdateRequest = null;
            Assert.Throws<ArgumentNullException>(() => _peopleServices.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                Id = Guid.NewGuid(),
            };

            Assert.Throws<ArgumentException>(() => _peopleServices.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public void UpdatePerson_NullName()
        {
            List<PersonResponse> people = Add3Person();
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                Id = people[0].Id,
                Name = null,
                Email = "jon@ex.com"
            };

            Assert.Throws<ArgumentException>(() => _peopleServices.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public void UpdatePerson_ProperPerson()
        {
            List<PersonResponse> people = Add3Person();
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                Id = people[0].Id,
                Name = "John Updated",
                Email = "jon@update.com",
                DateOfBirth = new DateTime(2011, 11, 11),
                Address = "1234 Main",
                Gender = Gender.Male,
                CountryId = people[0].CountryId
            };

            PersonResponse Expectedresponse = _peopleServices.UpdatePerson(personUpdateRequest);

            PersonResponse Actualresponse = _peopleServices.GetPersonById(people[0].Id);

            Assert.Equal(Expectedresponse, Actualresponse);
        }
        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_NullID()
        {
            Assert.Throws<ArgumentNullException>(() => _peopleServices.DeletePerson(null));
        }

        [Fact]
        public void DeletePerson_InvalidId()
        {
            Assert.False(_peopleServices.DeletePerson(Guid.NewGuid()));

        }

        [Fact]
        public void DeletePerson_ValidID()
        {
            List<PersonResponse> people =  Add3Person();
            Assert.True(_peopleServices.DeletePerson(people[0].Id));
            Assert.DoesNotContain(people[0], _peopleServices.GetAllPeople());
        }

        #endregion
        }
    }