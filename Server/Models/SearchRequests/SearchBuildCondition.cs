namespace Server.Models.SearchRequests;

public class SearchBuildCondition : ISearchCondition
{
    public SearchBuildCondition(ulong build, ComparisonOperator @operator)
    {
        Build = build;
        Operator = @operator;
    }

    public ulong Build { get; }
    public ComparisonOperator Operator { get; }
    
    public T Accept<T>(ISearchConditionVisitor<T> visitor) => visitor.Visit(this);
}