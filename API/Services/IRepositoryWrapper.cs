using API.Models;

namespace API.Services;

public interface IRepositoryWrapper
{
    IRepository<Person> People { get; }
    IRepository<Interest> Interests { get; }
    IRepository<Website> Websites { get; }
    ISubRepository<Interest> PersonInterests { get; }
    ISubRepository<Interest> WebsiteInterests { get; }
    ISubRepository<Person> InterestPeople { get; }
    ISubRepository<Person> WebsitePeople { get; }
    ISubRepository<Website> PersonWebsites { get; }
    ISubRepository<Website> InterestWebsites { get; }
}