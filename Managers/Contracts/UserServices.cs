using MongoDB.Driver;
using TelegramBotApi.Entities;

namespace TelegramBotApi.Managers.Contracts;

public class UserServices : IUser
{
    private readonly IMongoCollection<Users> _users;
    public UserServices()
    {
        var client = new MongoClient("mongodb://Muhammadjon:Muha1201@localhost:32768");
        var database = client.GetDatabase("question_db");
        _users = database.GetCollection<Users>("users"); 
    }
    public async Task<List<Users>> GetUsers()
    {
        var users =await _users.FindAsync(k=>true).Result.ToListAsync();
        return users;
    }

}