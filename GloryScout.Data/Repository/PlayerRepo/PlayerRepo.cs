using SpareParts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Repository.PlayerRepo
{
	public class PlayerRepo : Repo<Player, Guid>, IPlayerRepo
	{
		private readonly AppDbContext _db;

		public PlayerRepo(AppDbContext db) : base(db)
		{
			_db = db;
		}

		public async Task<Player> GetByUserIdAsync(Guid userId)
		{
			return await _db.Set<Player>().FirstOrDefaultAsync(p => p.UserId == userId);
		}

		public async Task<Player> GetWithUserDetailsAsync(Guid playerId)
		{
			return await _db.Set<Player>()
				.Include(p => p.User)
				.FirstOrDefaultAsync(p => p.Id == playerId);
		}

		public async Task<IEnumerable<Player>> GetByPositionAsync(string position)
		{
			return await _db.Set<Player>()
				.Include(p => p.User)
				.Where(p => p.Position == position)
				.ToListAsync();
		}

		public async Task<IEnumerable<Player>> GetByAgeRangeAsync(int minAge, int maxAge)
		{
			return await _db.Set<Player>()
				.Include(p => p.User)
				.Where(p => p.Age >= minAge && p.Age <= maxAge)
				.ToListAsync();
		}

        // returns all players 
        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await _db.Players.ToListAsync();
        }
    }
}