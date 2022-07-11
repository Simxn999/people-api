using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/websites")]
[ApiController]
public class WebsitesController : ControllerBase
{
    readonly IRepository<Website> _websites;

    public WebsitesController(IRepositoryWrapper repository)
    {
        _websites = repository.Websites;
    }

    [HttpGet]
    public async Task<IActionResult> GetWebsites()
    {
        try
        {
            return Ok(await _websites.GetAll());
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpGet("{websiteID:int}")]
    public async Task<ActionResult<Website>> GetWebsite(int websiteID)
    {
        try
        {
            var result = await _websites.Get(websiteID);

            if (result is null)
                return NotFound($"Website with ID={websiteID} was not found!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Website>> AddWebsite(Website website)
    {
        try
        {
            var newWebsite = await _websites.Add(website);

            if (newWebsite is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetWebsite), new { websiteID = newWebsite.WebsiteID, }, newWebsite);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<Website>> UpdateWebsite(int websiteID, Website website)
    {
        try
        {
            if (websiteID != website.WebsiteID)
                return BadRequest($"ID: {websiteID} does not match with the entity ID: {website.WebsiteID}!");

            var updatedWebsite = await _websites.Get(websiteID);

            if (updatedWebsite is null)
                return NotFound($"Website with ID={websiteID} was not found!");
            if (await _websites.Update(website) is null)
                return NotFound($"Website with ID={websiteID} could not be updated!");

            return updatedWebsite;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Website>> DeleteWebsite(int websiteID)
    {
        try
        {
            var result = await _websites.Get(websiteID);

            if (result is null)
                return NotFound($"Website with ID={websiteID} was not found!");
            if (await _websites.Delete(websiteID) is null)
                return NotFound($"Website with ID={websiteID} could not be deleted!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}