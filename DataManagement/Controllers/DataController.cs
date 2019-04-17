﻿using System;
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
        private readonly IDataControl _datacontrol;

        public DataController(IDataControl dataControl)
        {
            _datacontrol = dataControl;
        }

        [HttpPost]
        [Route("AddPost")]
        public async Task<IActionResult> AddPost([FromBody]DataTable model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var postId = await _datacontrol.AddPostAsync(model);
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

            return BadRequest();
        }


    }
}