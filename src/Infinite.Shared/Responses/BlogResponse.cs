namespace Infinite.Shared.Responses;

public class BlogResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastEditedDate { get; set; }
    public Visibility Visibility { get; set; }
}