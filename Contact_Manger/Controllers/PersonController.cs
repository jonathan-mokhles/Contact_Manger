using Microsoft.AspNetCore.Mvc;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;

namespace Contact_Manger.Controllers
{
    public class PersonController : Controller
    {
        private readonly  IPeopleServices _PersonService;

        public PersonController(IPeopleServices personService)
        {
            _PersonService = personService;
        }

        [Route("Person/Index")]
        [Route("/")]
        public IActionResult Index()
        {
            List<PersonResponse> people =  _PersonService.GetAllPeople();

            return View(people);
        }
    }
}
