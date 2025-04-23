using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloryScout.Domain.Entities;

namespace GloryScout.Data.Models.Entities
{
    public class PlayerProfile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public List<Media> MediaItems { get; set; } = new List<Media>();
    }
}
