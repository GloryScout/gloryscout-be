using System.ComponentModel.DataAnnotations;

namespace GloryScout.Domain.Dtos.UserProfileDtos;

public class UserProfileDto :IDtos
{
    public Guid Id {get; set;}
    public string Username{get ; set;}
    public string? ProfileDescription{get;set;}

    public List<string> Posts{get; set;}

}