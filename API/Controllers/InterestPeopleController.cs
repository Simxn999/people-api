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

    [HttpPost]
    public async Task<ActionResult<Person>> AddInterestPerson(int interestID, int entityID)
    {
        try
        {
            var person = await _interestPeople.Add(interestID, entityID);

            if (person is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetInterestPeople), interestID, person);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Person>> DeleteInterestPerson(int interestID, int entityID)
    {
        try
        {
            var person = await _interestPeople.Delete(interestID, entityID);

            if (person is null)
                return NotFound($"Could not remove Person with ID={entityID} from Interest with ID={interestID}!");

            return person;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}