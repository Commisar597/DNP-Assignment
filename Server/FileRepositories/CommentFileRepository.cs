using RepositoryContracts;

using Entities;
using static System.Text.Json.JsonSerializer;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    private async Task<List<Comment>> LoadCommentsAsync()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        return Deserialize<List<Comment>>(commentsAsJson) ?? new List<Comment>();
    }

    private async Task SaveCommentsAsync(List<Comment> comments)
    {
        string commentsAsJson = Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        int maxId = comments.Count > 0 ? comments.Max(c => c.CommentId) : 0;
        comment.CommentId = maxId + 1;
        comments.Add(comment);
        await SaveCommentsAsync(comments);
        return comment;
    }
    
    public async Task UpdateAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? existingComment = comments.FirstOrDefault(c => c.CommentId == comment.CommentId);
        if (existingComment == null)
        {
            throw new KeyNotFoundException($"Comment with ID {comment.CommentId} not found.");
        }
        existingComment.Body = comment.Body; 
        await SaveCommentsAsync(comments);
    }
    
    public async Task DeleteAsync(int id)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? commentToRemove = comments.FirstOrDefault(c => c.CommentId == id);
        if (commentToRemove == null)
        {
            throw new KeyNotFoundException($"Comment with ID {id} not found.");
        }
        comments.Remove(commentToRemove);
        await SaveCommentsAsync(comments);
    }
    
    public async Task<Comment> GetSingleAsync(int id)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? comment = comments.FirstOrDefault(c => c.CommentId == id);
        if (comment == null)
        {
            throw new KeyNotFoundException($"Comment with ID {id} not found.");
        }

        return comment;
    }

    public IQueryable<Comment> GetMany()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = Deserialize<List<Comment>>(commentsAsJson) ?? new List<Comment>();
        return comments.AsQueryable();
    }
}