using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TelegramBotApi.Entities;

public class Users
{
    [BsonId]
    public ObjectId Id { get; set; }
    public long ChatId { get; set; }
    public string UserName { get; set; }
    public int CorrectAnswers { get; set; }
    public int IncorrectAnswers { get; set; }
}