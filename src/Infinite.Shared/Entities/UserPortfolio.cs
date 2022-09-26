using Infinite.Shared.Contracts;

namespace Infinite.Shared.Entities;

public class UserPortfolio : IEntity<string>
{
    public string Id { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    
    public string PortfolioMarkdown { get; set; }
}