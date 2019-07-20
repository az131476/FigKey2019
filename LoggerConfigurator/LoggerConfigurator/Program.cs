using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoggerConfigurator
{
    static class Program
    {
        private static ApplicationContext context;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //欢迎界面
            WelcomForm sp = new WelcomForm();                        //启动窗体
            sp.Show();                                              //显示启动窗体
            context = new ApplicationContext();
            context.Tag = sp;
            Application.Idle += new EventHandler(Application_Idle); //注册程序运行空闲去执行主程序窗体相应初始化代码
            Application.Run(context);
        }
        //初始化等待处理函数
        private static void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(Application_Idle);
            if (context.MainForm == null)
            {
                MainForm mw = new MainForm();
                context.MainForm = mw;
                mw.Init();
                WelcomForm sp = (WelcomForm)context.Tag;
                sp.Close();                                 //关闭启动窗体 
                mw.Show();                                  //启动主程序窗体
            }
        }
    }
}
