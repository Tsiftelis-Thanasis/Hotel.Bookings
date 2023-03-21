using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookings.Application.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<int> Create(T item);

        Task<bool> Edit(T item);

        T GetById(int id);

        List<T> GetList();
    }
}