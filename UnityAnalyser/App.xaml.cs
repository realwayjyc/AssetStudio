using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;

namespace UnityAnalyzer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            try
            {
                string s = System.AppDomain.CurrentDomain.BaseDirectory + "log\\";
                if (!Directory.Exists(s))
                {
                    Directory.CreateDirectory(s);
                }
                string file = s + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".txt";
                if (File.Exists(file))
                {
                    file = s + DateTime.Now.ToString("yyyyMMdd HHmmss") + "_1.txt";
                }
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter ws = new StreamWriter(fs);
                ws.AutoFlush = true;
                Console.SetOut(ws);

            }
            catch (Exception e)
            {

            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = (Exception)e.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + error.Message);
        }
    }
}
