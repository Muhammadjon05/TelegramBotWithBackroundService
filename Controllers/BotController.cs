using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotApi.Services;

namespace TelegramBotApi.Controllers;

[Route("/")]
[ApiController]
public class BotController : ControllerBase
{
	private readonly IServiceProvider _serviceProdivider;

	public BotController(IServiceProvider serviceProdivider)
	{
		_serviceProdivider = serviceProdivider;
	}
	[HttpPost]
    public async Task GetUpdate([FromBody] Update update)
    {
        var bot = new TelegramBotClient("5886587622:AAG4KwO9rmQ4O5aco1fFdhpqesBZ5d_4ZIg");

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
	        await bot.SendTextMessageAsync(update.Message.From.Id, "Sizga har soatda savol junatilib turiladi");

	        if (!UsersStore.UserIds.Contains(update.Message.From.Id))
	        {
		        UsersStore.UserIds.Add(update.Message.From.Id);
		        UsersStore.Answers.Add(update.Message.From.Id, new UserData());
	        }


	        if (update.Message.Text == "/result")
	        {
		        var userData = UsersStore.Answers[update.Message.From.Id];
		        await bot.SendTextMessageAsync(update.Message.From.Id, $"Result: Correct: {userData.CorrectAnswer}, InCorrect: {userData.InCorrectAnswer}");
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
		        UsersStore.Answers[update.CallbackQuery.From.Id].CorrectAnswer++;
		        bot.SendTextMessageAsync(update.CallbackQuery.From.Id, "True");
		        bot.DeleteMessageAsync(update.CallbackQuery.From.Id, update.CallbackQuery.Message.MessageId);
	        }
	        else
	        {
		        UsersStore.Answers[update.CallbackQuery.From.Id].InCorrectAnswer++;
		        bot.SendTextMessageAsync(update.CallbackQuery.From.Id, "False");
		        bot.DeleteMessageAsync(update.CallbackQuery.From.Id, update.CallbackQuery.Message.MessageId);
	        }
        }
    }

    [HttpGet]
    public string GetMe() => "Working...";
}
