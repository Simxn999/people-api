using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/people/{personID:int}/interests")]
[ApiController]
public class PersonInterestsController : ControllerBase
{
    readonly ISubRepository<Interest> _personInterests;

    public PersonInterestsController(IRepositoryWrapper repository)
    {
        _personInterests = repository.PersonInterests;
    }

    [HttpGet]
    public async Task<IActionResult> GetPersonInterests(int personID)
    {
        try
        {
            return Ok(await _personInterests.Get(personID));
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost, Route("{interestID:int}")]
    public async Task<ActionResult<Interest>> AddPersonInterest(int personID, int interestID)
    {
        try
        {
            var interest = await _personInterests.Add(personID, interestID);

            if (interest is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetPersonInterests), new { personID, }, interest);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete, Route("{interestID:int}")]
    public async Task<ActionResult<Interest>> DeletePersonInterest(int personID, int interestID)
    {
        try
        {
            var result = await _personInterests.Delete(personID, interestID);

            if (result is null)
                return NotFound($"Could not remove Interest with ID={interestID} from Person with ID={personID}!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}