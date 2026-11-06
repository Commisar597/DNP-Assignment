using APIContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto commentDto);
    Task<CommentResponseDto> UpdateCommentAsync(int id, UpdateCommentDto commentDto);
    Task DeleteCommentAsync(int id);
    Task<CommentResponseDto> GetCommentByIdAsync(int id);
    Task<IEnumerable<CommentResponseDto>> GetCommentsAsync(int? postId = null, int? ownerId = null, int? parentCommentId = null);
    Task<IEnumerable<CommentResponseDto>> GetCommentsByPostIdAsync(int postId);
}