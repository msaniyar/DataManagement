using System.Threading.Tasks;
using DataManagement.Models;

namespace DataManagement.Services
{
    public class DataControl : IDataControl
    {
        DataManagementContext _db;

        public DataControl(DataManagementContext db)
        {
            _db = db;
        }
        public async Task<int> AddPostAsync(DataTable post)
        {
            if (_db != null)
            {
                await _db.DataTable.AddAsync(post);
                await _db.SaveChangesAsync();

                return post.GetHashCode();
            }

            return 0;
        }
    }
}