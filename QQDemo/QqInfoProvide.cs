using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Xml.Schema;

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

        public static void SendMessage(string msg)
        {
            var process = Process.GetProcessesByName("QQ");
            if (process.Length == 0)
                return;

            var aeTop = AutomationElement.RootElement;

            var aeForm = aeTop.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.IsEnabledProperty, true));


           

            Thread.Sleep(100);
            for (int i = 0; i < aeForm.Count; i++)
            {
                try
                {
                    if (aeForm[i].Current.AutomationId == "InputBox")
                    {
                        //先发送文本，后寻找发送按钮模拟点击
                        aeForm[i].SetFocus();
                        System.Windows.Forms.SendKeys.SendWait(msg);

                        //在UI目录树中找到TXGuiFoundation
                        AutomationElement aeForm1 = aeTop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.IsEnabledProperty, true));

                        if (aeForm1 != null)
                        {
                            AutomationElementCollection aeAllEdit1 = aeForm1.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));
                            Thread.Sleep(100);
                            for (int t = 0; t < aeAllEdit1.Count; t++)
                            {
                                try
                                {
                                    if (aeAllEdit1[t].Current.AutomationId == "send")
                                    {
                                        InvokePattern ipClickButton1 = (InvokePattern)aeAllEdit1[t].GetCurrentPattern(InvokePattern.Pattern);
                                        ipClickButton1.Invoke();

                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                   MessageBox.Show(ex.Message);
                }
            }
        }
    }
}