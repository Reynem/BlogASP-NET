using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class BlogController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBlogs([FromServices] BlogDbContext dbContext)
        {
           var blogs = await dbContext.Blogs.ToListAsync();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id, [FromServices] BlogDbContext dbContext)
        {
            var blog = await dbContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] Blog blog, [FromServices] BlogDbContext dbContext)
        {
            if (blog == null)
            {
                return BadRequest("Blog cannot be null.");
            }
            dbContext.Blogs.Add(blog);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlogById), new { id = blog.Id }, blog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] Blog updatedBlog, [FromServices] BlogDbContext dbContext)
        {
            if (updatedBlog == null || updatedBlog.Id != id)
            {
                return BadRequest("Blog data is invalid.");
            }
            var existingBlog = await dbContext.Blogs.FindAsync(id);
            if (existingBlog == null)
            {
                return NotFound();
            }
            existingBlog.Title = updatedBlog.Title;
            existingBlog.Content = updatedBlog.Content;
            existingBlog.Author = updatedBlog.Author;
            existingBlog.PublishedDate = updatedBlog.PublishedDate;
            dbContext.Blogs.Update(existingBlog);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id, [FromServices] BlogDbContext dbContext)
        {
            var blog = await dbContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            dbContext.Blogs.Remove(blog);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
