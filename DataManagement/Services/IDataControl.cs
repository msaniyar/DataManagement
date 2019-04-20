using System.Threading.Tasks;
using DataManagement.Models;

namespace DataManagement.Services
{
    /// <summary>
    /// Interface definition for data adding.
    /// </summary>
    public interface IDataControl
    {
        Task<int> AddPostAsync(TreeListTable post);
    }
}