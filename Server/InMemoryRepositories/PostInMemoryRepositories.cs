using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepositories : IPostRepository
{
    public List<Post> posts = [];

    public PostInMemoryRepositories()
    {
        _ = AddAsync(new Post("Magnus just wanted to save the Imperium and warn Father. Why does everyone blame him?", "Magnus did nothing wrong!", 1)).Result;
        _ = AddAsync(new Post("He literally broke the Webway, got Prospero burned, and became a daemon prince. Of course he did.", "Magnus did everything wrong", 1)).Result;
        _ = AddAsync(new Post("Eldar are arrogant xenos, T'au can't melee, and Orks are just having fun. Let's debate.", "Which faction is the absolute worst?", 3)).Result;
        _ = AddAsync(new Post("The Smurfs are cool and all, but their plot armor is getting ridiculous. Hydra Dominatus, boys.", "Are Ultramarines winning again?", 2)).Result;
        _ = AddAsync(new Post("Can someone explain the 41st millennium lore? Do I really need to read 60 Horus Heresy books to understand it?", "Newbie question here", 4)).Result;
        _ = AddAsync(new Post("I've been painting one single Ultramarines space marine for three months. How do I edge highlight without losing my sanity?", "Need help with painting", 3)).Result;
    }
    
    public Task<Post> AddAsync(Post post)
    {
        post.PostId = posts.Any()
            ? posts.Max(p => p.PostId) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }
    
    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.PostId}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.PostId == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.PostId == id);
        if(post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(post);
    }
    
    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}