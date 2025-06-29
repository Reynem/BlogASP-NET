namespace Blog.ViewModels
{
    public class ProfileViewModel
    {
        public required string UserName;
        public DateTime? BirthDate { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
