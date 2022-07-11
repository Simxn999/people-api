using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/interests/{interestID:int}/people")]
[ApiController]
public class InterestPeopleController : ControllerBase
{
    readonly ISubRepository<Person> _interestPeople;

    public InterestPeopleController(IRepositoryWrapper repository)
    {
        _interestPeople = repository.InterestPeople;
    }

    [HttpGet]
    public async Task<IActionResult> GetInterestPeople(int interestID)
    {
        try
        {
            return Ok(await _interestPeople.Get(interestID));
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost, Route("{personID:int}")]
    public async Task<ActionResult<Person>> AddInterestPerson(int interestID, int personID)
    {
        try
        {
            var people = await _interestPeople.Get(interestID);

            if (people.Any(p => p.PersonID == personID))
                return Ok(people); 
                
            var person = await _interestPeople.Add(interestID, personID);

            if (person is null)
                return BadRequest();

            return Ok(await _interestPeople.Get(interestID));
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete, Route("{personID:int}")]
    public async Task<ActionResult<Person>> DeleteInterestPerson(int interestID, int personID)
    {
        try
        {
            var people = await _interestPeople.Get(interestID);

            if (people.All(p => p.PersonID != personID))
                return NotFound($"Person with ID={personID} is not assigned to Interest with ID={interestID}");
            
            var person = await _interestPeople.Delete(interestID, personID);

            if (person is null)
                return NotFound($"Could not remove Person with ID={personID} from Interest with ID={interestID}!");

            return Ok(await _interestPeople.Get(interestID));
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}