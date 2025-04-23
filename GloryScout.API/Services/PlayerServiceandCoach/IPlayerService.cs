using GloryScout.DTOs.Player;

namespace GloryScout.API.Services.PlayerServiceandCoach
{
    public interface IPlayerService
    {

        Task<PlayerProfileDto> GetProfile(string username);
        Task<PlayerProfileDto> CreateProfile(CreatePlayerDto playerDto);
    }
}
