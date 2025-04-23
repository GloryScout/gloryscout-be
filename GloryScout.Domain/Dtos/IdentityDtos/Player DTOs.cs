// DTOs/Player/CreatePlayerDto.cs
using System.ComponentModel.DataAnnotations;

namespace GloryScout.DTOs.Player
{
    public class CreatePlayerDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; }

        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string ProfilePictureUrl { get; set; }
    }
}

// DTOs/Player/PlayerProfileDto.cs
namespace GloryScout.DTOs.Player
{
    public class PlayerProfileDto
    {
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Position { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public List<MediaDto> MediaItems { get; set; }
    }
}