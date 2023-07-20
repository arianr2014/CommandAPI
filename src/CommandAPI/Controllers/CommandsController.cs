using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        //comand
       
        [HttpGet] 
        public ActionResult<IEnumerable<Command>> GetCommandItems() {
            //Probando CI/CD
             return _context.CommandItems; 
        }


        //GET: api/commands/{Id}
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandItem(int id) 
        {
            var commandItem =  _context.CommandItems.Find(id);
            if(commandItem == null)
                return NotFound();  //areyes se acrego para dar soporte a las prueba de test cuando no encuentra

            return  commandItem;
        }

        //POST: api/commands
        [HttpPost]
        public ActionResult<Command> PostCommandItem(Command command)
        {
            _context.CommandItems.Add(command);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return CreatedAtAction("GetCommandItem", new Command{Id = command.Id}, command);
        }


        //PUT: api/commands/{Id}
        [HttpPut("{id}")]
        public ActionResult PutCommandItem(int id, Command command)
        {
            if (id != command.Id)
            {
            return BadRequest();
            }
            _context.Entry(command).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: api/commands/{Id}
        [HttpDelete("{id}")]
        public ActionResult<Command> DeleteCommandItem(int id)
        {
            var commandItem = _context.CommandItems.Find(id);
            if (commandItem == null)
                return NotFound();
            
            _context.CommandItems.Remove(commandItem);
            _context.SaveChanges();
            return commandItem;
        }
   }


}