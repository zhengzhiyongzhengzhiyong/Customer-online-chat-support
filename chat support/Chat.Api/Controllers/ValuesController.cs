using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chat.IServices;
using Chat.Models;
using Microsoft.Extensions.Logging;
using Chat.Common;
using NLog;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public  IStudentServices _studentService;
        private readonly ILogger<ValuesController> _logger;
        private readonly Logger logger = LogManager.GetLogger("dblog");
        public ValuesController(IStudentServices StudentServices, ILogger<ValuesController> logger)
        {
            _studentService = StudentServices;
            _logger = logger;
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
            logger.Debug("sutdent add successfully");
            _logger.LogDebug("sutdent add successfully");
            return "done";
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
          
            var number =  12 / "0".ToInt_V();
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
