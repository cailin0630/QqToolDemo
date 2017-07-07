using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Automation;

namespace QQDemo
{
    public class QqInfoProvide
    {
        public static List<QqInfo> GetCurrentInfo()
        {
            var list = new List<QqInfo>();
            var process = Process.GetProcessesByName("QQ");
            if (process.Length == 0)
                return list;
            //获取根节点
            var aeTop = AutomationElement.RootElement;

            //查找窗体名
            var aeForm = aeTop.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.IsEnabledProperty, true));

            foreach (AutomationElement ae in aeForm)
            {
                if (ae.Current.ClassName == "TXGuiFoundation" && ae.Current.AccessKey != "")
                    list.Add(new QqInfo
                    {
                        Name = ae.Current.Name
                    });
            }
            return list;
        }

        public class QqInfo
        {
            public string Name { get; set; }
        }
    }
}