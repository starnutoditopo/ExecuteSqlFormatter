// See: https://stackoverflow.com/a/40403351/1288109

namespace ExecuteSqlFormatter;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using ExecuteSqlFormatter.Utilities;


public class Program
{
    public static void Main()
    {
        //var sql = @"exec sp_executesql N'UPDATE MyTable SET [Field1] = @0, [Field2] = @1',N'@0 nvarchar(max) ,@1 int',@0=N'String',@1=0";
        var sql = WindowsClipboard.GetText();
        if (string.IsNullOrWhiteSpace(sql))
        {
            Console.WriteLine("Clipboard contains null or whitespace string.");
        }
        else
        {
            var converted = ConvertSql(sql);
            Console.WriteLine(converted);
            WindowsClipboard.SetText(converted);
        }
    }

    public static string ConvertSql(string origSql)
    {
        var re = new Regex(@"exec*\s*sp_executesql\s+N'([\s\S]*)',\s*N'(@[\s\S]*?)',\s*([\s\S]*)", RegexOptions.IgnoreCase); // 1: the sql, 2: the declare, 3: the setting
        var match = re.Match(origSql);
        if (match.Success)
        {
            var sql = match.Groups[1].Value.Replace("''", "'");
            //var declare = match.Groups[2].Value;
            var setting = match.Groups[3].Value + ',';

            // to deal with comma or single quote in variable values, we can use the variable name to split
            var re2 = new Regex(@"@[^',]*?\s*=");
            var variables = re2.Matches(setting).Cast<Match>().Select(m => m.Value).ToArray();
            var values = re2.Split(setting).Where(s => !string.IsNullOrWhiteSpace(s)).Select(m => m.Trim(',').Trim().Trim(';')).ToArray();

            for (int i = variables.Length - 1; i >= 0; i--)
            {
                sql = Regex.Replace(sql, "(" + variables[i].Replace("=", "") + ")", values[i], RegexOptions.Singleline);
            }
            return sql;
        }

        return @"Unknown sql query format.";
    }
}