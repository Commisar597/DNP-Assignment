namespace Entities;

public class Comment
{
    public int CommentId { get; set; }
    public int OwnerId { get; set; }
    public string Body { get; set; }
    public int AmountOfDislikes { get; set; }
    public int AmountOfLikes { get; set; }
}