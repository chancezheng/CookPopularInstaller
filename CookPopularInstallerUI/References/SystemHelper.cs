﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;


namespace CookPopularInstallerUI
{
    public class SystemHelper
    {
        /// <summary>
        /// 为文件夹增加User、EveryOne用户组的完全控制权限
        /// </summary>
        /// <param name="folderPath"></param>
        public static void AddSecurity(string folderPath)
        {
            var dir = new DirectoryInfo(folderPath);
            //获取该文件夹的所有访问权限
            var dirSecurity = dir.GetAccessControl(AccessControlSections.All);
            //设定文件ACL继承
            var inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            //添加Everyone用户组的访问权限规则-完全控制权限
            var everyoneFileSystemAccessRule = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            //添加Users用户组的访问权限规则-完全控制权限
            var usersFileSystemAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, everyoneFileSystemAccessRule, out var isModified);
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersFileSystemAccessRule, out isModified);
            //设置访问权限
            dir.SetAccessControl(dirSecurity);
        }
    }
}
