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


    //OBJECT COUNT INCREMENTS BY 1
    [Fact]
    public void PostCommandItemObjectCountIncrementWhenValidObject()
    {
        //Arrange
        var command = new Command
        {
            Howto = "Do Somethting",
            Plataform = "Some Platform",
            commandLine = "Some Command"
        };
        
        var oldCount = dbContext.CommandItems.Count();
        //Act
        var result = controller.PostCommandItem(command);
        //Assert
        Assert.Equal(oldCount + 1, dbContext.CommandItems.Count());
    }

    [Fact]
    public void PostCommandItemReturns201CreatedWhenValidObject()
    {
        //Arrange
        var command = new Command
        {
            Howto = "Do Somethting",
            Plataform = "Some Platform",
            commandLine = "Some Command"
        };
        //Act
        var result = controller.PostCommandItem(command);
        //Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public void PutCommandItem_AttributeUpdated_WhenValidObject()
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
        command.Howto = "UPDATED";
        //Act
        controller.PutCommandItem(cmdId, command);
        var result = dbContext.CommandItems.Find(cmdId);
        //Assert
        Assert.Equal(command.Howto, result.Howto);
    }


    [Fact]
    public void PutCommandItem_Returns204_WhenValidObject()
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
        command.Howto = "UPDATED";
        //Act
        var result = controller.PutCommandItem(cmdId, command);
        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void PutCommandItem_Returns400_WhenInvalidObject()
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
        var cmdId = command.Id+1;
        command.Howto = "UPDATED";
        //Act
        var result = controller.PutCommandItem(cmdId, command);
        //Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void PutCommandItem_AttributeUnchanged_WhenInvalidObject()
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
        var command2 = new Command
        {
            Id = command.Id,
            Howto = "UPDATED",
            Plataform = "UPDATED",
            commandLine = "UPDATED"
        };
        //Act
        controller.PutCommandItem(command.Id + 1, command2);
        var result = dbContext.CommandItems.Find(command.Id);
        //Assert
        Assert.Equal(command.Howto, result.Howto);
    }
    [Fact]
    public void DeleteCommandItem_ObjectsDecrement_WhenValidObjectID()
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
        var objCount = dbContext.CommandItems.Count();
        //Act
        controller.DeleteCommandItem(cmdId);
        //Assert
        Assert.Equal(objCount-1, dbContext.CommandItems.Count());
    }

    [Fact]
    public void DeleteCommandItem_Returns200OK_WhenValidObjectID()
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
        var result = controller.DeleteCommandItem(cmdId);
        //Assert
        Assert.Null(result.Result);
    }


    [Fact]
    public void DeleteCommandItem_Returns404NotFound_WhenValidObjectID()
    {
        //Arrange
        //Act
        var result = controller.DeleteCommandItem(-1);
        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
       
    }

    [Fact]
    public void DeleteCommandItem_ObjectCountNotDecremented_WhenValidObjectID()
    {
        //Arrange
        var command = new Command
        {
            Howto = "Arian Reyes",
            Plataform = "Arian Reyes",
            commandLine = "Arian Reyes"
        };
        dbContext.CommandItems.Add(command);
        dbContext.SaveChanges();
        var cmdId = command.Id;
        var objCount = dbContext.CommandItems.Count();
        //Act
        var result = controller.DeleteCommandItem(cmdId+1);
        //Assert
        Assert.Equal(objCount, dbContext.CommandItems.Count());
    }


  } 
    
}