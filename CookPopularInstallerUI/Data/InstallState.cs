using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Description：InstallState
 * Author：chance
 * Organization: CookCSharp
 * Create Time：2022-05-08 00:53:39
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) 2022 All Rights Reserved.
 */
namespace CookPopularInstallerUI
{
    /// <summary>
    /// 应用程序的安装状态
    /// </summary>
    public enum InstallState
    {
        /// <summary>
        /// 正在初始化
        /// </summary>
        Initializing,
        /// <summary>
        /// 已安装
        /// </summary>
        Present,
        /// <summary>
        /// 未安装
        /// </summary>
        NotPresent,
        /// <summary>
        /// 安装中或卸载中
        /// </summary>
        Applying,
        /// <summary>
        /// 取消安装
        /// </summary>
        Cancelled,
        /// <summary>
        /// 安装完成或卸载完成
        /// </summary>
        Applyed,
        /// <summary>
        /// 安装失败
        /// </summary>
        Failed,
    }
}
