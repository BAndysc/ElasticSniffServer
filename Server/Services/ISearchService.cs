using SearchSniffServer.Models;
using Server.Models.SearchRequests;

namespace Server.Services;

public interface ISearchService
{
    Task<IReadOnlyList<ISniff>> Search(ISniffSearchRequest request, int start, int count);
}