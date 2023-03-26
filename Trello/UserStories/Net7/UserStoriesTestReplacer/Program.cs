using CSVFile;
using System.Text.RegularExpressions;

var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.Parent;
var stories = File.ReadAllText(Path.Combine(dir.FullName, "UserStories.xml"));

var replacements = GetReplacement(Path.Combine(dir.FullName, "UserStories_Acceptance test.csv"));


Dictionary<string, string> GetReplacement(string csv_path)
{
    var csv = CSVReader.FromFile(csv_path);

    var dic = new Dictionary<string, string>();
    foreach (var line in csv)
    {
        var tests = Regex.Matches(line[1], "<Test>(.+)</Test>")
            .Select(m => m.Groups[1].Value).ToArray();

        var rp = line[2].Replace("\"", "");
        var replaces = Regex.Matches(rp, "((Given|When)\\s.+(?:\\n)?)((And|When|Then)\\s.+(?:\\n)?)+((?:\\n)?(And)\\s.+(?:\\n)?)?")
           .Select(x => x.Value.Trim()).ToArray();

        if(tests.Length != replaces.Length)
        {
            throw new Exception("数据数量不匹配: " + line[0]);
        }

        for(int i=0; i<tests.Length; i++)
        {
            dic.Add(tests[i], replaces[i]);
        }
    }

    return null;
}