namespace TelegramBotApi.FileHelper;

public class SavingFiles
{
    private const string Wwwroot = "wwwroot";

    private static void CheckDirectory(string folder)
    {
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
    }

    public static string QuestionImages(IFormFile file,int questionId)
    {
        return SaveFile(file, "QuestionImages",questionId);
    }

    private static string SaveFile(IFormFile file, string folder,int questionId)
    {
        CheckDirectory(Path.Combine(Wwwroot, folder));
        var fileName = questionId + Path.GetExtension(file.FileName);
        var ms = new MemoryStream();
        file.CopyToAsync(ms);
        File.WriteAllBytesAsync(Path.Combine(Wwwroot, folder, fileName), ms.ToArray());
        return $"/{folder}/{fileName}";
    }
}