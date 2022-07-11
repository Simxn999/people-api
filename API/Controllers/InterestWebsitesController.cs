using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/interests/{interestID:int}/websites")]
[ApiController]
public class InterestWebsitesController : ControllerBase
{
    readonly ISubRepository<Website> _interestWebsites;

    public InterestWebsitesController(IRepositoryWrapper repository)
    {
        _interestWebsites = repository.InterestWebsites;
    }

    [HttpGet]
    public async Task<IActionResult> GetInterestWebsites(int interestID)
    {
        try
        {
            return Ok(await _interestWebsites.Get(interestID));
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Website>> AddInterestWebsite(int interestID, int entityID)
    {
        try
        {
            var website = await _interestWebsites.Add(interestID, entityID);

            if (website is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetInterestWebsites), new { interestID, }, website);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Website>> DeleteInterestWebsite(int interestID, int entityID)
    {
        try
        {
            var website = await _interestWebsites.Delete(interestID, entityID);

            if (website is null)
                return NotFound($"Could not remove Website with ID={entityID} from Interest with ID={interestID}!");

            return website;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}