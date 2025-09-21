using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain entity representing a country.
    /// </summary>
    public class Country
    {
        [Key]
        public Guid Id { get; set; }


        [StringLength(40)]
        public string? Name { get; set; } 
    }
}
