using Nest;
using SearchSniffServer.Models;
using Server.Models;

namespace Server.Services.Elastic;

[ElasticsearchType(IdProperty = "MD5")]
public class SniffElasticDocument : ISniff
{
    public SniffElasticDocument(ISniff sniff)
    {
        BuildVersion = sniff.BuildVersion;
        StartTime = sniff.StartTime;
        EndTime = sniff.EndTime;
        MD5 = sniff.MD5;
        Source = sniff.Source;
        Path = sniff.Path;
        FileSize = sniff.FileSize;
        IndexedOn = sniff.IndexedOn;
        PathInArchive = sniff.PathInArchive;
        spells = sniff.Spells.ToList();
        gameObjects = sniff.GameObjects.ToList();
        creatures = sniff.Creatures.ToList();
        areaTriggers = sniff.AreaTriggers.ToList();
        phases = sniff.Phases.ToList();
        gossips = sniff.Gossips.ToList();
        sounds = sniff.Sounds.ToList();
        emotes = sniff.Emotes.ToList();
        quests = sniff.Quests.ToList();
        maps = sniff.Maps.ToList();
        broadcastTexts = sniff.BroadcastTexts.ToList();
        texts = sniff.Texts.ToList();
        gossipTexts = sniff.GossipTexts.ToList();
    }

    public ulong FileSize { get; set; }
    public ulong BuildVersion { get; set; }
    public DateTime IndexedOn { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string MD5 { get; set; }
    public string Source { get; set; }
    
    public string Path { get; set; }
    public string? PathInArchive { get; set; }
    
    public List<uint> spells { get; set; }
    public List<uint> gameObjects { get; set; }
    public List<uint> creatures { get; set; }
    public List<uint> areaTriggers { get; set; }
    public List<uint> phases { get; set; }
    public List<uint> gossips { get; set; }
    public List<uint> sounds { get; set; }
    public List<uint> emotes { get; set; }
    public List<uint> quests { get; set; }
    public List<uint> maps { get; set; }
    public List<uint> broadcastTexts { get; set; }
    public List<string> texts { get; set; }
    public List<string> gossipTexts { get; set; }
    
    
    public IEnumerable<uint> Spells => spells;
    public IEnumerable<uint> GameObjects => gameObjects;
    public IEnumerable<uint> Creatures => creatures;
    public IEnumerable<uint> AreaTriggers => areaTriggers;
    public IEnumerable<uint> Phases => phases;
    public IEnumerable<uint> Gossips => gossips;
    public IEnumerable<uint> Sounds => sounds;
    public IEnumerable<uint> Emotes => emotes;
    public IEnumerable<uint> Quests => quests;
    public IEnumerable<uint> Maps => maps;
    public IEnumerable<uint> BroadcastTexts => broadcastTexts;
    public IEnumerable<string> Texts => texts;
    public IEnumerable<string> GossipTexts => gossipTexts;
}