using SpareParts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Repository.PlayerRepo
{
	public interface IPlayerRepo : IRepo<Player,Guid>
	{
		Task<Player> GetByUserIdAsync(Guid userId);
		Task<Player> GetWithUserDetailsAsync(Guid playerId);
		Task<IEnumerable<Player>> GetByPositionAsync(string position);
		Task<IEnumerable<Player>> GetByAgeRangeAsync(int minAge, int maxAge);
		Task<IEnumerable<Player>> GetAllAsync();
	}
}

