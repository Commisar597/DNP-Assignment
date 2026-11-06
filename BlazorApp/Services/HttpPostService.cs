using APIContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostResponseDto> CreatePostAsync(CreatePostDto postDto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("api/post", postDto);
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create post: {error}");
        }
        return await response.Content.ReadFromJsonAsync<PostResponseDto>() 
               ?? throw new InvalidOperationException("Failed to deserialize created post response.");
    }

    public async Task<PostResponseDto> UpdatePostAsync(int id, UpdatePostDto postDto)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"api/post/{id}", postDto);
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to update post {id}: {error}");
        }
        return await response.Content.ReadFromJsonAsync<PostResponseDto>() 
               ?? throw new InvalidOperationException("Failed to deserialize updated post response.");
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"api/post/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete post {id}: {error}");
        }
    }

    public async Task<PostResponseDto> GetPostByIdAsync(int id)
    {
        return await client.GetFromJsonAsync<PostResponseDto>($"api/post/{id}")
               ?? throw new InvalidOperationException($"Post with ID {id} not found.");
    }

    public async Task<IEnumerable<PostResponseDto>> GetPostsAsync(int? ownerId = null)
    {
        string url = ownerId.HasValue ? $"api/posts?ownerId={ownerId}" : "api/post";
        return await client.GetFromJsonAsync<IEnumerable<PostResponseDto>>(url)
               ?? Enumerable.Empty<PostResponseDto>();
    }
}