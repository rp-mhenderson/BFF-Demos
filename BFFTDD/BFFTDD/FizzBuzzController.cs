using System;

namespace BFFTDD
{
    public class FizzBuzzController
    {
        private readonly IGameLogger _logger;

        public FizzBuzzController(IGameLogger logger)
        {
            _logger = logger;
        }

        public string Play(int number)
        {
            string result = null;
            var isBuzz = number % 5 == 0;
            var isFizz = number % 3 == 0;

            if (isFizz) result = "Fizz";
            if (isBuzz) result += "Buzz";

            var logSuccess = _logger.LogGame(new FizzBuzzGameResult(number, result));

            if(!logSuccess)
                throw new Exception("Something went wrong with logging the game");

            return result;
        }
    }
}
