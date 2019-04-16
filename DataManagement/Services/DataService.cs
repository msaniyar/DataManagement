using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataManagement.Models;

namespace DataManagement.Services
{
    public class DataService : IDataService
    {
        private readonly List<string> _datamodel;

        public DataService()
        {
            _datamodel = new List<string>();
        }

        public object AddDataService(DataModel model)
        {
            _datamodel.Add(model.username);
            _datamodel.Add(model.password);
            _datamodel.Add(model.tree);

            return _datamodel;
        }
    }
}
