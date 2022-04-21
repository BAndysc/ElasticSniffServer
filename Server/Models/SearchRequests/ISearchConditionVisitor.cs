namespace Server.Models.SearchRequests;

public interface ISearchConditionVisitor<T>
{
    T Visit(SearchBuildCondition condition);
    T Visit(NumericValueCondition condition);
}