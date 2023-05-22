namespace SearchSniffServer.Models;

public class SniffContainsRequest
{
    public SniffContainsRequest(string md5)
    {
        MD5 = md5;
    }

    public string MD5 { get; }
}