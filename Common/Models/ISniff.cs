namespace SearchSniffServer.Models;

public interface ISniff
{
    string Path { get; }
    string? PathInArchive { get; }
    string MD5 { get; }
    string Source { get; }
    
    public ulong FileSize { get; }
    public ulong BuildVersion { get; }
    public DateTime IndexedOn { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    
    public IEnumerable<uint> Spells { get; }
    public IEnumerable<uint> GameObjects { get; }
    public IEnumerable<uint> Creatures { get; }
    public IEnumerable<uint> AreaTriggers { get; }
    public IEnumerable<uint> Phases { get; }
    public IEnumerable<uint> Gossips { get; }
    public IEnumerable<uint> Sounds { get; }
    public IEnumerable<uint> Emotes { get; }
    public IEnumerable<uint> Quests { get; }
    public IEnumerable<uint> Maps { get; }
    public IEnumerable<string> Texts { get; }
}