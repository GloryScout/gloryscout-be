using SpareParts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Repository.UesrRepo
{
	public interface IUserRepo : IRepo<User,Guid>
	{
		Task<User> GetByEmailAsync(string email);
		Task<User> GetByUsernameAsync(string username);
		Task<bool> IsEmailUniqueAsync(string email);
		Task<bool> IsUsernameUniqueAsync(string username);
		Task<User> GetUserWithVerificationCodesAsync(string email);
	}
}
