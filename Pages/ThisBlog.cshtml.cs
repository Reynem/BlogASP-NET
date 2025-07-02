using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blog.Pages
{
    public class ThisBlogModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public ThisBlogModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Models.BlogModel Blog { get; set; } = new();
        public List<Models.CommentModel> Comments { get; set; } = [];

        public async Task OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{Request.Scheme}://{Request.Host}/api/blog/{id}");
            if (response.IsSuccessStatusCode)
            {
                var blog = await response.Content.ReadFromJsonAsync<Models.BlogModel>();
                Blog = blog ?? new();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to load blogs.");
            }

            var commentsResponse = await _httpClient.GetAsync($"{Request.Scheme}://{Request.Host}/api/comment/{id}");
            if (response.IsSuccessStatusCode)
            {
                var comments = await commentsResponse.Content.ReadFromJsonAsync<List<Models.CommentModel>>();
                Comments = comments ?? [];
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to load comments.");
            }
        }
    }
}

