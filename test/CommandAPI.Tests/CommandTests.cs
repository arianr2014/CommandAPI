using System; 
using Xunit; 
using CommandAPI.Models; 

namespace CommandAPI.Tests { 
    public class CommandTests : IDisposable
    {
        Command testCommand;
        public CommandTests() { 
            
             testCommand = new Command() { 
                Howto = "Do something awesome", 
                Plataform = "xUnit", 
                commandLine = "dotnet test" 
            };
            
        }
        
        public void Dispose() 
        { 
            testCommand = null; 
        }
        [Fact] 
        public void CanChangeHowTo() 
        { 
            //Arrange
            
            //Act 
            testCommand.Howto = "Execute Unit Tests"; 
            
            //Assert 
            Assert.Equal("Execute Unit Tests", testCommand.Howto);

        } 

        [Fact] 
        public void CanChangePlataForm() 
        { 
            //Arrange
           

            //Act 
            testCommand.Plataform = "Execute Unit TestsPlataform"; 
            
            //Assert 
            Assert.Equal("Execute Unit TestsPlataform", testCommand.Plataform);

        } 
        [Fact] 
        public void CanChangePlatacommandLine() 
        { 
            //Arrange
         

            //Act 
            testCommand.commandLine = "Execute Unit TestscommandLine"; 
            
            //Assert 
            Assert.Equal("Execute Unit TestscommandLine", testCommand.commandLine);

        } 

    } 
}