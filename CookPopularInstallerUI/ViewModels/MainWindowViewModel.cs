using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Description：MainWindowViewModel
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-06 11:14:13
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //private static readonly Lazy<MainWindowViewModel> _instance = new Lazy<MainWindowViewModel>(() => new MainWindowViewModel());
        //public static MainWindowViewModel Instance => _instance.Value;

        public DelegateCommand LoadedCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnLoadedAction)).Value;

        public MainWindowViewModel()
        {
            BootstrapperAgent.SetWindowHandle(System.Windows.Application.Current.MainWindow);
            App.SetInstallState(StandardBootstrapperApplication.InstallState);
        }

        private void OnLoadedAction()
        {

        }
    }
}
