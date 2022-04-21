namespace Server.Models.SearchRequests;

public interface ISearchRequestTerm
{
    IReadOnlyList<ISearchCondition> Conditions { get; }
}

public class SearchRequestTerm : ISearchRequestTerm
{
    public IReadOnlyList<ISearchCondition> Conditions { get; }

    public SearchRequestTerm(params ISearchCondition[] conditions)
    {
        Conditions = conditions.ToList();
    }
}