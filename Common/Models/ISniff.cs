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

public class AbstractSniff : ISniff
{
    public string Path { get; init; }
    public string? PathInArchive { get; init; }
    public string MD5 { get; init; }
    public string Source { get; init; }
     
    public ulong FileSize { get; init; }
    public ulong BuildVersion { get; init; }
    public DateTime IndexedOn { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    
    public IEnumerable<uint> Spells { get; init; }
    public IEnumerable<uint> GameObjects { get; init; }
    public IEnumerable<uint> Creatures { get; init; }
    public IEnumerable<uint> AreaTriggers { get; init; }
    public IEnumerable<uint> Phases { get; init; }
    public IEnumerable<uint> Gossips { get; init; }
    public IEnumerable<uint> Sounds { get; init; }
    public IEnumerable<uint> Emotes { get; init; }
    public IEnumerable<uint> Quests { get; init; }
    public IEnumerable<uint> Maps { get; init; }
    public IEnumerable<string> Texts { get; init; }
}