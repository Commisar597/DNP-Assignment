namespace APIContracts;

public class PostResponseDto
{
    public int PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int OwnerId { get; set; }
}