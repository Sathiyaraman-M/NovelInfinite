using Infinite.Shared.Contracts;

namespace Infinite.Shared.Entities;

public class Blog : IEntity<string>
{
    public string Id { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public string Markdown { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastEditedDate { get; set; }
    public Visibility Visibility { get; set; }
}