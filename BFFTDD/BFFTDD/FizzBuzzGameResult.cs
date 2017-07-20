namespace BFFTDD
{
    public class FizzBuzzGameResult
    {
        public FizzBuzzGameResult(int number, string result)
        {
            Number = number;
            Outcome = result;
        }

        public string Outcome { get; set; }
        public int Number { get; set; }
    }
}