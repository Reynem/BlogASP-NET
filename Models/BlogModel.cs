namespace Blog.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Author { get; set; }
        public string[]? Categories { get; set; }
        public int BlogScore { get; set; } = 0;
        public DateTime PublishedDate { get; set; }
    }
}
