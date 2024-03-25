namespace CodeParserNew;

public abstract class Lexem
{
    public LexemTypes Type { get; set; }
}

public class LexemCommon : Lexem { }

public class LexemExpression : Lexem
{
    public string? RightValue { get; set; }
    public string? LeftValue { get; set; }
    public Sings Sing { get; set; }
}

public class LexemCondition : Lexem
{ 
    public string? NameArgument { get; set; }
}

public enum LexemTypes
{
    MainClass,
    MainMethod,
    Expression,
    DeclaringVariable,
    Condition,
    EndModule
}

public enum Sings
{
    // '='
    Equal,
}