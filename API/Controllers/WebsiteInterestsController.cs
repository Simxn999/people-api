using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/websites/{websiteID:int}/interests")]
[ApiController]
public class WebsiteInterestsController : ControllerBase
{
    readonly ISubRepository<Interest> _websiteInterests;

    public WebsiteInterestsController(IRepositoryWrapper repository)
    {
        _websiteInterests = repository.WebsiteInterests;
    }

    [HttpGet]
    public async Task<IActionResult> GetWebsiteInterests(int websiteID)
    {
        try
        {
            return Ok(await _websiteInterests.Get(websiteID));
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Interest>> AddWebsiteInterest(int websiteID, int entityID)
    {
        try
        {
            var interest = await _websiteInterests.Add(websiteID, entityID);

            if (interest is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetWebsiteInterests), new { websiteID, }, interest);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Interest>> DeleteWebsiteInterest(int websiteID, int entityID)
    {
        try
        {
            var result = await _websiteInterests.Delete(websiteID, entityID);

            if (result is null)
                return NotFound($"Could not remove Interest with ID={entityID} from Website with ID={websiteID}!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}