using CSVFile;
using System.Text;
using System.Text.RegularExpressions;

var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.Parent;
var stories = File.ReadAllText(Path.Combine(dir.FullName, "UserStories.xml"));

var replacements = GetReplacement(Path.Combine(dir.FullName, "UserStories_Acceptance test.csv"));

foreach (var rp in replacements)
{
    stories = stories.Replace(rp.Test, rp.Replace);
}

File.WriteAllText(Path.Combine(dir.FullName, "UserStories.xml"), stories);

List<Replacement> GetReplacement(string csv_path)
{
    var csv = CSVReader.FromFile(csv_path);

    var list = new List<Replacement>();
    foreach (var line in csv)
    {
        var tests = Regex.Matches(line[1], "<Test>(.+)</Test>")
            .Select(m => m.Groups[1].Value).ToArray();

        var rp = line[2].Replace("\"", "");
        var replaces = Regex.Matches(rp, "((Given|When)\\s.+(?:\\n)?)((And|When|Then)\\s.+(?:\\n)?)+((?:\\n)?(And)\\s.+(?:\\n)?)?")
           .Select(x => x.Value.Trim()).ToArray();

        if (tests.Length != replaces.Length)
        {
            throw new Exception("数据数量不匹配: " + line[0]);
        }

        for (int i = 0; i < tests.Length; i++)
        {
            list.Add(new Replacement(int.Parse(line[0]), i, tests[i], replaces[i]));
        }
    }

    return list;
}

struct Replacement
{
    public int Id { get; init; }
    public int Index { get; init; }
    public string Test { get; init; }
    public string Replace { get; init; }
    public Replacement(int id, int index, string test, string replace)
    {
        Id = id;
        Index = index;
        Test = test;
        Replace = FormatReplace(replace);
    }
    string FormatReplace(string replace)
    {
        var str = new StringBuilder();
        string[] lines = replace.Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        str.AppendLine();
        for(int i=0;i<lines.Length; i++)
        {
            str.Append(
                "                " + 
                lines[i].Trim().Replace(",","").Replace(".", "") + 
                (i==lines.Length-1 ? "." : ",") + 
                "\n");
        }
        str.Append("            ");
        return str.ToString();
    }
}