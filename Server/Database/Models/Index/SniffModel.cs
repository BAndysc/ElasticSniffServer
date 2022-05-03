using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Models.Index;

public class SniffModel
{
    [Key] 
    [StringLength(32, MinimumLength = 32)]
    public string MD5 { get; set; } = "";
    
    public uint BuildVersion { get; set; }
    
    public DateTime IndexedOn { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }

    public string Source { get; set; } = "";

    public string Path { get; set; } = "";
    
    public string? PathInArchive { get; set; }
    
    public ulong FileSize { get; set; }
    
    public string? Uploader { get; set; }
}

public enum DatabaseNumberField
{
    Spell = 0,
    GameObject = 1,
    Creature = 2,
    AreaTrigger = 3,
    Phase = 4,
    Gossip = 5,
    Sound = 6,
    Emote = 7,
    Quest = 8,
    Map = 9
}

public enum DatabaseTextField
{
    Chat = 0
}

public class SniffNumberFieldModel
{
    [StringLength(32, MinimumLength = 32)]
    public string SniffModelId { get; set; } = "";
    
    public SniffModel SniffModel { get; set; } = null!;
    
    public DatabaseNumberField Field { get; set; }
    
    public long Value { get; set; }
}

public class SniffTextFieldModel
{
    [StringLength(32, MinimumLength = 32)]
    public string SniffModelId { get; set; } = "";
    
    public SniffModel SniffModel { get; set; } = null!;
    
    public DatabaseTextField Field { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Text { get; set; } = "";
}