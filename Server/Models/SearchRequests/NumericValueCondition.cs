namespace Server.Models.SearchRequests;

public class NumericValueCondition : ISearchCondition
{
    public NumericValueCondition(uint value, NumericField field)
    {
        Value = value;
        Field = field;
    }

    public uint Value { get; }
    public NumericField Field { get; }
    
    public T Accept<T>(ISearchConditionVisitor<T> visitor) => visitor.Visit(this);
}