namespace TelegramBotApi.Managers;


public class UsersStore
{
    public static List<long> UserIds = new List<long>();

    public static Dictionary<long, UserData> Answers = new ();
}

public class UserData
{
    public int CorrectAnswer { get; set; }
    public int InCorrectAnswer { get; set; }
}