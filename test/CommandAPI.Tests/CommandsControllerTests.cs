using System; 
using Xunit; 
using Microsoft.EntityFrameworkCore; 
using CommandAPI.Controllers; 
using CommandAPI.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommandAPI.Tests { 
    public class CommandsControllerTests  : IDisposable
    { 

        DbContextOptionsBuilder<CommandContext> optionsBuilder; 
        CommandContext dbContext; 
        CommandsController controller; 
        public CommandsControllerTests() {
             optionsBuilder = new DbContextOptionsBuilder<CommandContext>(); 
             optionsBuilder.UseInMemoryDatabase("UnitTestInMemBD"); 
             dbContext = new CommandContext(optionsBuilder.Options); 
             controller = new CommandsController(dbContext); 
        } 
        
        public void Dispose() 
        { 
            optionsBuilder = null; 
            foreach (var cmd in dbContext.CommandItems) 
            { 
                dbContext.CommandItems.Remove(cmd); 
            } 
            
            dbContext.SaveChanges(); 
            dbContext.Dispose(); 
            controller = null; 
        }


        [Fact] 
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty() 
        {
                         
             //ACT 
             var result = controller.GetCommandItems(); 
             
             //ASSERT 
             Assert.Empty(result.Value);
        }


        [Fact] 
        public void GetCommandItemsReturnsOneItemWhenDBHasOneObject() 
        { 
            //Arrange 
            var command = new Command { 
                Howto = "Do Somethting", 
                Plataform = "Some Platform", 
                commandLine = "Some Command" 
                }; 
            dbContext.CommandItems.Add(command); 
            dbContext.SaveChanges(); 
            
            //Act 
            var result = controller.GetCommandItems(); 
            
            //Assert 
            Assert.Single(result.Value); 
            
        }

        [Fact]
        public void GetCommandItemsReturnNItemsWhenDBHasNObjects()
        {
            //Arrange
            var command = new Command
            {
                Howto = "Do Somethting",
                Plataform = "Some Platform",
                commandLine = "Some Command"
            };
            var command2 = new Command
            {
                Howto = "Do Somethting",
                Plataform = "Some Platform",
                commandLine = "Some Command"
            };
            dbContext.CommandItems.Add(command);
            dbContext.CommandItems.Add(command2);
            dbContext.SaveChanges();
            //Act
            var result = controller.GetCommandItems();
            //Assert
            Assert.Equal(2, result.Value.Count());
        }


        [Fact]
        public void GetCommandItemsReturnsTheCorrectType()
        {
            //Arrange
            //Act
            var result = controller.GetCommandItems();
            //Assert
            Assert.IsType<ActionResult<IEnumerable<Command>>>(result);
        }



        [Fact]
        public void GetCommandItemReturnsNullResultWhenInvalidID()
        {
            //Arrange
            //DB should be empty, any ID will be invalid
            //Act
            var result = controller.GetCommandItem(0);
            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public  void  GetCommandItemReturns404NotFoundWhenInvalidID()
        {
            //Arrange
            //DB should be empty,any ID will be invalid
            //Act
            var result = controller.GetCommandItem(0);
            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandItemReturnsTheCorrectType()
        {
            //Arrange
            var command = new Command
            {
                Howto = "Do Somethting",
                Plataform = "Some Platform",
                commandLine = "Some Command"
            };
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();
            var cmdId = command.Id;
            //Act
            var result = controller.GetCommandItem(cmdId);
            //Assert
            Assert.IsType<ActionResult<Command>>(result);
        }


        [Fact]
        public void GetCommandItemReturnsTheCorrectResouce()
        {
        //Arrange
            var command = new Command
            {
                Howto = "Do Somethting",
                Plataform = "Some Platform",
                commandLine = "Some Command"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();
            
            var cmdId = command.Id;
            //Act
            var result = controller.GetCommandItem(cmdId);
            //Assert
            Assert.Equal(cmdId, result.Value.Id);
        }


    } 
    
}