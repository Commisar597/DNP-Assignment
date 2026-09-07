namespace Entities;

public class Comment
{
    public int CommentId { get; set; }
    public int OwnerId { get; }
    public string Body { get; }
    public int? AmountOfDislikes { get; set; }
    public int? AmountOfLikes { get; set; }
    
    public Comment(string body, int commentId, int ownerId)
    {
        Body = body;
        CommentId = commentId;
        OwnerId = ownerId;
    }
}