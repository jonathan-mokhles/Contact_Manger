using ServicesContracts.DTO;

namespace ServicesContracts
{
    /// <summary>
    /// Provides operations for managing countries, 
    /// </summary>
    /// <remarks>This service defines methods for interacting with country-related data. Implementations of
    /// this interface should handle the necessary validation and processing to ensure correct behavior.</remarks>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a new country based on the provided CountryAddRequest.
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns> country name with its id </returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Retrieves a list of all countries.
        /// </summary>
        /// <returns> list of  CountryResponse</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// gets a country by its unique identifier.
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns>CountryResponse</returns>
        Task<CountryResponse> GetCountrybyId(Guid? CountryId);




    }
}
