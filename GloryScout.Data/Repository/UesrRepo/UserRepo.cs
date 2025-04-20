using GloryScout.Data.Repository.UesrRepo;
using SpareParts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Repository
{
	public class UserRepo : Repo<User,Guid> , IUserRepo
	{
		private readonly AppDbContext _db;

		public UserRepo(AppDbContext db) : base(db)
		{
			_db = db;
		}

		public async Task<User> GetByEmailAsync(string email)
		{
			return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<User> GetByUsernameAsync(string username)
		{
			return await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
		}

		public async Task<bool> IsEmailUniqueAsync(string email)
		{
			return !await _db.Users.AnyAsync(u => u.Email == email);
		}

		public async Task<bool> IsUsernameUniqueAsync(string username)
		{
			return !await _db.Users.AnyAsync(u => u.UserName == username);
		}

		public async Task<User> GetUserWithVerificationCodesAsync(string email)
		{
			return await _db.Users
				.Include(u => u.VerificationCodes)
				.FirstOrDefaultAsync(u => u.Email == email);
		}
	}
}
