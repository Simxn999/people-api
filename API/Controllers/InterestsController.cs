using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/interests")]
[ApiController]
public class InterestsController : ControllerBase
{
    readonly IRepository<Interest> _interests;

    public InterestsController(IRepositoryWrapper repository)
    {
        _interests = repository.Interests;
    }

    [HttpGet]
    public async Task<IActionResult> GetInterests()
    {
        try
        {
            return Ok(await _interests.GetAll());
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpGet("{interestID:int}")]
    public async Task<ActionResult<Interest>> GetInterest(int interestID)
    {
        try
        {
            var result = await _interests.Get(interestID);

            if (result is null)
                return NotFound($"Interest with ID={interestID} was not found!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Interest>> AddInterest(Interest interest)
    {
        try
        {
            var newInterest = await _interests.Add(interest);

            if (newInterest is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetInterest), new { interestID = newInterest.InterestID, }, newInterest);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<Interest>> UpdateInterest(int interestID, Interest interest)
    {
        try
        {
            if (interestID != interest.InterestID)
                return BadRequest($"InterestID={interestID} does not match with the EntityID={interest.InterestID}!");

            var updatedInterest = await _interests.Get(interestID);

            if (updatedInterest is null)
                return NotFound($"Interest with ID={interestID} was not found!");
            if (await _interests.Update(interest) is null)
                return NotFound($"Interest with ID={interestID} could not be updated!");

            return updatedInterest;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Interest>> DeleteInterest(int interestID)
    {
        try
        {
            var result = await _interests.Get(interestID);

            if (result is null)
                return NotFound($"Interest with ID={interestID} was not found!");
            if (await _interests.Delete(interestID) is null)
                return NotFound($"Interest with ID={interestID} could not be deleted!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}