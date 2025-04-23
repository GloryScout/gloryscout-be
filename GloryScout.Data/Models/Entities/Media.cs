// ملف Domain/Entities/Media.cs
using GloryScout.Data.Models.Entities;

namespace GloryScout.Domain.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }

        // Foreign key for PlayerProfile
        public int? PlayerProfileId { get; set; }
        public PlayerProfile PlayerProfile { get; set; }

        // Foreign key for CoachProfile
        public int? CoachProfileId { get; set; }
        public CoachProfile CoachProfile { get; set; }
    }
}