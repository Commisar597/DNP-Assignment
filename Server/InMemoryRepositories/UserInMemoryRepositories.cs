using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepositories : IUserRepository
{
    public List<User> users { get; set; }
    
    public Task<User> AddAsync(User user)
    {
        user.UserId = users.Any()
            ? users.Max(p => p.UserId) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }
    
    public Task UpdateAsync(User user)
    {
        User? existingPost = users.SingleOrDefault(u => u.UserId == user.UserId);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{user.UserId}' not found");
        }

        users.Remove(existingPost);
        users.Add(user);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        User? postToRemove = users.SingleOrDefault(c => c.UserId == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        users.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.SingleOrDefault(p => p.UserId == id);
        if(user is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(user);
    }
    
    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}