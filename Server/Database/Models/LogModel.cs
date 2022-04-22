using System.ComponentModel.DataAnnotations;

namespace Server.Database.Models;

public class LogModel
{
    [Key]
    public Guid Key { get; set; }
    public DateTime Date { get; set; }
    public string Method { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public string? Ip { get; set; }
    public string Text { get; set; } = "";
    
    public LogModel(string method, string text, string userAgent)
    {
        Method = method;
        Text = text;
        UserAgent = userAgent;
    }
    
    public static LogModel Create(string? ip, string method, string text, string userAgent, DateTime? insertDate = null)
    {
        LogModel model = new LogModel(method, text, userAgent);
        model.Key = Guid.NewGuid();
        model.Ip = ip;
        model.Date = insertDate ?? DateTime.UtcNow;
        
        return model;
    }
}