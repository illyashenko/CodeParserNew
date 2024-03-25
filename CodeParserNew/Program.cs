using CodeParserNew;

Console.Write("укажите путь к файлу: ");
string? path = Console.ReadLine();

using var reader = new StreamReader(path??string.Empty);
var textCode = reader.ReadToEnd();
var parser = new Parser();
parser.Parse(textCode);

// get NOde main class
var main = parser.Tree.Nodes.FirstOrDefault(el => el.Value.Type == LexemTypes.MainClass);
var conditions = CommonExpression.Conditions(parser.NumberConditions);
var listResult = new List<int>();
// go around the tree
foreach (var condition in conditions)
{
    if (parser.Tree is not null)
    {
        if (main != null)
        {
            var res = CommonExpression.GetResult(main, condition);
            listResult.Add(res);
        }
    }
}
// Print array result
Console.WriteLine($"[{string.Join(", ", listResult.Distinct().Order().ToList())}]");