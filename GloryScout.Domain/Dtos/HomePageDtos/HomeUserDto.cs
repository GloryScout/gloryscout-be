using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.HomePageDtos
{
    public class HomeUserDto :IDtos
    {
		public string Nationality { get; set; }
		public string UserType { get; set; }
		public string ProfilePhoto { get; set; }
		public string ProfileDescription { get; set; }

	}
}
