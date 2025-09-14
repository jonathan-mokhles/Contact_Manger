using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.DTO
{
    /// <summary>
    /// Represents a response containing information about a country.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate country-related data in API responses.</remarks>
    public class CountryResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is CountryResponse other)
            {
                return this.Id == other.Id && this.Name == other.Name;
            }
            return false;
        }
    }

        public static class ExtensionCountry
        {
        public static CountryResponse toCountryResponse( this Country country)
        {
                return new CountryResponse()
            {
                Id = country.Id,
                Name = country.Name
            };

            }
        }
        }

