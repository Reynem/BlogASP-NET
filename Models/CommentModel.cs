namespace Blog.Models
{
    public class CommentModel
    {
        public int? Id { get; set; }
        public int BlogId { get; set; }
        public required string Content { get; set; }
        public string? Author { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
