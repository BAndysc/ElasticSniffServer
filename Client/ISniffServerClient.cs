using System.Net;
using SearchSniffServer.Models;

namespace SearchSniffServer.Client;

public interface ISniffServerClient
{
    Task UploadAsync(UploadSniffRequest request);
    void Upload(UploadSniffRequest request);
    Task<SniffSearchResponse> Search(RequestSniffSearch request);
}

public class UploadException : Exception
{
    public UploadException(HttpStatusCode statusCode, string error) : base("Code: " + statusCode + "\n" + error)
    {
        
    }
}

public class SearchException : Exception
{
    public SearchException(HttpStatusCode statusCode, string error) : base("Code: " + statusCode + "\n" + error)
    {
        
    }

    public SearchException(Exception e) : base("Search exception while reading", e)
    {
        
    }

    public SearchException(string msg) : base(msg)
    {
    }
}