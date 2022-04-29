using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularInstallerUI
{
    public class Program : BootstrapperApplication
    {
        [STAThread]
        public static int Main(string[] args)
        {
            // 这里的代码仅为调试使用，在最终的项目中无任何用途。
            var app = new App();
            return app.Run();
        }

        protected override void Run()
        {
            if (Environment.GetCommandLineArgs().Contains("-debug", StringComparer.OrdinalIgnoreCase))
            {
                Debugger.Launch();
            }

            // 稍后要在这里添加安装流程控制。
            Engine.Log(LogLevel.Standard, "Running the CookPopularInstaller.");
            try
            {
                LaunchUI();
                Engine.Log(LogLevel.Standard, "Exiting the CookPopularInstaller.");
                Engine.Quit(0);
            }
            catch (Exception ex)
            {
                Engine.Log(LogLevel.Error, $"The CookPopularInstaller is failed: {ex}");
                Engine.Quit(-1);
            }
            finally
            {
                Engine.Log(LogLevel.Standard, "The CookPopularInstaller has exited.");
            }
        }

        private int LaunchUI()
        {
            // 设置 WPF Application 的资源程序集，避免 WPF 自己找不到。
            Application.ResourceAssembly = Assembly.GetExecutingAssembly();

            // 正常启动 WPF Application。
            var app = new App();
            return app.Run();
        }
    }
}
