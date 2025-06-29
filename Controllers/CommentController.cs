using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class CommentController : ControllerBase
    {
        [HttpPost("{id}")]
        public async Task<IActionResult> CreateComment(int id, [FromBody] CommentModel comment, [FromServices] BlogDbContext dbContext)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be logged in to create a comment.");
            }
            if (comment == null || string.IsNullOrWhiteSpace(comment.Content))
            {
                return BadRequest("Comment cannot be null or empty.");
            }
            var blog = await dbContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound("Blog not found.");
            }
            comment.Author = User.Identity?.Name ?? "Anon";
            comment.CreatedAt = DateTime.UtcNow;
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCommentsByBlogId), new { id = blog.Id }, comment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentsByBlogId(int id, [FromServices] BlogDbContext dbContext)
        {
            var comments = await dbContext.Comments
                .Where(c => c.BlogId == id)
                .ToListAsync();
            if (comments == null || comments.Count == 0)
            {
                return NotFound("No comments found for this blog.");
            }
            return Ok(comments);
        }

        [HttpDelete("{id}/{commentId}")]
        public async Task<IActionResult> DeleteComment(int id, int commentId, [FromServices] BlogDbContext dbContext)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be logged in to delete a comment.");
            }
            var comment = await dbContext.Comments.FindAsync(commentId);
            if (comment == null || comment.BlogId != id)
            {
                return NotFound("Comment not found.");
            }
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/{commentId}")]
        public async Task<IActionResult> UpdateComment(int id, int commentId, [FromBody] CommentModel updatedComment, [FromServices] BlogDbContext dbContext)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be logged in to update a comment.");
            }
            if (updatedComment == null || updatedComment.Id != commentId)
            {
                return BadRequest("Comment data is invalid.");
            }
            var existingComment = await dbContext.Comments.FindAsync(commentId);
            if (existingComment == null || existingComment.BlogId != id)
            {
                return NotFound("Comment not found.");
            }
            existingComment.Content = updatedComment.Content;
            existingComment.Author = User.Identity?.Name ?? "Anon";
            dbContext.Comments.Update(existingComment);
            await dbContext.SaveChangesAsync();
            return Ok(existingComment);
        }

        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllComments([FromServices] BlogDbContext dbContext)
        //{
        //    var comments = await dbContext.Comments.ToListAsync();
        //    if (comments == null || comments.Count == 0)
        //    {
        //        return NotFound("No comments found.");
        //    }
        //    return Ok(comments);
        //}
    }
}
