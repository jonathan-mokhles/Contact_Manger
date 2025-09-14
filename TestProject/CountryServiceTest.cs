using Services;
using ServicesContracts;
using ServicesContracts.DTO;

namespace TestProject
{
    public class CountryServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountryServiceTest()
        {
            _countriesService = new CountriesService(false);
        }
        #region AddCountry
        [Fact]
        public void AddCountry_NullCountryAddRequest()
        { 
            CountryAddRequest? countryAddRequest = null;
            Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(countryAddRequest));    

        }

        [Fact]
        public void AddCountry_NullCountryName()
        { 
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                countryName = null
            };
            Assert.Throws<ArgumentException>(() => _countriesService.AddCountry(countryAddRequest));
        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest()
            {
                countryName = "Egypt"
            };
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest()
            {
                countryName = "Egypt"
            };
            _countriesService.AddCountry(countryAddRequest1);
            Assert.Throws<ArgumentException>(() => _countriesService.AddCountry(countryAddRequest2));
        }

        [Fact]
        public void AddCountry_properName()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                countryName = "Egypt"
            };

            CountryResponse response =  _countriesService.AddCountry(countryAddRequest);
            Assert.Contains(response, _countriesService.GetAllCountries());
            Assert.Equal("Egypt" , response.Name);
            Assert.NotEqual(Guid.Empty, response.Id);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public void GetAllCountries_NoCountries()
        {
            Assert.Empty(_countriesService.GetAllCountries());
        }
        [Fact]
        public void GetAllCountries_3Countries()
        {
            _countriesService.AddCountry(new CountryAddRequest() { countryName = "Egypt" });
            _countriesService.AddCountry(new CountryAddRequest() { countryName = "USA" });
            _countriesService.AddCountry(new CountryAddRequest() { countryName = "UK" });
            Assert.Collection(_countriesService.GetAllCountries(),
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
        public void GetCountrybyId_NameExist()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                countryName = "Egypt"
            };
            CountryResponse ExpectedResponse = _countriesService.AddCountry(countryAddRequest);
            CountryResponse Actualresponse =  _countriesService.GetCountrybyId(ExpectedResponse.Id);
            Assert.Equal(ExpectedResponse, Actualresponse);
        }
        #endregion


    }
}
