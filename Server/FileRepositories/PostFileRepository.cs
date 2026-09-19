using RepositoryContracts;

using Entities;
using static System.Text.Json.JsonSerializer;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    
    private readonly string filePath = "post.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    private async Task<List<Post>> LoadPostsAsync()
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        return Deserialize<List<Post>>(postAsJson) ?? new List<Post>();
    }

    private async Task SavePostsAsync(List<Post> posts)
    {
        string postsAsJson = Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();
        int maxId = posts.Count > 0 ? posts.Max(c => c.PostId) : 0;
        post.PostId = maxId + 1;
        posts.Add(post);
        await SavePostsAsync(posts);
        return post;
    }
    
    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? existingPost = posts.FirstOrDefault(c => c.PostId == post.PostId);
        if (existingPost == null)
        {
            throw new KeyNotFoundException($"Post with ID {post.PostId} not found.");
        }
        existingPost.Body = post.Body; 
        await SavePostsAsync(posts);
    }
    
    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? postToRemove = posts.FirstOrDefault(c => c.PostId == id);
        if (postToRemove == null)
        {
            throw new KeyNotFoundException($"post with ID {id} not found.");
        }
        posts.Remove(postToRemove);
        await SavePostsAsync(posts);
    }
    
    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? post = posts.FirstOrDefault(c => c.PostId == id);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        return post;
    }

    public IQueryable<Post> GetMany()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = Deserialize<List<Post>>(postsAsJson) ?? new List<Post>();
        return posts.AsQueryable();
    }
}