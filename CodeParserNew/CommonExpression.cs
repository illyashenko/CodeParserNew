namespace CodeParserNew;

public static class CommonExpression
{
    private static string DecToBin(int num)
    {
        int bin = 0, k = 1;

        while (num > 0)
        {
            bin += num % 2 * k;
            k *= 10;
            num /= 2;
        }

        var str = bin.ToString();
        while (str.Length < 4)
        {
            str = str.Insert(0, "0");
        }
        return str;
    }
    private static List<string> ConditionOptions(int numberConditions)
    {
        var conditions = new List<string>();
        while (numberConditions > 0)
        {
            conditions.Add(DecToBin(numberConditions));
            --numberConditions;
        }

        return conditions;
    }
    
    public static List<Dictionary<string, string>> Conditions(int num)
    {
        var listConditionOptions = ConditionOptions(NumberToBinStr(num));
        var listDic = new List<Dictionary<string, string>>();
        foreach (var option in listConditionOptions)
        {
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < num; i++)
            {
                dict.Add($"C{i.ToString()}", option[i].ToString());
            }
            listDic.Add(dict);
        }
        return listDic;
    }
    
    public static int GetResult(TreeNode<Lexem> treeNode, Dictionary<string, string> condition)
    {
        int res = 0;
        
        if (treeNode.Value.Type == LexemTypes.Expression)
        {
            if (treeNode.Value is LexemExpression lexem)
            {
                res = int.Parse(lexem.LeftValue??"0");   
            }
        }

        if (treeNode.Children.Count > 0)
        {
            foreach (var child in treeNode.Children)
            {
                if (child.Value.Type == LexemTypes.Condition)
                {
                    if (child.Value is LexemCondition lexemCondition)
                    {
                        if (lexemCondition.NameArgument != null && condition[lexemCondition.NameArgument] == "0")
                        {
                            continue;
                        }
                    }
                }
                
                res = GetResult(child, condition);
            }
        }

        return res;
    }
    
    static int NumberToBinStr(int num)
    {
        var str = string.Empty;
        for (int i = 0; i < num; i++)
        {
            str = string.Concat(str, "1");
        }

        return Convert.ToInt32(str, 2);
    }
}