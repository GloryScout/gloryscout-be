namespace GloryScout.Domain;
using System.ComponentModel.DataAnnotations;


public class LoginDto : IDtos
{
	

	public LoginDto(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
