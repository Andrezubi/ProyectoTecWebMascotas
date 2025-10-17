using AutoMapper;
using ProyectoMascotas.Api.Data;


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
        }
    }
}
