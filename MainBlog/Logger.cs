using MainBlog.BL;

namespace MainBlog
{
    public interface ILogger
    {
        public Task WriteEvent(string eventMessage);
        public Task WriteError(string errorMessage);
    }

    public class Logger : ILogger
    {
        IWebHostEnvironment _env;
        public Logger(IWebHostEnvironment env) { _env = env; }
        public async Task WriteEvent(string eventMessage)
        {
            Console.WriteLine(eventMessage);
            await WriteActions.CreateLogFolder_File(_env, "Logger", $"{eventMessage}");
        }
        public async Task WriteError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            await WriteActions.CreateLogFolder_File(_env, "ErrorsLogger", $"{errorMessage}");
        }
    }
}
