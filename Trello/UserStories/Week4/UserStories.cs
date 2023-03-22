using System.ComponentModel;
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

            //var invest = ReadInvest(story.Element("INVEST").Value);
            var tests = story.Element("Tests").Elements("Test").Select(x=>x.Value);
            var label = UserStory.LabelConverter(story.Element("Label").Value);
            var url = story.Element("Url").Value;

            return new UserStory();
        }
        private Dictionary<string, string> ReadInvest(string str)
        {
            throw new NotImplementedException();
        }
    }
    struct UserStory
    {
        public int Id;
        public string Name;
        public string NameZh;
        public string Description;
        public string DescriptionZh;
        public Dictionary<string, string> Invest;
        public List<string> Tests;
        public LabelType Label;
        public string Url;

        public UserStory(int id, string name, string nameZh, string description, string descriptionZh, Dictionary<string, string> invest, List<string> tests, LabelType label, string url)
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
    }
}
