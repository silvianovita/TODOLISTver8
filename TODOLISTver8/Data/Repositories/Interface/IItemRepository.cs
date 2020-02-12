using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interface
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemVM>> Get();
    }
}
