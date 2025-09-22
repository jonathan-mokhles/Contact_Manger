using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using Rotativa.AspNetCore;

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
        public async Task<IActionResult> Index(string searchField, string? searchString, string sortBy = nameof(PersonResponse.Name), SortOrderEnum sortOrder = SortOrderEnum.ASC)
        {
            List<PersonResponse> people = await _PersonService.GetFilteredpeople(searchField, searchString);
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

            people = await _PersonService.GetSortedpeople(people, sortBy, sortOrder);
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder.ToString();



            return View(people);
        }

        [Route("Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var countries = await _CountriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp => new SelectListItem { Text = temp.Name, Value = temp.Id.ToString() });

            return View();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest addRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _CountriesService.GetAllCountries();
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            await _PersonService.AddPerson(addRequest);
            return RedirectToAction("Index", "Person");
        }

        [Route("edit/{personId}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid personId)
        {
            PersonResponse? person = await _PersonService.GetPersonById(personId);
            if (person == null)
            {
                return RedirectToAction("Index", "Person");
            }
            var countries = await _CountriesService.GetAllCountries();

            ViewBag.Countries = countries.Select(temp => new SelectListItem { Text = temp.Name, Value = temp.Id.ToString(), Selected = temp.Id == person.CountryId });

            return View(person.toPersonUpdateRequest());
        }

        [Route("edit/{personId}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid personId, PersonUpdateRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                var countries = await _CountriesService.GetAllCountries();

                ViewBag.Countries = countries.Select(temp => new SelectListItem { Text = temp.Name, Value = temp.Id.ToString(), Selected = temp.Id == updateRequest.CountryId });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse? person = await _PersonService.GetPersonById(personId);
            if (person == null)
            {
                return RedirectToAction("Index", "Person");
            }
            await _PersonService.UpdatePerson(updateRequest);
            return RedirectToAction("Index", "Person");
        }

        [Route("delete/{personId}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid personId)
        {
            PersonResponse? person = await _PersonService.GetPersonById(personId);
            if (person == null)
            {
                return RedirectToAction("Index", "Person");
            }
            return View(person);
        }

        [Route("delete/{personId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid personId)
        {
            PersonResponse? person = await _PersonService.GetPersonById(personId);
            if (person == null)
            {
                return RedirectToAction("Index", "Person");
            }
            await _PersonService.DeletePerson(personId);
            return RedirectToAction("Index", "Person");
        }

        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF(Guid personId)
        {
            List<PersonResponse> people = await _PersonService.GetAllPeople();

            return new ViewAsPdf("PersonsPDF",people,ViewData) {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = new Rotativa.AspNetCore.Options.Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
            };

        }

        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV(Guid personId)
        {
           Task<MemoryStream> persons =  _PersonService.GetPersonsCSV();

            return File(persons.Result, "application/octet-stream", "Persons.csv");

        }
    }
}
