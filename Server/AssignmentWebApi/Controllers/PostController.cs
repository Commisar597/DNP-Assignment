using APIContracts;
using Entities;
using FileRepositories;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private PostFileRepository postRepository;
    
    public PostController(PostFileRepository postRepository)
    {
        this.postRepository = postRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto request)
    {
        try
        {
            Post post = new Post
            {
                Title = request.Title,
                Body = request.Body,
                OwnerId = request.OwnerId
            };

            Post createdPost = await postRepository.AddAsync(post); 

            PostResponseDto responseDto = new()
            {
                PostId = createdPost.PostId,
                Title = createdPost.Title,
                Body = createdPost.Body,
                OwnerId = createdPost.OwnerId
            };
            return CreatedAtAction(nameof(GetSingle), new { id = responseDto.PostId }, responseDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSingle([FromQuery] int id)
    {
        try
        {
            var post = await postRepository.GetSingleAsync(id);

            PostResponseDto responseDto = new() { PostId = post.PostId, Title = post.Title, Body = post.Body, OwnerId = post.OwnerId };

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
    public async Task<IActionResult> GetMany([FromQuery] string? title, [FromQuery] int? ownerId)
    {
        try
        {
            IQueryable<Post> postsQuery = postRepository.GetMany();
            
            if (!string.IsNullOrEmpty(title))
            {
                postsQuery = postsQuery.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }
            
            if (ownerId.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.OwnerId == ownerId.Value);
            }
            var result = postsQuery.Select(p => new PostResponseDto
            { PostId = p.PostId, Title = p.Title, Body = p.Body, OwnerId = p.OwnerId }).ToList();
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"{e.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post postToUpdate = new Post
            {
                PostId = id,
                Title = request.Title,
                Body = request.Body
            };

            await postRepository.UpdateAsync(postToUpdate);
            return NoContent(); 
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
            await postRepository.DeleteAsync(id);
            return NoContent(); 
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}