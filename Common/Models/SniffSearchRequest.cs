namespace SearchSniffServer.Models;

public class RequestSniffSearch
{
    public RequestSniffSearch(List<List<Term>> alternatives)
    {
        Alternatives = alternatives;
    }

    public List<List<Term>> Alternatives { get; set; }
}

public enum TermType
{
    build,
    numeric
}

public enum ComparisonEq
{
    eq,
    gt,
    gte,
    lt,
    lte
}

public class Term
{
    public TermType Type { get; set; }
    public uint NumericValue { get; set; }
    public ComparisonEq Eq { get; set; }
    public string? Field { get; set; }
}

