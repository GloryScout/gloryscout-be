using AutoMapper;
using GloryScout.API.Services.PlayerServiceandCoach;
using GloryScout.Data;
using GloryScout.Data.Models.Entities;
using GloryScout.DTOs.Player;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GloryScout.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlayerService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

      
        public async Task<PlayerProfileDto> GetProfile(string username)
        {
            var profile = await _context.PlayerProfiles
                .Include(p => p.MediaItems)
                .FirstOrDefaultAsync(p => p.Username == username);

            if (profile == null)
            {
                throw new KeyNotFoundException("Player profile not found");
            }

            return _mapper.Map<PlayerProfileDto>(profile);
        }

       
        public async Task<PlayerProfileDto> CreateProfile(CreatePlayerDto playerDto)
        {
           
            if (await _context.PlayerProfiles.AnyAsync(p => p.Username == playerDto.Username))
            {
                throw new ArgumentException("Username already exists");
            }

       
            var profile = _mapper.Map<PlayerProfile>(playerDto);
            profile.FollowersCount = 0;
            profile.FollowingCount = 0;

            
            _context.PlayerProfiles.Add(profile);
            await _context.SaveChangesAsync();

           
            return _mapper.Map<PlayerProfileDto>(profile);
        }
    }
}