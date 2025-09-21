using ServicesContracts;
using ServicesContracts.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Services
{
    public class CountriesService : ICountriesService
    {
        PersonDbContext _db;

        public  CountriesService(PersonDbContext personDb)
        {
            _db = personDb;

        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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

            if(_db.Countries.Any(c => c.Name.Equals(newCountry.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Country with name '{countryAddRequest.countryName}' already exists.", nameof(countryAddRequest.countryName));
            }
            newCountry.Id = Guid.NewGuid();
            _db.Countries.Add(newCountry);
            await _db.SaveChangesAsync();
            return newCountry.toCountryResponse();

        }
        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return _db.Countries.Select(c => c.toCountryResponse()).ToList();
        }
        public async Task<CountryResponse?> GetCountrybyId(Guid? CountryId) 
        {
            if (CountryId == null)
            {
                return null;
            }
            if(_db.Countries.Any(c => c.Id == CountryId))
            {
                Country foundCountry = _db.Countries.First(c => c.Id == CountryId);
                return foundCountry.toCountryResponse();
            }
            return null;
        }
    }
}
