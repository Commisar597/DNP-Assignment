namespace Entities;

public class Comment
{
    public int CommentId { get; set; }
    public int PostId { get; set; }
    public int OwnerId { get; set; }
    public string Body { get; set; }

    public Comment(string body, int postId, int ownerId)
    {
        Body = body;
        PostId = postId;
        OwnerId = ownerId;
    }
}