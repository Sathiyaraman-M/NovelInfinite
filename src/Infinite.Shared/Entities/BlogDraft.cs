using Infinite.Shared.Contracts;

namespace Infinite.Shared.Entities;

public class BlogDraft : IEntity<string>
{
    public string Id { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public string BlogId { get; set; } = Guid.Empty.ToString();
    public DateTime SaveDateTime { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public string MarkdownContent { get; set; }
}