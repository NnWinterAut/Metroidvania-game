using System.Text.RegularExpressions;

//var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.Parent;
//var stories = File.ReadAllText(Path.Combine(dir.FullName, "UserStories.xml"));

var stories = File.ReadAllText("Test.xml");

var tests = GetTests();
Console.WriteLine();
var replacements = GetReplacement();


IEnumerable<string> GetTests()
{
    Console.WriteLine("输入要替换的Test(s)：");
    string tests = Console.ReadLine();

    var tests_t = Regex.Matches(tests, "<Test>(.+)</Test>")
        .Select(m => m.Groups[1].Value).ToArray();

    return tests_t;
}

IEnumerable<string> GetReplacement()
{
    Console.WriteLine("输入要替换的内容：");
    string replacement = Console.ReadLine();

    var replaces = Regex.Matches(replacement, "Given.+\n(?:And.+\n)?When.+\nThen.+(?:\n)?")
        .Select(x => x.Value.Trim());

    return replaces;
}