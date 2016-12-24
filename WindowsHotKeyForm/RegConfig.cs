using Microsoft.Win32;
using System.Security.AccessControl;

namespace WindowsHotKeyForm
{
    /// <summary>
    /// 在注册表里保存、读取程序配置的公用类
    /// </summary>
    class RegConfig
    {
        /// <summary>
        /// 保存在注册表里的程序的名字
        /// </summary>
        public static string appName = "NO_DEF";

        /// <summary>
        /// 保存一个整数到注册表中
        /// </summary>
        /// <param name="name">项的名字</param>
        /// <param name="value">项的值</param>
        /// <returns>true:成功 false:失败</returns>
        public static bool saveValue(string name, int value)
        {
            RegistryKey software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
            var app = software.CreateSubKey(appName);
            app.SetValue(name, value);
            app.Close();
            software.Close();
            return true;
        }
        /// <summary>
        /// 获取一个整数配置
        /// </summary>
        /// <param name="name">配置的名字</param>
        /// <param name="defValue">配置的默认值</param>
        /// <returns></returns>
        public static int getValue(string name, int defValue)
        {
            try
            {
                RegistryKey software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                var app = software.CreateSubKey(appName);
                int value= (int)app.GetValue(name, defValue);
                app.Close();
                software.Close();
                return value;
            }
            catch { return defValue; }

        }
        /// <summary>
        /// 保存一个字符串到注册表中
        /// </summary>
        /// <param name="name">项的名字</param>
        /// <param name="value">项的值</param>
        /// <returns>true:成功 false:失败</returns>
        public static bool saveValue(string name, string value)
        {
            RegistryKey software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
            var app = software.CreateSubKey(appName);
            app.SetValue(name, value);
            app.Close();
            software.Close();
            return true;
        }
        /// <summary>
        /// 获取一个字符串配置
        /// </summary>
        /// <param name="name">配置的名字</param>
        /// <param name="defValue">配置的默认值</param>
        /// <returns></returns>
        public static string getValue(string name, string defValue)
        {
            try
            {
                RegistryKey software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                var app = software.CreateSubKey(appName);
                string value = (string)app.GetValue(name, defValue);
                app.Close();
                software.Close();
                return value;
            }
            catch { return defValue; }

        }

    }
}
