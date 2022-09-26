using Infinite.Shared.Contracts;

namespace Infinite.Shared.Entities;

public class MarkdownDraft : IEntity<string>
{
    public string Id { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public MarkdownSaveType DraftContentType { get; set; }
    public string BlogPortfolioId { get; set; }
    public DateTime SaveDateTime { get; set; }
    public string MarkdownContent { get; set; }
}