using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;

namespace Contact_Manger.Controllers
{
    [Route("person")]
    public class PersonController : Controller
    {
        private readonly IPeopleServices _PersonService;
        private readonly ICountriesService _CountriesService;

        public PersonController(IPeopleServices personService, ICountriesService countriesService)
        {
            _PersonService = personService;
            _CountriesService = countriesService;
        }

        [Route("Index")]
        [Route("/")]
        public IActionResult Index(string searchField, string? searchString, string sortBy = nameof(PersonResponse.Name), SortOrderEnum sortOrder = SortOrderEnum.ASC)
        {
            List<PersonResponse> people = _PersonService.GetFilteredpeople(searchField, searchString);
            ViewBag.SearchFields = new List<string>
            {
                nameof(PersonResponse.Name),
                nameof(PersonResponse.DateOfBirth),
                nameof(PersonResponse.Email),
                nameof(PersonResponse.Address),
                nameof(PersonResponse.Gender),
                nameof(PersonResponse.ReceiveNewsLetters),
                nameof(PersonResponse.Country),
                nameof(PersonResponse.Age),

            };
            ViewBag.SearchString = searchString;
            ViewBag.SearchField = searchField;

            people = _PersonService.GetSortedpeople(people, sortBy, sortOrder);
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder.ToString();



            return View(people);
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = _CountriesService.GetAllCountries().Select(temp => new SelectListItem { Text = temp.Name, Value = temp.Id.ToString() });

            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest addRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _CountriesService.GetAllCountries();
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            _PersonService.AddPerson(addRequest);
            return RedirectToAction("Index", "Person");
        }

        [Route("edit/{personId}")]
        [HttpGet]
        public IActionResult Edit(Guid personId)
        {
            PersonResponse? person = _PersonService.GetPersonById(personId);
            if (person == null)
            {
                return RedirectToAction("Index", "Person");
            }
            ViewBag.Countries = _CountriesService.GetAllCountries().Select(temp => new SelectListItem { Text = temp.Name, Value = temp.Id.ToString(), Selected = temp.Id == person.CountryId });

            return View(person.toPersonUpdateRequest());
        }

        [Route("edit/{personId}")]
        [HttpPost]
        public IActionResult Edit(Guid personId, PersonUpdateRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _CountriesService.GetAllCountries().Select(temp => new SelectListItem { Text = temp.Name, Value = temp.Id.ToString(), Selected = temp.Id == updateRequest.CountryId });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse? person = _PersonService.GetPersonById(personId);
            if (person == null)
            {
                return RedirectToAction("Index", "Person");
            }
            _PersonService.UpdatePerson(updateRequest);
            return RedirectToAction("Index", "Person");
        }

        [Route("delete/{personId}")]
        [HttpGet]
        public IActionResult Delete(Guid personId)
        {
            PersonResponse? person = _PersonService.GetPersonById(personId);
            if (person == null)
            {
                return RedirectToAction("Index", "Person");
            }
            return View(person);
        }

        [Route("delete/{personId}")]
        [HttpPost]
        public IActionResult DeleteConfirmed(Guid personId)
        {
            PersonResponse? person = _PersonService.GetPersonById(personId);
            if (person == null)
            {
                return RedirectToAction("Index", "Person");
            }
            _PersonService.DeletePerson(personId);
            return RedirectToAction("Index", "Person");
        }
    }
}
