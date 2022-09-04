### 安装界面分为七步：
- 欢迎界面
- 用户许可
- 安装组件
- 安装目录
- 安装进度
- 安装完成
### 查看按照程序运行日志
    生成安装包完成后，进入安装包路径，使用命令"\CookPopularInstaller.exe -l "log.log"可生成日志文件

### 批量添加文件到wxs中
    "C:\Program Files (x86)\WiX Toolset v3.11\bin\heat.exe" dir . -o out.wxs -cg MyComponentGroup -sfrag -gg -g1

### 生成安装包
    "C:\Program Files (x86)\WiX Toolset v3.11\bin\light.exe" -ext WixUIExtension -cultures:en-us -dWixUILicenseRtf=Resources\license.rtf CookPopularInstaller.Msi.wixobj -out CookPopularInstaller.msi

### 注册表删除应用
    计算机\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall  与控制面板有关
    计算机\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products  安装包检测是否安装有关
    计算机\HKEY_CLASSES_ROOT\Installer\Dependencies\安装包的具体ID，即guid
    计算机\HKEY_CLASSES_ROOT\Installer\Products\安装包的具体ID的倒序