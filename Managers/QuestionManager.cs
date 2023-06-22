using TelegramBotApi.Entities;
using TelegramBotApi.Managers.Contracts;
using TelegramBotApi.Models;

namespace TelegramBotApi.Managers;

public class QuestionManager
{
    private readonly IQuestion _questionRepository;

    public QuestionManager(IQuestion questionRepository)
    {
        _questionRepository = questionRepository;
    }
 
    public async Task<List<Question>> GetQuestions()
    {
        return await _questionRepository.GetQuestions();
    }

    public async Task<Question> GetQuestionById(int id)
    {
        if (id == null)
        {
            return null;
        }
        var question =  await _questionRepository.GetQuestionById(id);
        return question;
    }

    public async  Task<Question> AddQuestion(CreateQuestionModel model)
    {
        var question = ParseToModel(model);
        await _questionRepository.AddQuestion(question:question);
        return question;
    }

    public async Task<Question> Update(CreateQuestionModel model,int questionId)
    {
        var question = await _questionRepository.GetQuestionById(questionId);
        if (question != null)
        {
            question = ParseToModel(model);
            await _questionRepository.Update(question: question);
            return question;
        };
        return null;
    }

    public async Task<string> Delete(int id)
    {
        var question = await _questionRepository.GetQuestionById(id);
        if (question != null)
        {;
            await _questionRepository.Delete(id);
            return "Succes";
        };
        return "Not found";
    }

    private Question ParseToModel(CreateQuestionModel model)
    {
        var question = new Question()
        {
            Title = model.Title,
            Choices = model.Choices,
            Media = new Media()
        };
        return question;
    }

}

