using Microsoft.Extensions.Hosting;
using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProyectoMascotas.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<FoundPet> FoundPetRepository { get; }
        IBaseRepository<User> UserRepository { get; }
        IBaseRepository<LostPet> LostPetRepository { get; }
        IBaseRepository<Match> MatchRepository { get; }
        IBaseRepository<PetPhoto>PetPhotoRepository { get; }


        IUserRepository UserRepositoryExtra { get; }



        void SaveChanges();
        Task SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        // Nuevos miembros para Dapper
        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();


    }

}
