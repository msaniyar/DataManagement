using System;
using System.Threading.Tasks;
using DataManagement.Models;
using DataManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : Controller
    {
        private readonly IDataControl _dataControl;

        public DataController(IDataControl dataControl)
        {
            _dataControl = dataControl;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddPost([FromBody]DataTableView model)
        {

            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var postId = await _dataControl.AddPostAsync(new DataTable{UserName = model.UserName, Password = model.Password, Tree = model.Tree.ToString()});
                if (postId > 0)
                {
                    return Ok(postId);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


    }
}