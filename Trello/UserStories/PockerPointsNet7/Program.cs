using CSVFile;
using System.Text;

var workspace = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent;

var poker = CSVReader.FromFile(Path.Combine(workspace.FullName, "PokerPoints.csv"));

var count = 0;
var headers = poker.Headers.Skip(2).ToArray();

var pokers = new int[] { 1, 2, 3, 5, 13, 21, 55, 90, 125, 150 };

var points = new List<PokerPoint>();
foreach (var row in poker)
{
    count++;
    if (row == null) { Console.WriteLine("Missing a row"); continue; }

    points.Add(new PokerPoint(row, headers, pokers));
}

var result = PokerPoint.PokerPointsToString(points);
Console.WriteLine(result);

File.WriteAllText(
    Path.Combine(workspace.FullName, "PokerPoints_Gen.csv"),
    PokerPoint.PokerPointsToCsvStr(points));


struct PokerPoint
{
    public int Id { get; init; }
    public string Name { get; init; }
    public Dictionary<string, int> Points { get; init; }
    public int Total { get; init; }
    public float Average { get; init; }
    public int NearestPokerPoint { get; init; }
    public PokerPoint(string[] row, string[] heading, int[] pokers)
    {
        Id = int.Parse(row[0]);
        Name = row[1].Trim();

        Points = new Dictionary<string, int>
        {
            { heading[0], int.Parse(row[2]) },
            { heading[1], int.Parse(row[3]) },
            { heading[2], int.Parse(row[4]) },
            { heading[3], int.Parse(row[5]) },
            { heading[4], int.Parse(row[6]) }
        };

        Total = Points.Values.Sum();
        Average = (float)Total / Points.Values.Count;

        int min_index = 0; float? min = null;
        for(int i=0; i<pokers.Length; i++)
        {
            if(min == null) { min = Math.Abs(Average - pokers[i]); min_index = i; }
            else
            {
                var new_min = Math.Abs(Average - pokers[i]);
                if(new_min < min) { min = new_min; min_index = i; }
            }
        }
        NearestPokerPoint = pokers[min_index];
    }
    public override string ToString()
    {
        return $"{Id},{Name},{Total},{Average},{NearestPokerPoint}";
    }
    public static string PokerPointsToString(IEnumerable<PokerPoint> pokerpoints)
    {
        var str = new StringBuilder();
        str.AppendLine($"Id | {"Name",15}{" ",10} | Total | Average | Poker");
        str.AppendLine($"---+---------------------------+-------+---------+-------");
        foreach(var point in pokerpoints)
        {
            var p_str = $"{point.Id,2} | {point.Name,25} | {point.Total,5} | {point.Average,7} | {point.NearestPokerPoint,5}";
            str.AppendLine(p_str.ToString());
            
        }
        return str.ToString();
    }
    public static string PokerPointsToCsvStr(IEnumerable<PokerPoint> pokerpoints)
    {
        var str = new StringBuilder();
        str.AppendLine($"Id,Name,Total,Average,Poker");
        foreach (var point in pokerpoints)
        {
            str.AppendLine(point.ToString());
        }
        return str.ToString();
    }
}