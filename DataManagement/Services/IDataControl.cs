using System.Threading.Tasks;
using DataManagement.Models;

namespace DataManagement.Services
{
    public interface IDataControl
    {
        Task<int> AddPostAsync(DataTable post);
    }
}