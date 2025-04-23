using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloryScout.Data.Models.Entities;
using GloryScout.DTOs.Player;

namespace GloryScout.Domain.Profiles
{
	public interface IMapperProfile
	{
		void CreateMaps(IMapperConfigurationExpression configuration);
    }
	// Mappings/ProfileMapping.cs

    
        public class ProfileMapping : Profile
        {
            public ProfileMapping()
            {
                // Player mappings
                CreateMap<CreatePlayerDto, PlayerProfile>()
                    .ForMember(dest => dest.MediaItems, opt => opt.Ignore());

                CreateMap<PlayerProfile, PlayerProfileDto>();

                
            }
        }
    
}
