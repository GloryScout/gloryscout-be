
using System.ComponentModel.DataAnnotations;

namespace GloryScout.Domain
{
    public class ChangePasswordDto : IDtos
    {
        public ChangePasswordDto(Guid id, string password, string newPassword)
        {
            Id = id;
            Password = password;
            NewPassword = newPassword;
        }

        public Guid Id { get; set; }

        [Required, StringLength(256)]
        public string Password { get; }

        [Required, StringLength(256)]
        public string NewPassword { get; }
    }
}