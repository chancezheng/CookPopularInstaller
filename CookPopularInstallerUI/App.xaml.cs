using CookPopularInstallerUI.ViewModels;
using CookPopularInstallerUI.Views;
using CSharp.Communal.Logger;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace CookPopularInstallerUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public static TaskScheduler UITaskScheduler { get; private set; }
        private static InstallState _state;
        private static bool _isUninstalled;
        public static readonly string CookPopularInstallerMsi = "CookPopularInstaller.Msi";
        public static readonly string DotnetFramework48 = "Netfx48Full";


        static App()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var location = assembly.Location;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(location));

            Log4Net.Info($"*************程序启动 v{FileVersionInfo.GetVersionInfo(location).FileVersion}***************{Environment.NewLine}{GetSystemInfo(assembly)}");
        }

        public App()
        {           
#if RELEASE
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Fatal("应用程序域中存在引发但未处理异常", e.ExceptionObject as Exception);
            DispatcherUnhandledException += (sender, e) => Fatal("应用程序存在引发但未处理异常", e.Exception);
            TaskScheduler.UnobservedTaskException += (sender, e) => Fatal("任务中存在引发但未处理异常", e.Exception);
#endif
        }

#if RELEASE
        private void Fatal(object message, Exception ex)
        {
            Log4Net.Fatal(message, ex);
            Environment.Exit(0);
        }
#endif

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<InstallView>();
            containerRegistry.RegisterForNavigation<LicenseView>();
            containerRegistry.RegisterForNavigation<ProgressView>();
            containerRegistry.RegisterForNavigation<FinishView>();
            containerRegistry.RegisterForNavigation<ChangeRepairUninstallView>();
            containerRegistry.RegisterForNavigation<DetectInfoView>();
            containerRegistry.RegisterForNavigation<UninstallView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UITaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log4Net.Info($"*************************************程序退出*****************************************{Environment.NewLine}");

            base.OnExit(e);
        }

        private static string GetSystemInfo(Assembly assembly)
        {
            var value = new StringBuilder();
            var newLine = Environment.NewLine;
            var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

            value.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", Environment.OSVersion.VersionString, newLine);
            value.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", (assembly.GetCustomAttribute<AssemblyDescriptionAttribute>() is AssemblyDescriptionAttribute attribute) ? attribute.Description : "Last Changed Reversion: ****", newLine);
            value.AppendFormat(CultureInfo.InvariantCulture, "{0}位操作系统，{1}位进程{2}", Environment.Is64BitOperatingSystem ? "64" : "32", Environment.Is64BitProcess ? "64" : "32", newLine);
            value.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", RuntimeInformation.FrameworkDescription, newLine);
            value.AppendFormat(CultureInfo.InvariantCulture, "分辨率大小：{0}*{1}", primaryScreen.Width, primaryScreen.Height);

            return value.ToString();
        }

        private static IEnumerable<PropertyOrFieldServiceInfo> DeclaredPublicProperties(Request request)
        {
            return (request.ImplementationType ?? request.ServiceType).GetTypeInfo()
                    .DeclaredProperties.Where(p => p.IsInjectable())
                    .Select(PropertyOrFieldServiceInfo.Of);
        }

        public static void SetInstallState(InstallState state)
        {
            _state = state;
            BootstrapperApplicationAgent.Instance.OnStateValueChanged(state);
        }

        public static InstallState GetInstallState() => _state;

        public static void SetUninstalled() => _isUninstalled = true;

        public static bool GetIfUninstalled() => _isUninstalled;
    }
}
