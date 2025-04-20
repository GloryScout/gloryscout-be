using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Profiles
{
	public interface IMapperProfile
	{
		void CreateMaps(IMapperConfigurationExpression configuration);
	}
}
