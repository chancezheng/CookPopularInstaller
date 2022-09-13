using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularInstallerUI
{
    public class RegisterHelper
    {
        private static readonly string X86_64 = @"SOFTWARE\WOW6432Node\Microsoft\NCATest";
        private static readonly string X86 = @"SOFTWARE\Microsoft\NCATest";
        private static readonly string ProductName = "CookPopularInstaller"; //对应注册表中RegistryName

        /// <summary>
        /// 查找注册表看是否已经安装本产品的其他版本
        /// </summary>
        /// <returns></returns>
        public static bool ChekExistFromRegistry()
        {
            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    //64位
                    using (RegistryKey pathKey = Registry.LocalMachine.OpenSubKey(X86_64))
                    {
                        var strs = pathKey.GetSubKeyNames();
                        if(strs.Contains(ProductName))
                            return true;
                    }
                }
                else
                {
                    //32位
                    using (RegistryKey pathKey = Registry.LocalMachine.OpenSubKey(X86))
                    {
                        var strs = pathKey.GetSubKeyNames();
                        if (strs.Contains(ProductName))
                            return true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public static RegistryKey GetRegistryKey(string keyPath, bool throwEx = false)
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(keyPath, false);

            if (registryKey == null && throwEx)
            {
                throw new KeyNotFoundException("Not found the value of key from registry");
            }

            return registryKey;
        }

        public static RegistryKey GetAppBaseKey()
        {
            return GetRegistryKey(@"Software\WOW6432Node\CookCSharp\CookPopularInstaller");
        }

        public static bool TryGetAppPath(out string path)
        {
            path = string.Empty;

            try
            {
                using (RegistryKey registryKey = GetAppBaseKey())
                {
                    if (registryKey == null) return false;

                    object value = registryKey.GetValue("Path");
                    if (value == null) return false;

                    path = value.ToString();

                    return !string.IsNullOrEmpty(path);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
