using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using Prism.Ioc;
using Prism.Events;



/*
 * Description：ViewModelBase
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-09 09:59:23
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI
{
    public class ViewModelBase : BindableBase
    {
        public BootstrapperApplicationAgent BootstrapperAgent => BootstrapperApplicationAgent.Instance;

        public IRegionManager RegionManager { get; private set; }

        public IEventAggregator EventAggregator { get; private set; }

        public DelegateCommand CancelCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnCancelAction)).Value;

        private void OnCancelAction()
        {
            App.SetInstallState(InstallState.Cancelled);
            StandardBootstrapperApplication.InvokeShutdown();
        }

        public ViewModelBase()
        {
            RegionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            EventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
        }
    }
}
