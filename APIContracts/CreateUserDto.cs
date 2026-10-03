namespace APIContracts;

public class CreateUserDto
{
    public required string Name { get; set; }
    public required string Password { get; set; }
}