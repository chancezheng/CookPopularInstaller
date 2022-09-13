using CookPopularCSharpToolkit.Windows;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Prism.Regions;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.AxHost;



/*
 * Description：BootstrapperApplicationHelper
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-08 00:22:30
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI
{
    public class BootstrapperApplicationAgent
    {
        private static BootstrapperApplicationAgent _instance;
        public static BootstrapperApplicationAgent Instance => _instance;

        public BootstrapperApplication BootstrapperApplication { get; private set; }

        public int FinalResult { get; set; }

        private IntPtr hwnd = IntPtr.Zero;

        static BootstrapperApplicationAgent() { }

        private BootstrapperApplicationAgent(BootstrapperApplication bootstrapperApplication)
        {
            BootstrapperApplication = bootstrapperApplication;
            hwnd = IntPtr.Zero;
            string[] strs = GetCommandLine();
        }

        public static BootstrapperApplicationAgent GetBootstrapperApplication(BootstrapperApplication bootstrapperApplication)
        {
            if (_instance == null)
                _instance = new BootstrapperApplicationAgent(bootstrapperApplication);

            return _instance;
        }

        public void SetWindowHandle(Window win)
        {
            //var win = WindowExtension.GetActiveWindow();
            hwnd = win.EnsureHandle();
        }

        public void PlanAction(LaunchAction action)
        {
            BootstrapperApplication.Engine.Plan(action);
        }

        public void ApplyAction()
        {
            BootstrapperApplication.Engine.Apply(hwnd);
        }

        public void SetBurnVariable(string variableName, string value)
        {
            BootstrapperApplication.Engine.StringVariables[variableName] = value;
        }

        public string[] GetCommandLine()
        {
            return BootstrapperApplication.Command.GetCommandLineArgs();
        }

        public bool HelpRequested()
        {
            return BootstrapperApplication.Command.Action == LaunchAction.Help;
        }

        public void LogMessage(string message, LogLevel level = LogLevel.Standard)
        {
            BootstrapperApplication.Engine.Log(level, message);
        }

        public void OnStateValueChanged(InstallState state)
        {
            switch (state)
            {
                case InstallState.Initializing:
                    break;
                case InstallState.Present:
                    NavigateToSourceView("ChangeRepairUninstallView");
                    break;
                case InstallState.NotPresent:
                    if(RegisterHelper.ChekExistFromRegistry())
                        NavigateToSourceView("DetectInfoView");
                    else
                        NavigateToSourceView("InstallView");
                    break;
                case InstallState.Applying:
                    NavigateToSourceView("ProgressView");
                    break;
                case InstallState.Cancelled:
                    StandardBootstrapperApplication.InvokeShutdown();
                    break;
                case InstallState.Applyed:
                    NavigateToSourceView("FinishView");
                    break;
                case InstallState.Failed:
                    break;
                default:
                    break;
            }
        }

        private void NavigateToSourceView(string navigatePath)
        {
            StandardBootstrapperApplication.InvokeAsync(() =>
            {
                if (!string.IsNullOrEmpty(navigatePath))
                    ContainerLocator.Container.Resolve<IRegionManager>().RequestNavigate(RegionToken.MainWindowContentRegion, navigatePath, NavigateCompleted);
            });
        }

        private void NavigateCompleted(NavigationResult result)
        {
            LogMessage(string.Format("Navigation to {0} {1} {2}. ", result.Context.Uri, result.Result.Value, result.Error?.Message));
        }
    }
}
