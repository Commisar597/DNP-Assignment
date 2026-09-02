namespace Entities;

public class Post
{
    public int PostId { get; set; }
    public int OwnerId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int AmountOfDislikes { get; set; }
    public int AmountOfLikes { get; set; }
}