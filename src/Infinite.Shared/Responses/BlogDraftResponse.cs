namespace Infinite.Shared.Responses;

public class BlogDraftResponse
{
    public string Id { get; set; }
    public string BlogId { get; set; } = Guid.Empty.ToString();
    public DateTime SaveDateTime { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
}