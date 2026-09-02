namespace Entities;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public byte UserStatus { get; set; }
    public int AmountOfPosts { get; set; }
    public int AmountOfDislikes { get; set; }
    public int AmountOfComments { get; set; }
    public int AmountOfLikes { get; set; }
}