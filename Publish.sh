#!/bin/bash

# 定义打包项目发布的路径
PublishDir="Publish" #相对路径

HeatExePath="C:\Program Files (x86)\WiX Toolset v3.11\bin\heat.exe"
DirectoryWxiFilePath="CookPopularInstaller.Msi\Directory.wxi"
UpdateWxiScriptPath="CookPopularInstaller.Msi\update_wxi.py"
MSBuildExePath="C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"

# 1.项目发布

# 1.1 发布打包项目
dotnet publish "TestApp\TestApp.csproj" -f net461 -p:Configuration=Release -p:AllowedReferenceRelatedFileExtensions=none -p:PublishProfile=Net461
# dotnet publish TestApp\TestApp.csproj -f net461 -p:Configuration=Release -p:AllowedReferenceRelatedFileExtensions=none -p:PublishProfileFullPath=TestApp\Properties\PublishProfiles\Net461.pubxml
# dotnet publish TestApp\TestApp.csproj -f net6.0-widnows -p:Configuration=Release -p:AllowedReferenceRelatedFileExtensions=none -p:PublishProfile=Net6

# 1.2 将dll全部移动到Common文件夹中
mkdir $PublishDir/Common
mv $PublishDir/*.dll $PublishDir/Common

# 2 生成MSI/EXE安装包

# 2.1 批量生成所有安装目录文件结构
"$HeatExePath" dir $PublishDir -out $DirectoryWxiFilePath -gg -gl -sreg -scom -srd -platform x64 -template fragment dr INSTALLFOLDER -cg DependencyLibrariesGroup -var var.DependencyLibraryDir

# 2.2 python脚本更改Directory.wxi文件中的某些属性
python $UpdateWxiScriptPath

# 2.3 MSBuild生成安装包或light.exe生成
"$MSBuildExePath" -p:Configuration=Release CookPopularInstaller.sln

# 运行完不关闭窗口，类似bat中pause效果
read -n 1