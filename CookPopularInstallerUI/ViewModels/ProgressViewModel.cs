using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.AxHost;



/*
 * Description：ProgressViewModel
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-09 09:37:46
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI.ViewModels
{
    public class ProgressViewModel : ViewModelBase
    {
        public string CurrentState
        {
            get
            {
                if (App.GetInstallState() == InstallState.Applying)
                    return "安装中：";
                else if (App.GetInstallState() == InstallState.Cancelled)
                    return "取消中：";
                else
                    return "Applying：";
            }
        }
        public string Message { get; set; }
        public int Progress { get; set; }
        public BitmapSource BackImage
        {
            get
            {
                Bitmap bmp = Properties.Resources.semiconductor;
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                    IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            }
        }


        private static readonly object LockInstanceObject = new object();
        private int _cacheProgress;
        private int _executeProgress;


        public ProgressViewModel()
        {
            BootstrapperAgent.BootstrapperApplication.ExecutePackageBegin += BootstrapperApplication_ExecutePackageBegin;
            BootstrapperAgent.BootstrapperApplication.CacheAcquireProgress += BootstrapperApplication_CacheAcquireProgress;
            BootstrapperAgent.BootstrapperApplication.ExecuteProgress += BootstrapperApplication_ExecuteProgress;
            BootstrapperAgent.BootstrapperApplication.ExecuteMsiMessage += BootstrapperApplication_ExecuteMsiMessage;
            BootstrapperAgent.BootstrapperApplication.ExecutePackageComplete += BootstrapperApplication_ExecutePackageComplete;
            BootstrapperAgent.BootstrapperApplication.ApplyComplete += BootstrapperApplication_ApplyComplete;
        }

        private void BootstrapperApplication_ExecutePackageBegin(object sender, ExecutePackageBeginEventArgs e)
        {
            BootstrapperAgent.LogMessage($"******ExecutePackageBegin---PackageId:{e.PackageId};Result:{e.Result};ShouldExecute:{e.ShouldExecute}*********");

            if (e.PackageId.Equals(App.DotnetFramework48, StringComparison.Ordinal))
            {
                Message = "Microsoft .NET Framework 4.8 Setup";
            }

            e.Result = App.GetInstallState() == InstallState.Cancelled ? Result.Cancel : Result.Ok;
        }

        private void BootstrapperApplication_CacheAcquireProgress(object sender, CacheAcquireProgressEventArgs e)
        {
            _cacheProgress = e.OverallPercentage;
            Progress = (_cacheProgress + _executeProgress) / 2;
        }

        private void BootstrapperApplication_ExecuteProgress(object sender, ExecuteProgressEventArgs e)
        {
            _executeProgress = e.OverallPercentage;
            Progress = (_cacheProgress + _executeProgress) / 2;
        }

        private void BootstrapperApplication_ExecuteMsiMessage(object sender, ExecuteMsiMessageEventArgs e)
        {
            lock (LockInstanceObject)
            {
                if (e.MessageType == InstallMessage.ActionStart)
                {
                    Message = e.Message;
                }

                e.Result = App.GetInstallState() == InstallState.Cancelled ? Result.Cancel : Result.Ok;

                string extendMsg = string.Empty;
                e.Data.ToList().ForEach(s => extendMsg += (s + "&"));
                BootstrapperAgent.LogMessage($"---------------Message:{e.Message},MessageType:{e.MessageType},ExtendMessage:{extendMsg}---------------");
            }
        }

        private void BootstrapperApplication_ExecutePackageComplete(object sender, ExecutePackageCompleteEventArgs e)
        {
            BootstrapperAgent.LogMessage($"************ExecutePackageComplete---PackageId:{e.PackageId};Restart:{e.Restart};Status:{e.Status};Result:{e.Result}************");

            e.Result = App.GetInstallState() == InstallState.Cancelled ? Result.Cancel : Result.Ok;
        }

        private void BootstrapperApplication_ApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            BootstrapperAgent.LogMessage($"************ApplyComplete---Restart:{e.Restart};Status:{e.Status};Result:{e.Result}************");

            App.SetInstallState(InstallState.Applyed);
            BootstrapperAgent.FinalResult = e.Status;
        }
    }
}
