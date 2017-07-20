using System;
using AutoMoq;
using Moq;
using Xunit;

namespace BFFTDD.xUnit.Tests
{
    public class FizzBuzzTests
    {
        private readonly FizzBuzzController _testObject;
        private readonly Mock<IGameLogger> _logger;

        public FizzBuzzTests()
        {
            var mocker = new AutoMoqer();
            _testObject = mocker.Create<FizzBuzzController>();
            _logger = mocker.GetMock<IGameLogger>();
        }

        //Rules
            // If number is divisible by 3 - return Fizz
            // If number is divisible by 5 - return Buzz
            // If number is divisible by 3 and 5 - return FizzBuzz

        [Theory]
        //Happy Path
        [InlineData(3, "Fizz")]
        [InlineData(5, "Buzz")]
        [InlineData(15, "FizzBuzz")]
        //Sad Path
        [InlineData(101, null)]
        public void Play_Given_Number_Returns_Expected(int number, string expected)
        {
            //Arrange
            _logger.Setup(x => x.LogGame(It.IsAny<FizzBuzzGameResult>()))
                .Returns(true);

            //Act
            var result = _testObject.Play(number);

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Play_Logs_Each_Game()
        {
            //Arrange
            FizzBuzzGameResult gameResultCallback = null;
            const int expectedNumber = 5;
            const string expectedOutcome = "Buzz";

            //Callbacks run the given action when LogGame is called
            _logger.Setup(x => x.LogGame(It.IsAny<FizzBuzzGameResult>()))
                .Callback<FizzBuzzGameResult>(r => gameResultCallback = r)
                .Returns(true); 

            //Act
            //We don't care about the outcome itself, just that it was logged
            var result = _testObject.Play(expectedNumber);

            //Assert
            Assert.NotNull(gameResultCallback);
            Assert.Equal(expectedNumber, gameResultCallback.Number);
            Assert.Equal(expectedOutcome, gameResultCallback.Outcome);

            //This would be valid if we were only logging primitive types
            //Sure we could put complex objects in here but then we would
            //have to override Equals on the result object which is bad form for this case
            //_logger.Verify(x => x.LogGame("5,Buzz"), "Wrong game logged or not logged");
        }

        [Fact]
        public void Play_Throws_Exception_If_LogGame_Fails()
        {
            //Arrange
            _logger.Setup(x => x.LogGame(It.IsAny<FizzBuzzGameResult>()))
                .Returns(false);

            //Act
            var ex = Assert.Throws<Exception>(() => _testObject.Play(5));

            //Assert
            Assert.NotNull(ex);
            Assert.Equal("Something went wrong with logging the game", ex.Message);

        }
    }
}