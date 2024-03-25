using System.Text.RegularExpressions;

namespace CodeParserNew;

public class Parser
{
    public int NumberConditions { get; set; } = 0;
    public Tree<Lexem> Tree { get; } = new();
    
    static readonly Dictionary<LexemTypes, string> DynamicDefinition = new()
    {
        {LexemTypes.MainClass, "[a-z,A-Z](Main)[:{:]"},
        {LexemTypes.MainMethod, @"[a-z,A-Z](method)[:(:](\S*)[:):][:{:]"},
        {LexemTypes.DeclaringVariable, "^(int|boolean|char|string|float|double)[a-z,A-Z][:;:]"},
        {LexemTypes.Expression, @"[a-z,A-z][:=:](\S*)[:;:]"},
        {LexemTypes.Condition, @"^(if)[:(:](\S*)[:):][:{:]"},
        {LexemTypes.EndModule, "^[:}:]"}
    };

    public void Parse(string inputString)
    {
        var startIdx = 0;
        inputString = inputString.Replace(" ", String.Empty);
        inputString = inputString.Replace("\t", String.Empty);
        for (int i = 0; i < inputString.Length; i++)
        {
            if (inputString[i] == '{' || inputString[i] == ';' || inputString[i] == '}')
            {
                var str = inputString.Substring(startIdx, i - startIdx + 1);
                str = str.Replace("\n", string.Empty);
                str = str.Replace("\r", string.Empty);
                startIdx = i + 1;
                foreach (var definition in DynamicDefinition)
                {
                    if (new Regex(definition.Value).Match(str).Length > 0)
                    {
                        switch (definition.Key)
                        {
                            case LexemTypes.MainClass:
                                Tree.Begin(new LexemCommon {Type = LexemTypes.MainClass});
                                break;
                            case LexemTypes.MainMethod:
                                Tree.Begin(new LexemCommon {Type = LexemTypes.MainMethod});
                                break;
                            case LexemTypes.DeclaringVariable:
                                Tree.Add(new LexemCommon {Type = LexemTypes.DeclaringVariable});
                                break;
                            case LexemTypes.Expression:
                                str = str.Replace(";", "");
                                var idx = str.IndexOf('=');
                                Tree.Add(new LexemExpression
                                {
                                    RightValue = str.Substring(0, idx),
                                    LeftValue = str.Substring(idx + 1),
                                    Type = LexemTypes.Expression,
                                    Sing = Sings.Equal
                                });
                                break;
                            case LexemTypes.Condition:
                                var idxBegin = str.IndexOf('[') + 1;
                                var idxEnd = str.IndexOf(']');
                                Tree.Begin(new LexemCondition
                                {
                                    Type = LexemTypes.Condition,
                                    NameArgument = $"C{str.Substring(idxBegin, idxEnd - idxBegin)}"
                                });
                                ++NumberConditions;
                                break;
                            case LexemTypes.EndModule:
                                Tree.End();
                                break;
                        }
                    }
                }
            }
        }
    }
}