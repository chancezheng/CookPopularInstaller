using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularInstallerUI.ViewModels
{
    public class DetectInfoViewModel : ViewModelBase
    {
        public string DetectMessage => "您的计算机已经安装了其它版本，请前往控制面板进行卸载或检查注册表值是否未删除";
        public DelegateCommand SureCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnSureAction)).Value;

        private void OnSureAction()
        {
            StandardBootstrapperApplication.InvokeShutdown();
        }
    }
}
