using TelegramBotApi.Entities;

namespace TelegramBotApi.Managers.Contracts;

public class UserManager
{
    private readonly IUser _user;
    public UserManager(IUser user)
    {
        _user = user;
    }
    public async Task<List<Users>> GetUser()
    {
        var users = await _user.GetUsers();
        return users;
    }
}