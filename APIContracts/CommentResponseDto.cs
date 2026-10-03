namespace APIContracts;

public class CommentResponseDto
{
    public int CommentId { get; set; }
    public string Body { get; set; } = string.Empty;
    public int PostId { get; set; }
    public int OwnerId { get; set; }
}