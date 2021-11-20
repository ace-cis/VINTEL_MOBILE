using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VCargo_Courier.Services
{
 public  interface IReasonDataStore<T>
    {
        Task<bool> AddReasonAsync(T item);
        Task<bool> UpdateReasonAsync(T item);
        Task<bool> DeleteReasonAsync(string id);
        Task<T> GetReasonAsync(string id);
        Task<IEnumerable<T>> GetReasonsAsync(bool forceRefresh = false);
    }
}
