using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts
{
    public interface IPeopleServices
    {
        /// <summary>
        /// Adds a new person to the system and returns the result of the operation.
        /// </summary>
        /// <param name="personAddRequest">The details of the person to be added. This parameter can be null, in which case the operation will fail.</param>
        /// <returns>A <see cref="PersonResponse"/> object containing the details of the added person and the status of the
        /// operation.</returns>
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Retrieves a list of all people.
        /// </summary>
        /// <returns>A list of <see cref="PersonResponse"/> objects representing all people.  The list will be empty if no people
        /// are found.</returns>
        public List<PersonResponse> GetAllPeople();

        /// <summary>
        /// Retrieves a person's details based on their unique identifier.
        /// </summary>
        /// <remarks>If the specified <paramref name="id"/> does not correspond to an existing person, the
        /// method returns <see langword="null"/>.</remarks>
        /// <param name="id">The unique identifier of the person to retrieve. This parameter can be <see langword="null"/>.</param>
        /// <returns>A <see cref="PersonResponse"/> object containing the person's details if found; otherwise, <see
        ///langword="null"/>.</returns>
        public PersonResponse? GetPersonById(Guid? id);

        /// <summary>
        /// Retrieves a list of people filtered based on the specified criteria.
        /// </summary>
        /// <remarks>If <paramref name="searchBy"/> is null, the method applies a default filtering logic 
        /// that may include multiple fields. The filtering is case-insensitive.</remarks>
        /// <param name="searchBy">The field to filter by, such as "Name" or "City". Can be null to apply no specific field filter.</param>
        /// <param name="searchString">The value to search for within the specified field. Cannot be null or empty.</param>
        /// <returns>A list of <see cref="PersonResponse"/> objects that match the specified filter criteria.  Returns an empty
        /// list if no matches are found.</returns>
        public List<PersonResponse> GetFilteredpeople(string? searchBy,string searchString);

        /// <summary>
        /// Retrieves a list of people filtered by the specified search criteria and sorted based on the given
        /// parameters.
        /// </summary>
        /// <remarks>This method allows filtering and sorting of people based on dynamic criteria. The
        /// caller can specify both the field to sort by and the order of sorting. If no search or sort parameters are
        /// provided, the method returns all people sorted by the default field in ascending order.</remarks>
        /// <param name="searchBy">An optional search term used to filter the results. If null or empty, no filtering is applied.</param>
        /// <param name="sortBy">An optional field name by which to sort the results. If null or empty, a default sorting field is used.</param>
        /// <param name="sortOreder">Specifies the sort order, either ascending or descending. Use <see cref="SortOrderEnum.Ascending"/> for
        /// ascending order or <see cref="SortOrderEnum.Descending"/> for descending order.</param>
        /// <returns>A list of <see cref="PersonResponse"/> objects that match the specified search criteria and are sorted as
        /// requested. If no results match the criteria, an empty list is returned.</returns>
        public List<PersonResponse> GetSortedpeople(List<PersonResponse> allPeople, string sortBy, SortOrderEnum sortOreder);

        /// <summary>
        /// Updates the details of an existing person based on the provided update request.
        /// </summary>
        /// <param name="personUpdateRequest">The request containing the updated information for the person.  Must not be <see langword="null"/> and must
        /// include valid data for the update.</param>
        /// <returns>A <see cref="PersonResponse"/> object containing the updated details of the person,  or <see
        /// langword="null"/> if the update could not be performed.</returns>
        public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest);


        public bool DeletePerson(Guid? id);
    }
}
