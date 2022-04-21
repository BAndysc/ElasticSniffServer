using SearchSniffServer.Models;
using Server.Models.SearchRequests;

namespace Server.Services;

public interface ISearchService
{
    Task<SearchResults> Search(ISniffSearchRequest request, int start, int count);
}

public class SearchResults
{
    public SearchResults(IReadOnlyList<ISniff> items, long total)
    {
        Items = items;
        Total = total;
    }

    public IReadOnlyList<ISniff> Items { get; }
    public long Total { get; }
}