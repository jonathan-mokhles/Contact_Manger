using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServicesContracts.DTO
{
    /// <summary>
    /// DTO for adding a new country.
    /// </summary>
    public class CountryAddRequest
    {
        public string? countryName { get; set; }


        public Country toCountry()
        {
            return new Country()
            {
                Name = countryName
            };
        }
    }
}
