using ServicesContracts;
using ServicesContracts.DTO;
using Entities;
namespace Services
{
    public class CountriesService : ICountriesService
    {
        List<Country> countries;

        public CountriesService(bool initialize = true)
        {
            countries = new List<Country>();
            if(initialize)
            {
                countries.Add(new Country() { Id = Guid.Parse("8EFB04EB-A6BA-43F6-A51D-BE8A97346549"), Name = "Egypt", });
                countries.Add(new Country() { Id = Guid.Parse("599760DD-C1CE-4E52-BEEA-C0CBDA96792C"), Name = "United States"});
                countries.Add(new Country() { Id = Guid.Parse("7FEA2114-3916-413C-A880-2A091888A677"), Name = "United Kingdom",  });
                countries.Add(new Country() { Id = Guid.Parse("26675BF8-3B0A-4A1C-9E9C-44AC090E7645"), Name = "Canada",  });
                countries.Add(new Country() { Id = Guid.Parse("249D2D84-4F6B-46A3-B123-059B31121920"), Name = "Australia" });
            }
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest), "CountryAddRequest cannot be null.");
            }
            if(string.IsNullOrEmpty(countryAddRequest.countryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.countryName));
            }

            Country newCountry = countryAddRequest.toCountry();

            if(countries.Any(c => c.Name.Equals(newCountry.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Country with name '{countryAddRequest.countryName}' already exists.", nameof(countryAddRequest.countryName));
            }
            newCountry.Id = Guid.NewGuid();
            countries.Add(newCountry);
            return newCountry.toCountryResponse();

        }

        public List<CountryResponse> GetAllCountries()
        {
            return countries.Select(c => c.toCountryResponse()).ToList();
        }

        public CountryResponse? GetCountrybyId(Guid? CountryId)
        {
            if (CountryId == null)
            {
                return null;
            }
            if(countries.Any(c => c.Id == CountryId))
            {
                Country foundCountry = countries.First(c => c.Id == CountryId);
                return foundCountry.toCountryResponse();
            }
            return null;
        }
    }
}
