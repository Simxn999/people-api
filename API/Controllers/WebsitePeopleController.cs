using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/websites/{websiteID:int}/people")]
[ApiController]
public class WebsitePeopleController : ControllerBase
{
    readonly ISubRepository<Person> _websitePeople;

    public WebsitePeopleController(IRepositoryWrapper repository)
    {
        _websitePeople = repository.WebsitePeople;
    }

    [HttpGet]
    public async Task<IActionResult> GetWebsitePeople(int websiteID)
    {
        try
        {
            return Ok(await _websitePeople.Get(websiteID));
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Person>> AddWebsitePerson(int websiteID, int entityID)
    {
        try
        {
            var person = await _websitePeople.Add(websiteID, entityID);

            if (person is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetWebsitePeople), new { websiteID, }, person);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Person>> DeleteWebsitePerson(int websiteID, int entityID)
    {
        try
        {
            var person = await _websitePeople.Delete(websiteID, entityID);

            if (person is null)
                return NotFound($"Could not remove Person with ID={entityID} from Website with ID={websiteID}!");

            return person;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}