using System.Text;
using UserStories;

// User stories 的根目录
var stories_dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent;

// 反序列化的 User stories 实例
var userStories = new UserStories.UserStories(Path.Combine(stories_dir.FullName,"UserStories.xml"));

// 输出全部
OutputToFile("", userStories.Stories);

// 输出 could have
OutputToFile("_could", userStories.Stories
    .Where(x => x.Label == UserStory.LabelType.Could));


// 输出 UserStories 到文件
void OutputToFile(string name, IEnumerable<UserStory> stories)
{
    StreamWriter sw = new StreamWriter(new FileStream($"UserStories_Generated{name}.txt", FileMode.Create), Encoding.UTF8);
    sw.Write(UserStories.UserStories.StoryiesToString(stories));
    sw.Flush(); sw.Close();
}