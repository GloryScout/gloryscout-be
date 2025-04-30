using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.UserProfileDtos
{
    public class EditProfileDto
    {
        public Guid UserId{ get; set; }
		public string? ProfileDescription { get; set; }
	}
}
