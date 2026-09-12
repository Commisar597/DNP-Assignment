namespace Entities;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string UserStatus { get; set; }

    public User(string name, string password)
    {
        Name = name;
        Password = password;
        UserStatus = "neutral";
    }
}