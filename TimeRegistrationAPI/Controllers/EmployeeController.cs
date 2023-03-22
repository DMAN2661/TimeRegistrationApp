using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace TimeRegistrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        [EnableCors("MyPolicy")]
        [HttpGet]

        public Project GetProject() 
        {
            return;
        
        }

    }

    public class Project
    {
        public int ID { get; set; }
    }
}