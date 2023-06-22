using TelegramBotApi.Entities;

namespace TelegramBotApi.Models;

public class CreateQuestionModel
{
    public string Title { get; set; }
    public List<Choice> Choices { get; set; } = new List<Choice>();

 
}