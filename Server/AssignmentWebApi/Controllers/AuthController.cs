using APIContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace AssignmentWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    
    private IUserRepository userRepository;
    
    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    [HttpGet("{name}")]
    public async Task<IActionResult> FindUserByName(string name)
    {
        try
        {
            var user = await userRepository.FindUserByNameAsync(name);
            var result = new UserResponseDto
            {
                UserId = user.UserId, Name = user.Name, UserStatus = user.UserStatus
            };
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }
}