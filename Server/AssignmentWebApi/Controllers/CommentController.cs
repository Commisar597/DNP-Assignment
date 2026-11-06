using APIContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace AssignmentWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    
    public CommentController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto request)
    {
        try
        {
            Comment comment = new Comment
            {
                Body = request.Body,
                PostId = request.PostId,
                OwnerId = request.OwnerId,
                ParentCommentId = request.ParentCommentId
            };
            Comment createdComment = await commentRepository.AddAsync(comment); 
            CommentResponseDto responseDto = new()
            {
                CommentId = createdComment.CommentId,
                Body = createdComment.Body,
                PostId = createdComment.PostId,
                OwnerId = createdComment.OwnerId,
                ParentCommentId = createdComment.ParentCommentId,
            };

            return CreatedAtAction(nameof(GetSingle), new { id = responseDto.CommentId }, responseDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        try
        {
            var comment = await commentRepository.GetSingleAsync(id);
            CommentResponseDto responseDto = new()
            {
                CommentId = comment.CommentId,
                Body = comment.Body,
                PostId = comment.PostId,
                ParentCommentId = comment.ParentCommentId,
                OwnerId = comment.OwnerId
            };

            return Ok(responseDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMany([FromQuery] int? postId, [FromQuery] int? ownerId, [FromQuery] int? parentCommentId)
    {
        try
        {
            IQueryable<Comment> commentsQuery = commentRepository.GetMany();
        
            if (postId.HasValue)
            {
                commentsQuery = commentsQuery.Where(c => c.PostId == postId.Value);
            }
        
            if (ownerId.HasValue)
            {
                commentsQuery = commentsQuery.Where(c => c.OwnerId == ownerId.Value);
            }
        
            if (parentCommentId.HasValue)
            {
                commentsQuery = commentsQuery.Where(c => c.ParentCommentId == parentCommentId.Value);
            }
        
            var result = commentsQuery.Select(c => new CommentResponseDto
            {
                CommentId = c.CommentId,
                Body = c.Body,
                PostId = c.PostId,
                OwnerId = c.OwnerId,
                ParentCommentId = c.ParentCommentId
            }).ToList();

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment commentToUpdate = new Comment
            {
                CommentId = id,
                Body = request.Body
            };

            await commentRepository.UpdateAsync(commentToUpdate);
            Comment updated = await commentRepository.GetSingleAsync(id);
            return Ok(new CommentResponseDto {CommentId = updated.CommentId, Body = updated.Body, PostId = updated.PostId, OwnerId = updated.OwnerId, ParentCommentId = updated.ParentCommentId});
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await commentRepository.DeleteAsync(id);
            return NoContent(); 
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
}
