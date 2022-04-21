namespace SearchSniffServer.Models;

public class SniffSearchResponse
{
    public SniffSearchResponse(List<SniffModelResponse> responses)
    {
        Responses = responses;
    }

    public List<SniffModelResponse> Responses { get; }
}

public class SniffModelResponse
{
    public string Path { get; set; } = "";
    public string? PathInArchive { get; set; } = "";
    public string Source { get; set; } = "";
    public ulong GameBuild { get; set; }
    public ulong FileSize { get; set; }
    public DateTime SniffTime { get; set; }
    public string MD5 { get; set; } = "";
}