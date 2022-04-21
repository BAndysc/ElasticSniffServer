namespace Server.Models.SearchRequests;

public interface ISearchCondition
{
    T Accept<T>(ISearchConditionVisitor<T> visitor);
}