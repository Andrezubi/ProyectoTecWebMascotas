using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces.ServiceInterfaces
{
    public interface IPasswordService
    {
        string Hash(string password);

        bool Check(string hash, string password);
    }

}
