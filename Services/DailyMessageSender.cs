using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotApi.Entities;
using TelegramBotApi.Managers;
using TelegramBotApi.Managers.Contracts;
using File = System.IO.File;

namespace TelegramBotApi.Services;

public class DailyMessageSender : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private IMongoCollection<Users> _users;
	public DailyMessageSender(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (true)
		{
			SendMessage();
			await Task.Delay(TimeSpan.FromSeconds(10));
		}
	}

	public async Task SendMessage()
	{
		var scope = _serviceProvider.CreateScope();
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
		var users = await userManager.GetUser();
		var bot = new TelegramBotClient("5886587622:AAG4KwO9rmQ4O5aco1fFdhpqesBZ5d_4ZIg");
			foreach (var user in users)
			{
				await SendQuestion(bot, user.ChatId);
			}
	}

	public async Task SendQuestion(ITelegramBotClient bot, long chatId)
	{
		var scope = _serviceProvider.CreateScope();
		var questionManager = scope.ServiceProvider.GetRequiredService<QuestionManager>();
		var randoom = new Random();
		var random = randoom.Next(1,21);
		var question = questionManager.GetQuestionById(random);
		SendQuestionByIndex(bot,chatId,question.Result.Id);
	}
	InlineKeyboardMarkup CreateQuestionChoiceButtons(int index, int? choiceIndex = null, bool? answer = null)
	{
		var scope = _serviceProvider.CreateScope();
		var questionManager = scope.ServiceProvider.GetRequiredService<QuestionManager>();
		var question = questionManager.GetQuestionById(index);
		var choiceButtons = new List<List<InlineKeyboardButton>>();

		for (int i = 0; i < question.Result.Choices.Count; i++)
		{
			var choiceText = answer == null ? question.Result.Choices[i].Text : question.Result.Choices[i].Text + answer;

			var choiceButton = new List<InlineKeyboardButton>()
			{

				InlineKeyboardButton.WithCallbackData(choiceText, $"{index},{i}")
			};

			choiceButtons.Add(choiceButton);
		}
		return new InlineKeyboardMarkup(choiceButtons);
	}
	public bool QuestionAnswer(int questionIndex, int choiceIndex)
	{
		var scope = _serviceProvider.CreateScope();
		var questionManager = scope.ServiceProvider.GetRequiredService<QuestionManager>();
		var question = questionManager.GetQuestionById(questionIndex);
		return question.Result.Choices[choiceIndex].Answer;
	}
	public async Task SendQuestionByIndex(ITelegramBotClient bot,long chatId, int index)
	{
		var scope = _serviceProvider.CreateScope();
		var questionManager = scope.ServiceProvider.GetRequiredService<QuestionManager>();
		var question = questionManager.GetQuestionById(index);

		var message = $"{question.Result.Id}. {question.Result.Title}";
		if (question.Result.Media.isExist)
		{
			try
			{
				string path = $"C://Users//Muhammadjon//Desktop//TelegramBotApi//wwwroot//QuestionImages/{question.Result.Id}.jpg";
				using (
					var stream = File.OpenRead(path))
				{
					await bot.SendPhotoAsync(
						chatId: chatId,
						photo: new InputFileStream(stream),
						caption: message,
						replyMarkup: CreateQuestionChoiceButtons(index));
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		else
		{
			bot.SendTextMessageAsync(chatId, message, replyMarkup: CreateQuestionChoiceButtons(index));
		}
	}
}