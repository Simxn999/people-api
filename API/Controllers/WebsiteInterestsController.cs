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

    [HttpPost, Route("{interestID:int}")]
    public async Task<ActionResult<Interest>> AddWebsiteInterest(int websiteID, int interestID)
    {
        try
        {
            var interest = await _websiteInterests.Add(websiteID, interestID);

            if (interest is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetWebsiteInterests), new { websiteID, }, interest);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete, Route("{interestID:int}")]
    public async Task<ActionResult<Interest>> DeleteWebsiteInterest(int websiteID, int interestID)
    {
        try
        {
            var result = await _websiteInterests.Delete(websiteID, interestID);

            if (result is null)
                return NotFound($"Could not remove Interest with ID={interestID} from Website with ID={websiteID}!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}