using System.Text;

namespace CoffeeShopManagerApplication.Repository;

public class Logger
{
    private readonly string _filePath;
    private readonly object _lock = new();

    public Logger(string filePath)
    {
        _filePath = filePath;
        if (!File.Exists(_filePath))
        {
            File.Create(_filePath).Dispose();
        }
    }

    public void Log(string eventType, string description)
    {
        byte[] bytes = Encoding.UTF8.GetBytes($"{DateTime.UtcNow},[{eventType}],{description}{Environment.NewLine}");

        lock (_lock)
        {
            using FileStream fileStream = new(_filePath, FileMode.Append, FileAccess.Write);
            fileStream.Write(bytes, 0, bytes.Length);
        }
    }

    public IEnumerable<string> GetAllLogs()
    {
        lock (_lock)
        {
            return File.ReadAllLines(_filePath).Select(line => line.Replace(",", " ")).ToList();
        }
    }
}