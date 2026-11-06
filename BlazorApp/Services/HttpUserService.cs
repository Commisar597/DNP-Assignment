using APIContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto userDto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("api/user", userDto);
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create user: {error}");
        }
        return await response.Content.ReadFromJsonAsync<UserResponseDto>() 
               ?? throw new InvalidOperationException("Failed to deserialize created user response.");
    }

    public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto userDto)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"api/user/{id}", userDto);
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to update user {id}: {error}");
        }
        return await response.Content.ReadFromJsonAsync<UserResponseDto>() 
               ?? throw new InvalidOperationException("Failed to deserialize updated user response.");
    }

    public async Task DeleteUserAsync(int id)
    {
        HttpResponseMessage response = id <= 0 ? throw new ArgumentException("Invalid ID") : await client.DeleteAsync($"api/user/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete user {id}: {error}");
        }
    }

    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        return await client.GetFromJsonAsync<UserResponseDto>($"api/user/{id}")
               ?? throw new InvalidOperationException($"User with ID {id} not found.");
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsersAsync()
    {
        return await client.GetFromJsonAsync<IEnumerable<UserResponseDto>>("api/user") 
               ?? Enumerable.Empty<UserResponseDto>();
    }
}