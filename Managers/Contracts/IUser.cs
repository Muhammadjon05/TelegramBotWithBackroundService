using TelegramBotApi.Entities;

namespace TelegramBotApi.Managers.Contracts;

public interface IUser
{
    Task<List<Users>> GetUsers();

}