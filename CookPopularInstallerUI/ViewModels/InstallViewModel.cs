using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System.Diagnostics;



/*
 * Description：InstallViewModel
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-06 10:31:18
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI.ViewModels
{
    public class InstallViewModel : ViewModelBase
    {
        public bool IsAgreeLicense { get; set; }
        public string InstallFolder { get; set; }
        /// <summary>
        /// 是否创建桌面快捷方式
        /// </summary>
        public bool IsCreateShortCut { get; set; }


        public DelegateCommand QuickInstallCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(QuickInstallAction)).Value;
        public DelegateCommand<string> OpenLicenseCommand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(OpenLicenseAction)).Value;
        public DelegateCommand FolderBrowserCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(FolderBrowserAction)).Value;


        private readonly string DefaultInstallFolder = (Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)) + "\\CookCSharp";


        public InstallViewModel()
        {
            InstallFolder = DefaultInstallFolder;
            EventAggregator.GetEvent<Events>().Subscribe(b => IsAgreeLicense = b);

            BootstrapperAgent.BootstrapperApplication.PlanComplete += BootstrapperApplication_PlanComplete;
            BootstrapperAgent.BootstrapperApplication.ApplyBegin += BootstrapperApplication_ApplyBegin;
        }

        private void QuickInstallAction()
        {
            BootstrapperAgent.PlanAction(LaunchAction.Install);
        }

        private void OpenLicenseAction(string navigatePath)
        {
            if (!string.IsNullOrEmpty(navigatePath))
                RegionManager.RequestNavigate(RegionToken.MainWindowContentRegion, navigatePath);
        }

        private void FolderBrowserAction()
        {
            CreateFolder();

            var folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;
            folderDialog.Title = "选择安装路径";
            folderDialog.AllowNonFileSystemItems = true;
            folderDialog.InitialDirectory = InstallFolder;
            folderDialog.DefaultDirectory = InstallFolder;
            if (!string.IsNullOrEmpty(InstallFolder))
            {
                var fileName = Directory.GetDirectories(InstallFolder).FirstOrDefault();
                folderDialog.DefaultFileName = Path.GetFileNameWithoutExtension(fileName);
            }
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                InstallFolder = folderDialog.FileName;
            }
        }

        private void CreateFolder()
        {
            if (!string.IsNullOrEmpty(InstallFolder) && !Directory.Exists(InstallFolder))
            {
                var process = Process.Start(new ProcessStartInfo("cmd", $"/c mkdir \"{InstallFolder}\"")
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Verb = "runas",
                });

                process.WaitForExit();
                process.Close();
            }

            BootstrapperAgent?.SetBurnVariable("InstallFolder", InstallFolder);
        }

        /// <summary>
        /// 开始安装时调用的方法为BootstrapperAgent.PlanAction(LaunchAction.Install),
        /// 该方法执行完成之后触发本事件, 事件中调用ApplyAction()方法开始执行安装进程
        /// </summary>
        private void BootstrapperApplication_PlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (App.GetInstallState() == InstallState.Cancelled)
            {
                StandardBootstrapperApplication.InvokeShutdown();
                return;
            }

            BootstrapperAgent.ApplyAction();
        }

        /// <summary>
        /// 当安装进程开始时触发事件,
        /// 将当前状态更改为Applying
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BootstrapperApplication_ApplyBegin(object sender, ApplyBeginEventArgs e)
        {
            BootstrapperAgent.LogMessage($"*********开始安装了********");

            App.SetInstallState(InstallState.Applying);
        }
    }
}
