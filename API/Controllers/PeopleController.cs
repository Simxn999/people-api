using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/people")]
[ApiController]
public class PeopleController : ControllerBase
{
    readonly IRepository<Person> _people;

    public PeopleController(IRepositoryWrapper repository)
    {
        _people = repository.People;
    }

    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        try
        {
            return Ok(await _people.GetAll());
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpGet("{personID:int}")]
    public async Task<ActionResult<Person>> GetPerson(int personID)
    {
        try
        {
            var result = await _people.Get(personID);

            if (result is null)
                return NotFound($"Person with ID={personID} was not found!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Person>> AddPerson(Person person)
    {
        try
        {
            var newPerson = await _people.Add(person);

            if (newPerson is null)
                return BadRequest();


            return CreatedAtAction(nameof(GetPerson), new { personID = newPerson.PersonID, }, newPerson);
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<Person>> UpdatePerson(int personID, Person person)
    {
        try
        {
            if (personID != person.PersonID)
                return BadRequest($"PersonID={personID} does not match with the EntityID={person.PersonID}!");

            var updatedPerson = await _people.Get(personID);

            if (updatedPerson is null)
                return NotFound($"Person with ID={personID} was not found!");
            if (await _people.Update(person) is null)
                return NotFound($"Person with ID={personID} could not be updated!");

            return updatedPerson;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Person>> DeletePerson(int personID)
    {
        try
        {
            var result = await _people.Get(personID);

            if (result is null)
                return NotFound($"Person with ID = {personID} was not found!");
            if (await _people.Delete(personID) is null)
                return NotFound($"Person with ID = {personID} could not be deleted!");

            return result;
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }
}