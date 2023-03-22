var userStories = new UserStories.UserStories("UserStories.xml");

// 输出成文件
var outfile = File.CreateText("UserStories_Generated.txt");
foreach(var story in userStories.Stories)
{
    outfile.WriteLine(story);
    if(story.Id != userStories.Stories.Last().Id)
    {
        outfile.WriteLine("\n ---- ---- ---- ---- \n");
    }
    outfile.Flush();
}
outfile.Close();
