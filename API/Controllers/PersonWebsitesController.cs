using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/people/{personID:int}/websites")]
[ApiController]
public class PersonWebsitesController : ControllerBase
{
    readonly ISubRepository<Website> _personWebsites;

    public PersonWebsitesController(IRepositoryWrapper repository)
    {
        _personWebsites = repository.PersonWebsites;
    }

    [HttpGet]
    public async Task<IActionResult> GetPersonWebsites(int personID)
    {
        try
        {
            return Ok(await _personWebsites.Get(personID));
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Website>> AddPersonWebsite(int personID, int entityID)
    {
        try
        {
            var website = await _personWebsites.Add(personID, entityID);

            if (website is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetPersonWebsites), new { personID, }, website);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Website>> DeletePersonWebsite(int personID, int entityID)
    {
        try
        {
            var result = await _personWebsites.Delete(personID, entityID);

            if (result is null)
                return NotFound($"Could not remove Website with ID={entityID} from Person with ID={personID}!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}