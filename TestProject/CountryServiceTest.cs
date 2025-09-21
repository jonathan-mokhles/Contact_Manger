using Entities;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;

namespace TestProject
{
    public class CountryServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountryServiceTest(PersonDbContext personDb)
        {
            _countriesService = new CountriesService(personDb);
        }
        #region AddCountry
        [Fact]
        public async Task AddCountry_NullCountryAddRequest()
        { 
            CountryAddRequest? countryAddRequest = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _countriesService.AddCountry(countryAddRequest));    

        }

        [Fact]
        public async Task AddCountry_NullCountryName()
        { 
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                countryName = null
            };
            await Assert.ThrowsAsync<ArgumentException>(() => _countriesService.AddCountry(countryAddRequest));
        }

        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest()
            {
                countryName = "Egypt"
            };
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest()
            {
                countryName = "Egypt"
            };
            await _countriesService.AddCountry(countryAddRequest1);
            await Assert.ThrowsAsync<ArgumentException>(() => _countriesService.AddCountry(countryAddRequest2));
        }

        [Fact]
        public async Task AddCountry_properNameAsync()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                countryName = "Egypt"
            };

            CountryResponse response =  await _countriesService.AddCountry(countryAddRequest);
            Assert.Contains(response, await _countriesService.GetAllCountries());
            Assert.Equal("Egypt" , response.Name);
            Assert.NotEqual(Guid.Empty, response.Id);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_NoCountries()
        {
            Assert.Empty(await _countriesService.GetAllCountries());
        }
        [Fact]
        public async Task GetAllCountries_3Countries()
        {
            await _countriesService.AddCountry(new CountryAddRequest() { countryName = "Egypt" });
            await _countriesService.AddCountry(new CountryAddRequest() { countryName = "USA" });
            await _countriesService.AddCountry(new CountryAddRequest() { countryName = "UK" });
            Assert.Collection<CountryResponse>( await _countriesService.GetAllCountries(),
                country1 => Assert.Equal("Egypt", country1.Name),
                country2 => Assert.Equal("USA", country2.Name),
                country3 => Assert.Equal("UK", country3.Name)
                );
        }
        #endregion

        #region GetCountrybyId

        
        

        [Fact]
        public void GetCountrybyId_NullId()
        {

            Assert.Null(_countriesService.GetCountrybyId(null));
        }

        [Fact]
        public void GetCountrybyId_NotFound()
        {
             Assert.Null(_countriesService.GetCountrybyId(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetCountrybyId_NameExist()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                countryName = "Egypt"
            };
            CountryResponse ExpectedResponse = await _countriesService.AddCountry(countryAddRequest);
            CountryResponse Actualresponse =  await _countriesService.GetCountrybyId(ExpectedResponse.Id);
            Assert.Equal(ExpectedResponse, Actualresponse);
        }
        #endregion


    }
}
