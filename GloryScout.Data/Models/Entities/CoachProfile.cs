using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloryScout.Domain.Entities;

namespace GloryScout.Data.Models.Entities
{
    public class CoachProfile: BaseProfile
    {
        public List<Media> MediaItems { get; set; } = new List<Media>();
    }
}
