using CSharpFunctionalExtensions;
using static System.Net.Mime.MediaTypeNames;

namespace MainBlog.Domain.Models;
/// <summary>
/// Класс, отвечающий за новость в ленте
/// </summary>
public class News
{
    private const int MAX_TITLE_LENGTH = 100;
    public Guid Id { get; set; }
    public string Title { get; } = string.Empty;
    public string TextData { get; } = string.Empty;
    public DateTime CreateDate { get; set; }

    //счётчик просмотров новости
    public int Views { get; private set; } = 0;

    //превьюшка новости в ленте
    public Image? TitleImage { get; }

    private News(Guid id, string title, string textData, DateTime creationDateTime, Image? image)
    {
        Id = id;
        Title = title;
        TextData = textData;
        CreateDate = creationDateTime;
        TitleImage = image;
    }

    //использование паттерна "фабричный метод"
    /// <summary>
    /// Метод вместо публичного конструктора для валидации и возвращения результата валидации
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="textData"></param>
    /// <param name="creationDateTime"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    public static Result<News> Create(Guid id, string title, string textData, DateTime creationDateTime, Image? titleImage)//необходима библиотека csharpfunctionalextensions. 
    {
        if (string.IsNullOrEmpty(title) || title.Length >= MAX_TITLE_LENGTH)
        {
            return Result.Failure<News>($"'{nameof(title)}' cannot be null or empty!");
        }
        if (string.IsNullOrEmpty(textData) || title.Length >= MAX_TITLE_LENGTH)
        {
            return Result.Failure<News>($"'{nameof(title)}' cannot be null or empty!");
        }
        var news = new News(id, title, textData, DateTime.Now, titleImage);
        return Result.Success<News>(news);
    }
}
