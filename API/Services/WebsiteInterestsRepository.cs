using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class WebsiteInterestsRepository : ISubRepository<Interest>
{
    readonly DataContext _context;

    public WebsiteInterestsRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Interest>> Get(int websiteID)
    {
        var website = _context.Websites.FirstOrDefault(w => w.WebsiteID == websiteID);
        if (website is null) return Enumerable.Empty<Interest>();

        var interests = await _context.Interests.Include(i => i.People)
                                      .Include(i => i.Websites)
                                      .Where(i => i.Websites!.Contains(website))
                                      .ToListAsync();

        return interests.Select(InterestsRepository.FilterInterest);
    }

    public async Task<Interest?> Add(int websiteID, int entityID)
    {
        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == websiteID);

        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == entityID);

        if (website is null || interest is null || website.Interests!.Any(i => i.InterestID == entityID)) return null;

        website.Interests!.Add(interest);
        await _context.SaveChangesAsync();

        return InterestsRepository.FilterInterest(interest);
    }

    public async Task<Interest?> Delete(int websiteID, int entityID)
    {
        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == websiteID);

        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == entityID);

        if (website is null || interest is null || website.Interests!.All(i => i.InterestID != entityID))
            return null;

        website.Interests!.Remove(interest);
        await _context.SaveChangesAsync();

        return InterestsRepository.FilterInterest(interest);
    }
}