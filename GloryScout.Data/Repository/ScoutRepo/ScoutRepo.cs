using SpareParts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Repository.ScoutRepo
{
	public class ScoutRepo : Repo<Scout, Guid>, IScoutRepo
	{
		private readonly AppDbContext _db;

		public ScoutRepo(AppDbContext db) : base(db)
		{
			_db = db;
		}

		public async Task<Scout> GetByUserIdAsync(Guid userId)
		{
			return await _db.Set<Scout>().FirstOrDefaultAsync(s => s.UserId == userId);
		}

		public async Task<Scout> GetWithUserDetailsAsync(Guid scoutId)
		{
			return await _db.Set<Scout>()
				.Include(s => s.User)
				.FirstOrDefaultAsync(s => s.Id == scoutId);
		}

		public async Task<IEnumerable<Scout>> GetByClubNameAsync(string clubName)
		{
			return await _db.Set<Scout>()
				.Include(s => s.User)
				.Where(s => s.ClubName == clubName)
				.ToListAsync();
		}

		public async Task<IEnumerable<Scout>> GetBySpecializationAsync(string specialization)
		{
			return await _db.Set<Scout>()
				.Include(s => s.User)
				.Where(s => s.Specialization == specialization)
				.ToListAsync();
		}

		// returns all coaches 
		public async Task<IEnumerable<Scout>> GetAllAsync()
		{
			return await _db.Scouts.ToListAsync();
		}
	}
}