using APIContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto commentDto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("api/comment", commentDto);
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create comment: {error}");
        }
        return await response.Content.ReadFromJsonAsync<CommentResponseDto>() 
               ?? throw new InvalidOperationException("Failed to deserialize created comment response.");
    }

    public async Task<CommentResponseDto> UpdateCommentAsync(int id, UpdateCommentDto commentDto)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"api/comment/{id}", commentDto);
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to update comment {id}: {error}");
        }
        return await response.Content.ReadFromJsonAsync<CommentResponseDto>() 
               ?? throw new InvalidOperationException("Failed to deserialize updated comment response.");
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"api/comment/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete comment {id}: {error}");
        }
    }

    public async Task<CommentResponseDto> GetCommentByIdAsync(int id)
    {
        return await client.GetFromJsonAsync<CommentResponseDto>($"api/comment/{id}")
               ?? throw new InvalidOperationException($"Comment with ID {id} not found.");
    }

    public async Task<IEnumerable<CommentResponseDto>> GetCommentsAsync(int? postId = null, int? ownerId = null, int? parentCommentId = null)
    {
        List<string> query = new();
        if (postId.HasValue) query.Add($"postId={postId}");
        if (ownerId.HasValue) query.Add($"ownerId={ownerId}");
        if (parentCommentId.HasValue) query.Add($"parentCommentId={parentCommentId}");
        
        string url = query.Count > 0 ? "api/comment?" + string.Join("&", query) : "api/comment";

        return await client.GetFromJsonAsync<IEnumerable<CommentResponseDto>>(url)
               ?? Enumerable.Empty<CommentResponseDto>();
    }

    public async Task<IEnumerable<CommentResponseDto>> GetCommentsByPostIdAsync(int postId)
    {
        return await GetCommentsAsync(postId: postId);
    }
}