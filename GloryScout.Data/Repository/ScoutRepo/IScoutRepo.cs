using SpareParts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Repository.ScoutRepo
{
	public interface IScoutRepo : IRepo<Scout, Guid>
	{
		Task<Scout> GetByUserIdAsync(Guid userId);
		Task<Scout> GetWithUserDetailsAsync(Guid scoutId);
		Task<IEnumerable<Scout>> GetByClubNameAsync(string clubName);
		Task<IEnumerable<Scout>> GetBySpecializationAsync(string specialization);
	}
}