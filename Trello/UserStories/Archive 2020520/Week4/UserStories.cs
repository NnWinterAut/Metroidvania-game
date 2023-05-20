using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace UserStories
{
    internal class UserStories
    {
        /// <summary>
        /// User stories 的实例
        /// </summary>
        public List<UserStory> Stories;

        /// <summary>
        /// 从 xml 文件中读取 user stories
        /// </summary>
        /// <param name="xml_path">xml文件的路径</param>
        public UserStories(string xml_path)
        {
            Stories = new List<UserStory>();
            XElement xmlFile = XElement.Load(xml_path);

            // 获取 UserStory 节点
            var story_nodes = xmlFile.Nodes()
                .Where(x => x.NodeType == XmlNodeType.Element)
                .Cast<XElement>()
                .Where(x => x.Name == "UserStory");

            // 遍历反序列化所有节点
            foreach (var node in story_nodes)
            {
                Stories.Add(ReadNode(node));
            }
        }

        /// <summary>
        /// 从 UserStory 节点反序列化数据
        /// </summary>
        /// <param name="story">UserStory 节点</param>
        /// <returns>反序列化后的 UserStory</returns>
        private UserStory ReadNode(XElement story)
        {
            int id = int.Parse(story.Attribute("id").Value);
            string name = story.Attribute("heading").Value;
            string nameZh = story.Attribute("headingZH").Value;

            string description = story.Element("Description").Value;
            string descriptionZh = story.Element("DescriptionZH").Value;

            var invest = new UserStory.InvestStruct(story.Element("INVEST").Value);
            var tests = story.Element("Tests").Elements("Test").Select(x => x.Value).ToList();
            var label = UserStory.LabelConverter(story.Element("Label").Value);
            var url = story.Element("Url").Value;

            return new UserStory(id, name, nameZh, description, descriptionZh, invest, tests, label, url);
        }

        /// <summary>
        /// 将可迭代的 User stories 转换为连续的文本
        /// </summary>
        /// <param name="stories">User stories</param>
        /// <returns>文本</returns>
        public static string StoriesToString(IEnumerable<UserStory> stories)
        {
            var str = new StringBuilder();
            foreach (var story in stories)
            {
                str.AppendLine(story.ToString());
                if (story.Id != stories.Last().Id)
                {
                    str.AppendLine("\n ---- ---- ---- ---- \n");
                }
            }
            return str.ToString();
        }
        /// <summary>
        /// 只获取 ID 和 Heading 的列表
        /// </summary>
        /// <param name="stories">User stories</param>
        /// <returns>文本</returns>
        public static string StoriesHeadingsOnly(IEnumerable<UserStory> stories)
        {
            var str = new StringBuilder();
            foreach (var story in stories)
            {
                str.AppendLine($"ID = {story.Id,2} , {story.Name,20} , {story.NameZh}");
            }
            return str.ToString();
        }
    }
    public struct UserStory
    {
        public int Id;
        public string Name;
        public string NameZh;
        public string Description;
        public string DescriptionZh;
        public InvestStruct Invest;
        public List<string> Tests;
        public LabelType Label;
        public string Url;

        public UserStory(int id, string name, string nameZh, string description, string descriptionZh, InvestStruct invest, List<string> tests, LabelType label, string url)
        {
            Id = id;
            Name = name;
            NameZh = nameZh;
            Description = description;
            DescriptionZh = descriptionZh;
            Invest = invest;
            Tests = tests;
            Label = label;
            Url = url;
        }

        public enum LabelType : byte
        {
            [Description("None")]
            None = 0,
            [Description("Must Have")]
            Must,
            [Description("Should Have")]
            Should,
            [Description("Could Have")]
            Could
        }
        public static LabelType LabelConverter(string labelStr)
        {
            if (labelStr == GetDescription(LabelType.Must)) { return LabelType.Must; }
            if (labelStr == GetDescription(LabelType.Should)) { return LabelType.Should; }
            if (labelStr == GetDescription(LabelType.Could)) { return LabelType.Could; }
            return LabelType.None;
        }
        public static string LabelConverter(LabelType label)
        {
            return GetDescription(label);
        }
        public static string GetDescription(Enum val)
        {
            var field = val.GetType().GetField(val.ToString());
            var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return customAttribute == null ? val.ToString() : ((DescriptionAttribute)customAttribute).Description;
        }
        public struct InvestStruct
        {
            public (string, bool) Independent;
            public (string, bool) Negotiable;
            public (string, bool) Valuable;
            public (string, bool) Estimable;
            public (string, bool) Small;
            public (string, bool) Testable;
            public InvestStruct(string str)
            {
                var m_independent = Regex.Match(str, LabelRegex("Independent"));
                var m_negotiable = Regex.Match(str, LabelRegex("Negotiable"));
                var m_valuable = Regex.Match(str, LabelRegex("Valuable"));
                var m_estimable = Regex.Match(str, LabelRegex("Estimable"));
                var m_small = Regex.Match(str, LabelRegex("Small"));
                var m_testable = Regex.Match(str, LabelRegex("Testable"));

                Func<Match, (string, bool)> Func_LabelToTuple = (match) =>
                {
                    return (
                        match.Groups[2].Value,
                        match.Groups[1].Value == "Yes" ? true : false
                        );
                };

                Independent = Func_LabelToTuple(m_independent);
                Negotiable = Func_LabelToTuple(m_negotiable);
                Valuable = Func_LabelToTuple(m_valuable);
                Estimable = Func_LabelToTuple(m_estimable);
                Small = Func_LabelToTuple(m_small);
                Testable = Func_LabelToTuple(m_testable);
            }
            private static string LabelRegex(string labelName)
            {
                return labelName + "\\s+-\\s+(Yes|No),\\s+(.+)\\n";
            }
        }
        public override string ToString()
        {
            var str = new StringBuilder();

            // 基本信息
            str.AppendLine($"ID = {Id,2} , {Name,10} , {NameZh}");
            str.AppendLine();
            str.AppendLine($"  Label = {LabelConverter(Label),11} , Url = {Url}");
            str.AppendLine();
            str.AppendLine($"    I ({Invest.Independent.Item2,5}) = {Invest.Independent.Item1}");
            str.AppendLine($"    N ({Invest.Negotiable.Item2,5}) = {Invest.Negotiable.Item1}");
            str.AppendLine($"    V ({Invest.Valuable.Item2,5}) = {Invest.Valuable.Item1}");
            str.AppendLine($"    E ({Invest.Estimable.Item2,5}) = {Invest.Estimable.Item1}");
            str.AppendLine($"    S ({Invest.Small.Item2,5}) = {Invest.Small.Item1}");
            str.AppendLine($"    T ({Invest.Testable.Item2,5}) = {Invest.Testable.Item1}");
            str.AppendLine();
            foreach (var test in Tests) { str.AppendLine($"      Test - {test}"); }
            return str.ToString();
        }
    }
}
