namespace IL.MainBlog.API;

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
    }
    public async Task WriteError(string errorMessage)
    {
        Console.WriteLine(errorMessage);            
    }
}
