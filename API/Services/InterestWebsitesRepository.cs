using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class InterestWebsitesRepository : ISubRepository<Website>
{
    readonly DataContext _context;

    public InterestWebsitesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Website>> Get(int interestID)
    {
        var interest = _context.Interests.FirstOrDefault(i => i.InterestID == interestID);
        if (interest is null) return Enumerable.Empty<Website>();

        var websites = await _context.Websites.Include(w => w.People)
                                     .Include(w => w.Interests)
                                     .Where(w => w.Interests!.Contains(interest))
                                     .ToListAsync();

        return websites.Select(WebsitesRepository.FilterWebsite);
    }

    public async Task<Website?> Add(int interestID, int entityID)
    {
        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == interestID);

        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == entityID);

        if (interest is null || website is null || interest.Websites!.Any(w => w.WebsiteID == entityID)) return null;

        interest.Websites!.Add(website);
        await _context.SaveChangesAsync();

        return WebsitesRepository.FilterWebsite(website);
    }

    public async Task<Website?> Delete(int interestID, int entityID)
    {
        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == interestID);

        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == entityID);

        if (interest is null || website is null || interest.Websites!.All(w => w.WebsiteID != entityID))
            return null;

        interest.Websites!.Remove(website);
        await _context.SaveChangesAsync();

        return WebsitesRepository.FilterWebsite(website);
    }
}