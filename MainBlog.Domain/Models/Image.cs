using CSharpFunctionalExtensions;

namespace MainBlog.Domain.Models;

public class Image
{
    private Image(string fileName)
    {
        FileName = fileName;
    }
    public Guid NewsId { get; set; }
    public string FileName { get; set; }

    public Result<Image> Create(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return Result.Failure<Image>($"'{nameof(fileName)}' can not be null or empty!");
        }
        var image = new Image(fileName);
        return Result.Success(image);
    }
}