using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Blog.Models;
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
        public async Task<IActionResult> CreateBlog([FromBody] BlogModel blog, [FromServices] BlogDbContext dbContext)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be logged in to create a blog post.");
            }

            if (blog == null)
            {
                return BadRequest("Blog cannot be null.");
            }
            blog.Author = User.Identity.Name ?? "Anonymous";
            dbContext.Blogs.Add(blog);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlogById), new { id = blog.Id }, blog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogModel updatedBlog, [FromServices] BlogDbContext dbContext)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be logged in to create a blog post.");
            }

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
            existingBlog.Author = User.Identity.Name ?? updatedBlog.Author;
            existingBlog.PublishedDate = updatedBlog.PublishedDate;

            if (updatedBlog.Categories != null)
            {
                existingBlog.Categories = updatedBlog.Categories;
            }

            dbContext.Blogs.Update(existingBlog);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id, [FromServices] BlogDbContext dbContext)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("You must be logged in to delete a blog post.");
            }

            var blog = await dbContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            dbContext.Blogs.Remove(blog);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBlogs([FromQuery] string query, [FromServices] BlogDbContext dbContext)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }
            var blogs = await dbContext.Blogs
                .Where(b => b.Title.Contains(query) || b.Content.Contains(query))
                .ToListAsync();
            if (blogs.Count == 0)
            {
                return NotFound("No blogs found matching the search criteria.");
            }
            return Ok(blogs);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularBlogs([FromServices] BlogDbContext dbContext)
        {
            var blogs = await dbContext.Blogs
                .OrderByDescending(b => b.BlogScore)
                .Take(10)
                .ToListAsync();
            return Ok(blogs);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentBlogs([FromServices] BlogDbContext dbContext)
        {
            var blogs = await dbContext.Blogs
                .OrderByDescending(b => b.PublishedDate)
                .Take(10)
                .ToListAsync();
            return Ok(blogs);
        }

        [HttpGet("by-author/{author}")]
        public async Task<IActionResult> GetBlogsByAuthor(string author, [FromServices] BlogDbContext dbContext)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                return BadRequest("Author name cannot be empty.");
            }
            var blogs = await dbContext.Blogs
                .Where(b => b.Author == author)
                .ToListAsync();
            if (blogs.Count == 0)
            {
                return NotFound($"No blogs found for author {author}.");
            }
            return Ok(blogs);
        }

        [HttpGet("by-category/{category}")]
        public async Task<IActionResult> GetBlogsByCategory(string category, [FromServices] BlogDbContext dbContext)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("Category cannot be empty.");
            }
            var blogs = await dbContext.Blogs
                .Where(b => b.Categories != null && b.Categories.Contains(category))
                .ToListAsync();
            if (blogs.Count == 0)
            {
                return NotFound($"No blogs found in category {category}.");
            }
            return Ok(blogs);
        }
    }
}
