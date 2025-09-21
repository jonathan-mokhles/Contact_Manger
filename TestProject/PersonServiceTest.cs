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

        public PersonServiceTest(ITestOutputHelper testOutputHelper, PersonDbContext personDb)
        {
            _testOutputHelper = testOutputHelper;
            _countriesService = new CountriesService(personDb);
            _peopleServices = new PersonService(personDb, _countriesService);
        }



        #region AddPerson

        [Fact]
        public async Task AddPerson_NullPersonAddRequest()
        {
            PersonAddRequest? personAddRequest = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _peopleServices.AddPerson(personAddRequest));
        }

        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                Name = null
            };
            await Assert.ThrowsAsync<ArgumentException>(() => _peopleServices.AddPerson(personAddRequest));
        }

        [Fact]
        public async Task AddPerson_properName()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                Name = "John Doe",
                Email = "jon@doe.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryId = Guid.NewGuid()

            };

            PersonResponse response = await _peopleServices.AddPerson(personAddRequest);

            //Assert.Contains(response, _peopleServices.GetAllPeople());
            Assert.Equal("John Doe", response.Name);
            Assert.NotEqual(Guid.Empty, response.Id);
        }
        #endregion

        #region GetPersonById
        [Fact]
        public async Task GetPersonById_NullGuid()
        {
            Guid? id = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _peopleServices.GetPersonById(id));
        }
        [Fact]
        public async Task GetPersonById_ProperGuid()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                Name = "John Doe",
                Email = "jon@example.com"
            };
            PersonResponse response = await _peopleServices.AddPerson(personAddRequest);

            _testOutputHelper.WriteLine($"Person Id: {response.Id}");
            _testOutputHelper.WriteLine($"Person Name: {response.Name}");

            PersonResponse? Actualresponse = await _peopleServices.GetPersonById(response.Id);

            Assert.Equal(response, Actualresponse);
        }
        #endregion

        #region GetAllPeople
        [Fact]
        public async Task GetAllPeople_EmptyList()
        {
            Assert.Empty(await _peopleServices.GetAllPeople());
        }
        [Fact]
        public async Task GetAllPeople_ProperList()
        {
            List<PersonResponse> Expectedpeople = new List<PersonResponse>();

            List<PersonResponse> Actualpeople = await _peopleServices.GetAllPeople();

            foreach (var person in Expectedpeople)
            {
                Assert.Contains(person, Actualpeople);

            }
        }
        #endregion

        #region GetFilteredPeople
        [Fact]
        public async Task GetFilteredPeople_EmptySearchText()
        {
            List<PersonResponse> Expectedpeople = new List<PersonResponse>();

            List<PersonResponse> Actualpeople = await _peopleServices.GetFilteredpeople(nameof(Person.Name), "");

            foreach (var person in Expectedpeople)
            {
                Assert.Contains(person, Actualpeople);

            }
        }
        [Fact]
        public async Task GetFilteredPeople_ProperList()
        {
            List<PersonResponse> Expectedpeople = new List<PersonResponse>(); ;

            List<PersonResponse> Actualpeople = await _peopleServices.GetFilteredpeople(nameof(Person.Name), "John");


            Assert.Contains(Expectedpeople.First(), Actualpeople);


        }
        #endregion

        #region GetSortedPeople
        [Fact]
        public async Task GetSortedPeople_PersonNameDescOrder()
        {
            List<PersonResponse> Expectedpeople = new List<PersonResponse>();

            Expectedpeople = Expectedpeople.OrderByDescending(temp => temp.Name).ToList();

            List<PersonResponse> people =  await _peopleServices.GetAllPeople();
            List<PersonResponse> Sortedpeople = await _peopleServices.GetSortedpeople(people, nameof(Person.Name), SortOrderEnum.DESC);

            for (int i = 0; i < Expectedpeople.Count; i++)
            {
                Assert.Equal(Expectedpeople[i], Sortedpeople[i]);
            }
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public async Task UpdatePerson_NullPersonUpdateRequest()
        {
            PersonUpdateRequest? personUpdateRequest = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _peopleServices.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                Id = Guid.NewGuid(),
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _peopleServices.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public async Task UpdatePerson_NullName()
        {
            List<PersonResponse> people = new List<PersonResponse>();
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                Id = people[0].Id,
                Name = null,
                Email = "jon@ex.com"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _peopleServices.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public async Task UpdatePerson_ProperPerson()
        {
            List<PersonResponse> people = new List<PersonResponse>();
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                Id = people[0].Id,
                Name = "John Updated",
                Email = "jon@update.com",
                DateOfBirth = new DateTime(2011, 11, 11),
                Address = "1234 Main",
                Gender = GenderOptions.Male,
                CountryId = people[0].CountryId
            };

            PersonResponse Expectedresponse = await _peopleServices.UpdatePerson(personUpdateRequest);

            PersonResponse Actualresponse = await _peopleServices.GetPersonById(people[0].Id);

            Assert.Equal(Expectedresponse, Actualresponse);
        }
        #endregion

        #region DeletePerson

        [Fact]
        public async Task DeletePerson_NullID()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _peopleServices.DeletePerson(null));
        }

        [Fact]
        public async Task DeletePerson_InvalidId()
        {
            Assert.False(await _peopleServices.DeletePerson(Guid.NewGuid()));

        }

        [Fact]
        public async Task DeletePerson_ValidID()
        {
            List<PersonResponse> people =  new List<PersonResponse>();
            Assert.True(await _peopleServices.DeletePerson(people[0].Id));
            Assert.DoesNotContain(people[0],await _peopleServices.GetAllPeople());
        }

        #endregion
        }
    }