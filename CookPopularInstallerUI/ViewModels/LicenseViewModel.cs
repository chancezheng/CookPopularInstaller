using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;



/*
 * Description：LicenseViewModel
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-06 11:06:32
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI.ViewModels
{
    public class LicenseViewModel : ViewModelBase
    {
        private string _licenseContent;
        public string LicenseContent
        {
            get => _licenseContent;
            set => SetProperty(ref _licenseContent, value);
        }

        public DelegateCommand<string> AgreeLicenseCommand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(AgreeLicenseAction)).Value;
        public DelegateCommand<string> DisagreeLicenseCommand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(DisagreeLicenseAction)).Value;


        public LicenseViewModel()
        {
            LicenseContent = File.ReadAllText(@"D:\WPFSourceCode\CookPopularInstaller\LICENSE");
        }

        private void AgreeLicenseAction(string navigatePath)
        {
            NavigateToSourcePath(navigatePath, true);
        }

        private void DisagreeLicenseAction(string navigatePath)
        {
            NavigateToSourcePath(navigatePath, false);
        }

        private void NavigateToSourcePath(string navigatePath, bool isAgreeLicense)
        {
            if (!string.IsNullOrEmpty(navigatePath))
                RegionManager.RequestNavigate(RegionToken.MainWindowContentRegion, navigatePath);

            EventAggregator.GetEvent<Events>().Publish(isAgreeLicense);
        }
    }
}
