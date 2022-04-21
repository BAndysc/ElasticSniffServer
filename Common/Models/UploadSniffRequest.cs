namespace SearchSniffServer.Models;

public class UploadSniffRequest : ISniff
{
    public string Path { get; set; } = "";
    public string? PathInArchive { get; set; }
    public string MD5 { get; set; } = "";
    public string Source { get; set; } = "";
    public ulong FileSize { get; set; }
    public ulong BuildVersion { get; set; }
    public DateTime IndexedOn { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public IEnumerable<uint> Spells { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> GameObjects { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> Creatures { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> AreaTriggers { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> Phases { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> Gossips { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> Sounds { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> Emotes { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> Quests { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<uint> Maps { get; set; } = Enumerable.Empty<uint>();
    public IEnumerable<string> Texts { get; set; } = Enumerable.Empty<string>();
}