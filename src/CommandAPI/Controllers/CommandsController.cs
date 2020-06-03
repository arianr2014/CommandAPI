using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Models;

namespace CommandAPI.Controllers 
{


    [Route("api/[Controller]")]
    [ApiController]
    public class CommandsController :ControllerBase
    {

        private readonly CommandContext _context; 
        public CommandsController(CommandContext context) => _context = context;
        
        /*
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(){
            return new string[] {"this","is","hard","coded"};
        }*/

        [HttpGet] 
        public ActionResult<IEnumerable<Command>> GetCommandItems() {
             return _context.CommandItems; 
        }
    }


}