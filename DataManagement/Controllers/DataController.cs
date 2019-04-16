using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataManagement.Models;
using DataManagement.Services;
using Microsoft.AspNetCore.Mvc;
using static DataManagement.Models.DataModel;

namespace DataManagement.Controllers
{
    [Route("v1/")]
    [ApiController]
    public class DataController : ControllerBase
    {

        private readonly IDataService _service;

        public DataController(IDataService service)
        {
            _service = service;
        }

        // POST: api/Todo
        [HttpPost]
        public ActionResult<DataModel> AddData(DataModel model)
        {
            var dataItems = _service.AddDataService(model);
            return Ok();
        }
    }
}
