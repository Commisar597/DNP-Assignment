using APIContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserResponseDto> CreateUserAsync(CreateUserDto userDto);
    Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto userDto);
    Task DeleteUserAsync(int id);
    Task<UserResponseDto> GetUserByIdAsync(int id);
    Task<IEnumerable<UserResponseDto>> GetUsersAsync();
}