using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chat.IServices;
using Chat.Models;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public  IStudentServices _studentService;

        public ValuesController(IStudentServices StudentServices)
        {
            _studentService = StudentServices;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            Student sutdent1 = new Student()
            {
                Age = 18,
                Name = "Jackson",
                IsDelete = false
            };
            _studentService.Create(sutdent1);
            return "done";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
