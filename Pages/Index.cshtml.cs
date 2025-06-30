using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blog
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Blog.Models.BlogModel> Blogs { get; set; } = [];

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync($"{Request.Scheme}://{Request.Host}/api/blog");
            if (response.IsSuccessStatusCode)
            {
                var blogs = await response.Content.ReadFromJsonAsync<List<Models.BlogModel>>();
                Blogs = blogs ?? []; 
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to load blogs.");
            }
        }
    }
}
