

using GloryScout.Data;
using GloryScout.Data.Repository;
using GloryScout.Data.Repository.PlayerRepo;
using GloryScout.Data.Repository.ScoutRepo;
using GloryScout.Data.Repository.UesrRepo;


namespace SpareParts.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;

		public IUserRepo User { get; private set; }
		public IPlayerRepo Player { get; private set; }
		public IScoutRepo Scout { get; private set; }
		//public IPostRepo Post { get; private set; }
		//public ICommentRepo Comment { get; private set; }
		//public ILikeRepo Like { get; private set; }
		//public IApplicationRepo Application { get; private set; }
		//public IVerificationCodeRepo VerificationCode { get; private set; }

		public UnitOfWork(AppDbContext context)
		{
			_context = context;
			User = new UserRepo(_context);
			Player = new PlayerRepo(_context);
			Scout = new ScoutRepo(_context);
			//Post = new PostRepo(_context);
			//Comment = new CommentRepo(_context);
			//Like = new LikeRepo(_context);
			//Application = new ApplicationRepo(_context);
			//VerificationCode = new VerificationCodeRepo(_context);
		}

		public void Dispose()
		{
			_context.Dispose();
		}

		public int Save()
		{
			return _context.SaveChanges();
		}
	}
}