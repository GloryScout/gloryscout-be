namespace GloryScout.Domain
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Role { get; set; } = String.Empty;
    }
}