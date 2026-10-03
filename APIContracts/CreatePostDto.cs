namespace APIContracts;

public class CreatePostDto
{
    public required string Title { get; set; }
    public required string Body { get; set; }
    public required int OwnerId { get; set; }
}