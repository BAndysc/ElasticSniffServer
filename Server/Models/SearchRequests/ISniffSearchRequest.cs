namespace Server.Models.SearchRequests;

public interface ISniffSearchRequest
{
    IReadOnlyList<ISearchRequestTerm> Alternatives { get; }
}

public class SniffSearchRequest : ISniffSearchRequest
{
    public IReadOnlyList<ISearchRequestTerm> Alternatives { get; }
    
    public SniffSearchRequest(params ISearchRequestTerm[] alternatives)
    {
        Alternatives = alternatives.ToList();
    }   
}