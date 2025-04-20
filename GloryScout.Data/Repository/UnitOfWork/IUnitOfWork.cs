


using GloryScout.Data.Repository.PlayerRepo;
using GloryScout.Data.Repository.ScoutRepo;
using GloryScout.Data.Repository.UesrRepo;

namespace GloryScout.Data
{
	public interface IUnitOfWork : IDisposable
	{
		IUserRepo User { get; }
		IPlayerRepo Player { get; }
		IScoutRepo Scout { get; }
		//IPostRepo Post { get; }
		//ICommentRepo Comment { get; }
		//ILikeRepo Like { get; }
		//IApplicationRepo Application { get; }
		//IVerificationCodeRepo VerificationCode { get; }

		int Save();
	}
}