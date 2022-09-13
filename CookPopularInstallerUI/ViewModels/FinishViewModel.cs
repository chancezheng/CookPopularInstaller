using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;



/*
 * Description：FinishViewModel
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-09 11:02:02
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI.ViewModels
{
    public class FinishViewModel : ViewModelBase
    {
        public BitmapSource BackImage
        {
            get
            {
                Bitmap bmp = Properties.Resources.finish;
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                    IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            }
        }

        public bool IsRunApp { get; set; } = true;
        public bool IsUninstalled => App.GetIfUninstalled();

        public DelegateCommand FinishInstallCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(FinishInstallAction)).Value;

        private void FinishInstallAction()
        {
            if(IsRunApp && !IsUninstalled)
            {
                RegisterHelper.TryGetAppPath(out string appPath);
                Process.Start(appPath);
            }

            StandardBootstrapperApplication.InvokeShutdown();
        }
    }
}
