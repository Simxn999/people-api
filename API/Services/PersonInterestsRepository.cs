using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class PersonInterestsRepository : ISubRepository<Interest>
{
    readonly DataContext _context;

    public PersonInterestsRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Interest>> Get(int personID)
    {
        var person = _context.People.FirstOrDefault(p => p.PersonID == personID);
        if (person is null) return Enumerable.Empty<Interest>();

        var interests = await _context.Interests.Include(i => i.People).Include(i => i.Websites).Where(i => i.People!.Contains(person)).ToListAsync();

        return interests.Select(InterestsRepository.FilterInterest);
    }

    public async Task<Interest?> Add(int personID, int entityID)
    {
        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == personID);

        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == entityID);

        if (person is null || interest is null || person.Interests!.Any(i => i.InterestID == entityID)) return null;

        foreach (var website in interest.Websites ?? new List<Website>())
        {
            if (person.Websites!.Contains(website)) continue;

            person.Websites!.Add(website);
        }

        person.Interests!.Add(interest);
        await _context.SaveChangesAsync();

        return InterestsRepository.FilterInterest(interest);
    }

    public async Task<Interest?> Delete(int personID, int entityID)
    {
        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == personID);

        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == entityID);

        if (person is null || interest is null || person.Interests!.All(i => i.InterestID != entityID))
            return null;

        person.Interests!.Remove(interest);
        await _context.SaveChangesAsync();

        return InterestsRepository.FilterInterest(interest);
    }
}