using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotApi.Entities;
using TelegramBotApi.Services;

namespace TelegramBotApi.Controllers;

[Route("/")]
[ApiController]
public class BotController : ControllerBase
{
	private readonly IServiceProvider _serviceProdivider;
	
	private readonly IMongoCollection<Users> _users;
	public BotController(IServiceProvider serviceProdivider)
	{
		var client = new MongoClient("mongodb://Muhammadjon:Muha1201@localhost:32768");
		var database = client.GetDatabase("question_db");
		_users = database.GetCollection<Users>("users");   
		_serviceProdivider = serviceProdivider;
	}
	[HttpPost]
    public async Task GetUpdate([FromBody] Update update)
    {
        var bot = new TelegramBotClient("5886587622:AAG4KwO9rmQ4O5aco1fFdhpqesBZ5d_4ZIg");

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
	        await bot.SendTextMessageAsync(update.Message.From.Id, "Sizga har soatda savol junatilib turiladi");
	        var user = Builders<Users>.Filter.Eq(p => p.ChatId, update.Message.From.Id);
	        var users = await _users.FindAsync(user).Result.AnyAsync();
	        if (!users)
	        {
		        var user1 = new Users
		        {
			        
			        ChatId = update.Message.From.Id,
			        UserName = update.Message.From.Username,
			        CorrectAnswers = 0,
			        IncorrectAnswers = 0
		        };
		        _users.InsertOne(user1);
	        }
	        if (update.Message.Text == "/result")
	        {
		        var filter = Builders<Users>.Filter.Eq(p => p.ChatId, update.CallbackQuery.From.Id);
		        var userInfo = await _users.FindAsync(filter).Result.FirstOrDefaultAsync();
		        await bot.SendTextMessageAsync(update.Message.From.Id, $"Result: Correct: {userInfo.CorrectAnswers}, InCorrect: {userInfo.IncorrectAnswers}");
			}
		}

        if (update.Type == UpdateType.CallbackQuery)
        {
	        var dataFrom = update.CallbackQuery.Data;
	        int[] data = dataFrom.Split(',').Select(int.Parse).ToArray();
	        var scope = _serviceProdivider.CreateScope();
	        var questionManager = scope.ServiceProvider.GetRequiredService<DailyMessageSender>();
	        var answer = questionManager.QuestionAnswer(data[0], data[1]);

	        if (answer)
	        {
		        var filter = Builders<Users>.Filter.Eq(p => p.ChatId, update.CallbackQuery.From.Id);
		        var user = await _users.FindAsync(filter).Result.FirstOrDefaultAsync();
		        user.CorrectAnswers++;
		        bot.SendTextMessageAsync(update.CallbackQuery.From.Id, "True");
		        bot.DeleteMessageAsync(user.ChatId, update.CallbackQuery.Message.MessageId);
	        }
	        else
	        {
		        var filter = Builders<Users>.Filter.Eq(p => p.ChatId, update.CallbackQuery.From.Id);
		        var user = await _users.FindAsync(filter).Result.FirstOrDefaultAsync();
		        user.IncorrectAnswers++;
		        bot.SendTextMessageAsync(update.CallbackQuery.From.Id, "False");
		        bot.DeleteMessageAsync(user.ChatId, update.CallbackQuery.Message.MessageId);
	        }
        }
    }

    [HttpGet]
    public string GetMe() => "Working...";
}
