using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VCargo_Courier.Services
{
 public   interface IUserDataStore<T>
    {
        Task<bool> AddUserAsync(T item);
        Task<bool> UpdateUserAsync(T item);
        Task<bool> DeleteUserAsync(string id);
        Task<T> GetUserAsync(string id);
        Task<IEnumerable<T>> GetUserAsync(bool forceRefresh = false);

    }
}
