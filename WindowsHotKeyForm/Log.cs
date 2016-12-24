using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsHotKeyForm
{
    /// <summary>
    /// 打印日志的公用类
    /// </summary>
    class Log
    {
        /// <summary>
        /// 日志文件的路径
        /// </summary>
        public static string logPath = "E:\\log.txt";
        /// <summary>
        /// 打印debug日志
        /// </summary>
        /// <param name="msg">要打印的消息</param>
        public static void debug(string msg)
        {
            write(DateTime.Now.ToString() + " [DEBUG] " + msg);
        }

        /// <summary>
        /// 打印error日志
        /// </summary>
        /// <param name="msg">要打印的消息</param>
        public static void error(string msg)
        {
            write(DateTime.Now.ToString() + " [ERROR] " + msg);
        }

        private static void write(string msg)
        {
            Console.WriteLine(msg);
            FileStream fs = new FileStream(logPath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.WriteLine(msg);
            sw.Close();
            fs.Close();

        }
    }
}
