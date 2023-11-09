namespace Blog
{
    public interface ILogger
    {
        public void WriteEvent(string eventMessage);
        public void WriteError(string errorMessage);
    }

    public class Logger : ILogger
    {
        public void WriteEvent(string eventMessage)
        {
            Console.WriteLine(eventMessage);
        }
        public void WriteError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }
    }
}
