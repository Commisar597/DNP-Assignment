using APIContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostResponseDto> CreatePostAsync(CreatePostDto postDto);
    Task<PostResponseDto> UpdatePostAsync(int id, UpdatePostDto postDto);
    Task DeletePostAsync(int id);
    Task<PostResponseDto> GetPostByIdAsync(int id);
    Task<IEnumerable<PostResponseDto>> GetPostsAsync(int? ownerId = null);
}