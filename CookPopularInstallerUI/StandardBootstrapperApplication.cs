using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CookPopularInstallerUI
{
    public class StandardBootstrapperApplication : BootstrapperApplication
    {
        private static Dispatcher _dispatcher;
        public static InstallState InstallState { get; private set; }
        public static void InvokeShutdown() => _dispatcher.InvokeShutdown();
        public static void InvokeAsync(Action action) => _dispatcher.InvokeAsync(action, DispatcherPriority.Normal);

        private int LaunchUI()
        {
            // 设置WPF Application的资源程序集，避免WPF自己找不到。
            Application.ResourceAssembly = Assembly.GetExecutingAssembly();

            var app = new App();
            app.InitializeComponent();
            return app.Run();
        }

        protected override void Run()
        {
            if (Environment.GetCommandLineArgs().Contains("-debug", StringComparer.OrdinalIgnoreCase))
            {
                Debugger.Launch();
            }

            try
            {
                _dispatcher = Dispatcher.CurrentDispatcher;
                var agent = BootstrapperApplicationAgent.GetBootstrapperApplication(this);

                Engine.Detect();
                LaunchUI();
                Dispatcher.Run();

                Engine.Log(LogLevel.Verbose, "The CookPopularInstaller is Installed Successfully");
                Engine.Log(LogLevel.Verbose, $"FinalResult's value is {agent.FinalResult}");
                Engine.Quit(agent.FinalResult);
            }
            catch (Exception ex)
            {
                Engine.Log(LogLevel.Standard, $"The CookPopularInstaller is failed: {ex}");
                Engine.Quit(0);
            }
            finally
            {
                Engine.Log(LogLevel.Verbose, "The CookPopularInstaller has exited.");
            }
        }

        protected override void OnDetectBegin(DetectBeginEventArgs args)
        {
            base.OnDetectBegin(args);

            var wixBundleProviderKey = Engine.StringVariables["WixBundleProviderKey"];
            var keyPath = $@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{wixBundleProviderKey}";
            var registerKey = RegisterHelper.GetRegistryKey(keyPath);
            if(registerKey != null)
            {

            }
        }

        protected override void OnDetectComplete(DetectCompleteEventArgs args)
        {
            base.OnDetectComplete(args);

            Engine.Log(LogLevel.Verbose, $"**********DetectComplete---Status:{args.Status}**********");
            
            if (Command.Action == LaunchAction.Uninstall)
            {
                Engine.Log(LogLevel.Verbose, "Invoking automatic plan for uninstall");
                Engine.Plan(LaunchAction.Uninstall);
            }
        }

        //重新定义BootstrapperApplication的安装包验证方式，判断是当前msi安装包是否已经安装过
        protected override void OnDetectPackageComplete(DetectPackageCompleteEventArgs args)
        {
            base.OnDetectPackageComplete(args);

            Engine.Log(LogLevel.Verbose, $"**********DetectPackageComplete---PackageId:{args.PackageId};State:{args.State};Status:{args.Status}**********");
            
            if (args.PackageId.Equals(App.CookPopularInstallerMsi, StringComparison.Ordinal))
            {
                InstallState = args.State == PackageState.Present ? InstallState.Present : InstallState.NotPresent;
            }
        }

        //检查安装文件是否为空
        protected override void OnResolveSource(ResolveSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.DownloadSource))
            {
                // Downloadable package found 
                e.Result = Result.Download;
            }
            else
            {
                // Not downloadable 
                e.Result = Result.Ok;
            }
        }
    }
}
