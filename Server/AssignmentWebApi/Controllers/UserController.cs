using APIContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace AssignmentWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository userRepository;
    
    public UserController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    private async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        var users = userRepository.GetMany();
        bool exists = users.Any(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
        if (exists)
        {
            throw new InvalidOperationException($"Username '{userName}' is not free.");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request)
    {
        try 
        {
            await VerifyUserNameIsAvailableAsync(request.Name);
            User user = new User(request.Name, request.Password);
            User createdUser = await userRepository.AddAsync(user); 
            UserResponseDto responseDto = new()
            {
                UserId = createdUser.UserId,
                Name = createdUser.Name,
                UserStatus = createdUser.UserStatus
            };
            return Ok(responseDto);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        try
        {
            var user = await userRepository.GetSingleAsync(id);
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

    [HttpGet]
    public async Task<IActionResult> GetMany([FromQuery] string? name)
    {
        try
        {
            IQueryable<User> usersQuery = userRepository.GetMany();
            
            if (!string.IsNullOrEmpty(name))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            
            var result = usersQuery
                .Select(u => new UserResponseDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    UserStatus = u.UserStatus
                })
                .ToList();
            
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User userToUpdate = new User(request.Name, request.Password)
            {
                UserId = id
            };

            await userRepository.UpdateAsync(userToUpdate);
            return Ok(new UserResponseDto { UserId = id, Name = userToUpdate.Name, UserStatus = userToUpdate.UserStatus });
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


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await userRepository.DeleteAsync(id);
            return NoContent(); 
        }
        catch (KeyNotFoundException e)
        {
            
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
}