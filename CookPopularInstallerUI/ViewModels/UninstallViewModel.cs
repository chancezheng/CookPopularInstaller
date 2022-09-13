using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Description：UnistallViewModel
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-09 16:38:58
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI.ViewModels
{
    public class UninstallViewModel : ViewModelBase
    {
        public DelegateCommand SureCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(SureAction)).Value;

        private void SureAction()
        {
            BootstrapperAgent.PlanAction(LaunchAction.Uninstall);
        }
    }
}
