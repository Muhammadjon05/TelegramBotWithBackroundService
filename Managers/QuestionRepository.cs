using MongoDB.Driver;
using TelegramBotApi.Entities;
using TelegramBotApi.Managers.Contracts;

namespace TelegramBotApi.Managers;

public class QuestionRepository : IQuestion
{
    private readonly IMongoCollection<Question> _questions;
    public QuestionRepository()
    {
        var client = new MongoClient("mongodb://Muhammadjon:Muha1201@localhost:32768");
        var database = client.GetDatabase("question_db");
        _questions = database.GetCollection<Question>("questions");   
    }
    public async Task<List<Question>> GetQuestions()
    {
        return await _questions.Find(k => true).ToListAsync();
    }

    public async Task<Question> GetQuestionById(int id)
    {
        return await (await _questions.FindAsync(i => i.Id == id)).FirstOrDefaultAsync();
    }

    public async  Task AddQuestion(Question question)
    {
        await _questions.InsertOneAsync(question);
    }

    public async Task Update(Question question)
    {
        var filter = Builders<Question>.Filter.Eq(p => p.Id, question.Id);
        await _questions.ReplaceOneAsync(filter,question);
    }
    
    public async Task Delete(int question)
    {
        var filter = Builders<Question>.Filter.Eq(p => p.Id, question);
        await _questions.DeleteOneAsync(filter);
    }
}