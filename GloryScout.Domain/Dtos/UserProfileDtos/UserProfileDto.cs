using System.ComponentModel.DataAnnotations;

namespace GloryScout.Domain.Dtos.UserProfileDtos;

public class UserProfileDto :IDtos
{
    public Guid Id {get; set;}
    public string Username{get ; set;}
    public string? ProfilePhoto { get; set; }
	public string? ProfileDescription{get;set;}

    public ICollection<PostDto> Posts{get; set;}

    public int FollowersCount { get; set; }
	public int FollowingCount { get; set; }


}