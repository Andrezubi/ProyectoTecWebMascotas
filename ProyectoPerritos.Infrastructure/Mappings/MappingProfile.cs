using AutoMapper;
using ProyectoMascotas.Api.Data;
using SocialMedia.Core.Entities;


namespace ProyectoMascotas.Infrastructure.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<LostPet, LostPetDTO>();
            CreateMap<LostPetDTO, LostPet>();

            CreateMap<FoundPet, FoundPetDTO>();
            CreateMap<FoundPetDTO, FoundPet>();

            CreateMap<PetPhoto, PetPhotoDTO>();
            CreateMap<PetPhotoDTO, PetPhoto>();

            CreateMap<Match, MatchDTO>();
            CreateMap<MatchDTO, Match>();


            CreateMap<UserDTO, Security>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
