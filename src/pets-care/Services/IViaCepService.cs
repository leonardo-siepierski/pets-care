using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Services
{
    public interface IViaCepService
    {
        Task<object?> FindAdress(string cep);
    }
}