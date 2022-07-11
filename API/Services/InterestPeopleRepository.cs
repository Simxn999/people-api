using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class InterestPeopleRepository : ISubRepository<Person>
{
    readonly DataContext _context;

    public InterestPeopleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> Get(int interestID)
    {
        var interest = _context.Interests.FirstOrDefault(i => i.InterestID == interestID);
        if (interest is null) return Enumerable.Empty<Person>();

        var people = await _context.People.Include(p => p.Interests)
                                   .Include(i => i.Websites)
                                   .Where(p => p.Interests!.Contains(interest))
                                   .ToListAsync();

        return people.Select(PeopleRepository.FilterPerson);
    }

    public async Task<Person?> Add(int interestID, int entityID)
    {
        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == interestID);

        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == entityID);

        if (interest?.People is null || person is null || interest.People.Any(p => p.PersonID == entityID)) return null;

        interest.People.Add(person);
        await _context.SaveChangesAsync();

        return PeopleRepository.FilterPerson(person);
    }

    public async Task<Person?> Delete(int interestID, int entityID)
    {
        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == interestID);

        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == entityID);

        if (interest?.People is null || person is null || interest.People.All(p => p.PersonID != entityID))
            return null;

        interest.People.Remove(person);
        await _context.SaveChangesAsync();

        return PeopleRepository.FilterPerson(person);
    }
}