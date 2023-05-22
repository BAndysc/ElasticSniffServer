namespace SearchSniffServer.Models;

public class SniffContainsResponse
{
    public SniffContainsResponse(bool containsSniff)
    {
        ContainsSniff = containsSniff;
    }

    public bool ContainsSniff { get; }
}