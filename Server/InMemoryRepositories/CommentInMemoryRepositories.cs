using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepositories : ICommentRepository
{
    public List<Comment> comments { get; set; }
    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.CommentId = comments.Any()
            ? comments.Max(c => c.CommentId) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }
    
    public Task UpdateAsync(Comment comment)
    {
        Comment? existingPost = comments.SingleOrDefault(c => c.CommentId == comment.CommentId);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.CommentId}' not found");
        }

        comments.Remove(existingPost);
        comments.Add(comment);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        Comment? postToRemove = comments.SingleOrDefault(c => c.CommentId == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        comments.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(p => p.CommentId == id);
        if(comment is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(comment);
    }
    
    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}