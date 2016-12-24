using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WindowsHotKeyForm
{
    public partial class Form1 : Form
    {
        string appName = "WindowsHotKeyForm";
        public Form1()
        {
            Log.logPath = "E:\\java\\HotKeyForm.log";
            RegConfig.appName = appName;

            Log.debug("OnCreate ");
            InitializeComponent();
            InitComponent();
        }

        private void InitComponent()
        {
            checkBox1.Checked = RegConfig.getValue("autorun", 0) == 1;
            checkBox2.Checked = RegConfig.getValue("hasRegisterHotKey", 0) == 1;
            setHotKey();
            label2.Text = "启动程序 : " + RegConfig.getValue("openFile", "cmd");
            //设置自启动
            AutoRun.setAutoRun(appName, Application.ExecutablePath, checkBox1.Checked);
            //注册热键
            if (RegConfig.getValue("hasRegisterHotKey", 0) == 1)
            {
                registerHotKey();
            }
            //隐藏主窗口
            if (RegConfig.getValue("hide", 0) == 1)
            {//这里之所以用一个线程是因为程序刚启动的时候调用隐藏窗口不起作用
                new Thread(new ThreadStart(delyHide)).Start();
            }
        }



        private long lastTime = 0;
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;//如果m.Msg的值为0x0312那么表示用户按下了热键
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToString())
                    {
                        case "100":
                            string openFile = RegConfig.getValue("openFile", "cmd");
                            Log.debug("press hotkey Ctrl+Shift+X open " + openFile);
                            if (File.Exists(openFile))
                            {
                                System.Diagnostics.Process.Start(openFile);
                            }
                            else
                            {
                                MessageBox.Show("文件“" + openFile + "”不存在！");
                            }
                            if (DateTime.Now.ToFileTimeUtc() - lastTime < 2000000)
                            {
                                RegConfig.saveValue("hide", 0);
                                this.Show();
                            }
                            lastTime = DateTime.Now.ToFileTimeUtc();
                            break;

                        default: break;
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.debug("OnClosing : " + e.ToString());
        }

        //其它线程中回掉主线程
        delegate void hideForm(object sender, EventArgs e);

        private void delyHide()
        {
            Thread.Sleep(100);

            if (this.InvokeRequired)
            {
                hideForm d = new hideForm(button3_Click);
                this.Invoke(d, new object[] { null, null });
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//隐藏窗口
            if (RegConfig.getValue("hasRegisterHotKey", 0) == 1)
            {
                RegConfig.saveValue("hide", 1);
                this.Hide();
                Log.debug("hide form ...");
            }
            else
            {
                MessageBox.Show("注册热键后才能隐藏窗口！");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//开机启动选择框改变事件
            Log.debug("CheckedChanged value:" + checkBox1.Checked);
            Log.debug("ExecutablePath:" + Application.ExecutablePath);
            AutoRun.setAutoRun(appName, Application.ExecutablePath, checkBox1.Checked);
            RegConfig.saveValue("autorun", checkBox1.Checked ? 1 : 0);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {//textBox失去焦点
            textBox1.Hide();
        }

        private bool ctrl = false;
        private bool shift = false;
        private string keyCode = "";
        private Keys key = Keys.X;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Log.debug("KeyDown : " + e.KeyCode);
            string k = "";
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    if (ctrl)
                        break;
                    ctrl = true;
                    k += ctrl ? "Ctrl+" : "";
                    k += shift ? "Shift+" : "";
                    k += keyCode;
                    textBox1.Text = k;
                    break;
                case Keys.ShiftKey:
                    if (shift)
                        break;
                    shift = true;
                    k += ctrl ? "Ctrl+" : "";
                    k += shift ? "Shift+" : "";
                    k += keyCode;
                    textBox1.Text = k;
                    break;
                case Keys.Back:
                    ctrl = false;
                    shift = false;
                    keyCode = "";
                    textBox1.Text = "";
                    break;
                default:
                    if (keyCode.Equals(e.KeyCode.ToString()))
                        break;
                    keyCode = e.KeyCode.ToString();
                    k += ctrl ? "Ctrl+" : "";
                    k += shift ? "Shift+" : "";
                    k += keyCode;
                    key = e.KeyCode;
                    textBox1.Text = k;
                    break;
            }
        }

        private void setHotKey()
        {//只是保存热键信息
            ctrl = RegConfig.getValue("ctrl", 1) == 1;
            shift = RegConfig.getValue("shift", 1) == 1;
            key = (Keys)RegConfig.getValue("keyCode", (int)Keys.X);
            string k = "";
            k += ctrl ? "Ctrl+" : "";
            k += shift ? "Shift+" : "";
            k += key;
            textBox1.Text = k;
            label1.Text = "快捷键 : " + k;
            textBox1.Hide();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Log.debug("KeyUp : " + e.KeyCode);
            string k = "";
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    ctrl = false;
                    k += ctrl ? "Ctrl+" : "";
                    k += shift ? "Shift+" : "";
                    k += keyCode;
                    textBox1.Text = k;
                    break;
                case Keys.ShiftKey:
                    shift = false;
                    k += ctrl ? "Ctrl+" : "";
                    k += shift ? "Shift+" : "";
                    k += keyCode;
                    textBox1.Text = k;
                    break;
                case Keys.Back:
                    ctrl = false;
                    shift = false;
                    textBox1.Text = "";
                    break;
                default:
                    keyCode = e.KeyCode.ToString();
                    k += ctrl ? "Ctrl+" : "";
                    k += shift ? "Shift+" : "";
                    k += keyCode;
                    RegConfig.saveValue("ctrl", ctrl ? 1 : 0);
                    RegConfig.saveValue("shift", shift ? 1 : 0);
                    RegConfig.saveValue("keyCode", (int)e.KeyCode);
                    setHotKey();
                    if (RegConfig.getValue("hasRegisterHotKey", 0) == 1)
                    {//先取消再注册
                        HotKey.UnregisterHotKey(this.Handle, 100);
                        registerHotKey();
                    }
                    break;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {//设置是否注册热键
            if (checkBox2.Checked)
            {//注册热键
                registerHotKey();
            }
            else
            {//取消注册热键
                var result = HotKey.UnregisterHotKey(this.Handle, 100);
                if (result == false)
                {
                    MessageBox.Show("取消注册失败");
                }
                else
                {
                    RegConfig.saveValue("hasRegisterHotKey", 0);
                }
            }
        }

        private bool registerHotKey()
        {//注册热键
            var mod = (ctrl && shift) ? HotKey.KeyModifiers.Ctrl | HotKey.KeyModifiers.Shift :
                                ctrl ? HotKey.KeyModifiers.Ctrl :
                                shift ? HotKey.KeyModifiers.Shift : 0;
            var result = HotKey.RegisterHotKey(this.Handle, 100, mod, key);
            if (result == false)
            {
                RegConfig.saveValue("hasRegisterHotKey", 0);
                MessageBox.Show("注册失败");
                Log.debug("注册失败");
            }
            else
            {
                RegConfig.saveValue("hasRegisterHotKey", 1);
            }
            return result;
        }

        private void label1_Click(object sender, EventArgs e)
        {//配置热键
            textBox1.Show();
            textBox1.Focus();
        }

        private void label2_Click(object sender, EventArgs e)
        {//选择要启动的程序
            textBox1.Hide();
            openFileDialog1.ShowDialog();
            string fileName = openFileDialog1.FileName;
            if (File.Exists(fileName))
            {
                RegConfig.saveValue("openFile", fileName);
                Log.debug("select run app : " + fileName);
                label2.Text = "启动程序 : " + fileName;
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            textBox1.Hide();
        }
    }
}
