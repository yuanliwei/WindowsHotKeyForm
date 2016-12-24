using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsHotKeyForm
{
    /// <summary>
    /// 设置程序开机自动运行的公用类
    /// </summary>
    class AutoRun
    {
        /// <summary>
        /// 设置开机自动运行的程序
        /// </summary>
        /// <param name="fileName">要设置开机自动运行的程序路径</param>
        /// <param name="isAutoRun">设置是否开机自动运行 true:是 falsep:否</param>
        /// <returns></returns>
        public static bool setAutoRun(string name,string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                    throw new Exception("该文件不存在!");
                reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }

            return true;
        }
    }
}
