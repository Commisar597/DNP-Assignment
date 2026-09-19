namespace Entities;

public class Post
{
    public int PostId { get; set; }
    public int OwnerId { get; set; }
    public string Title { get; set;}
    public string Body { get; set;}

    public Post(string body, string title, int ownerId)
    {
        Body = body;
        Title = title;
        OwnerId = ownerId;
    }
}