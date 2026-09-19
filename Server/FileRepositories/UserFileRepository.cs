using Entities;
using RepositoryContracts;
using static System.Text.Json.JsonSerializer;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    private async Task<List<User>> LoadUsersAsync()
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        return Deserialize<List<User>>(usersAsJson) ?? new List<User>();
    }

    private async Task SaveUsersAsync(List<User> users)
    {
        string usersAsJson = Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await LoadUsersAsync();
        int maxId = users.Count > 0 ? users.Max(u => u.UserId) : 0;
        user.UserId = maxId + 1;
        users.Add(user);
        await SaveUsersAsync(users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = await LoadUsersAsync();
        User? existingUser = users.FirstOrDefault(u => u.UserId == user.UserId);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with ID {user.UserId} not found.");
        }
        existingUser.Name = user.Name;
        existingUser.Password = user.Password;
        existingUser.UserStatus = user.UserStatus;
        await SaveUsersAsync(users);
    }

    public async Task DeleteAsync(int id)
    {
        List<User> users = await LoadUsersAsync();
        User? userToRemove = users.FirstOrDefault(u => u.UserId == id);
        if (userToRemove == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        users.Remove(userToRemove);
        await SaveUsersAsync(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        List<User> users = await LoadUsersAsync();
        User? user = users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        return user;
    }

    public IQueryable<User> GetMany()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> users = Deserialize<List<User>>(usersAsJson) ?? new List<User>();
        return users.AsQueryable();
    }
}
