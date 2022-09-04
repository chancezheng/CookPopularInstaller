#!/bin/bash

# ��������Ŀ������·��
PublishDir="Publish" #���·��

HeatExePath="C:\Program Files (x86)\WiX Toolset v3.11\bin\heat.exe"
DirectoryWxiFilePath="CookPopularInstaller.Msi\Directory.wxi"
UpdateWxiScriptPath="CookPopularInstaller.Msi\update_wxi.py"
MSBuildExePath="C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"

# 1.��Ŀ����

# 1.1 ���������Ŀ
dotnet publish "TestApp\TestApp.csproj" -f net461 -p:Configuration=Release -p:AllowedReferenceRelatedFileExtensions=none -p:PublishProfile=Net461
# dotnet publish TestApp\TestApp.csproj -f net461 -p:Configuration=Release -p:AllowedReferenceRelatedFileExtensions=none -p:PublishProfileFullPath=TestApp\Properties\PublishProfiles\Net461.pubxml
# dotnet publish TestApp\TestApp.csproj -f net6.0-widnows -p:Configuration=Release -p:AllowedReferenceRelatedFileExtensions=none -p:PublishProfile=Net6

# 1.2 ��dllȫ���ƶ���Common�ļ�����
mkdir $PublishDir/Common
mv $PublishDir/*.dll $PublishDir/Common

# 2 ����MSI/EXE��װ��

# 2.1 �����������а�װĿ¼�ļ��ṹ
"$HeatExePath" dir $PublishDir -out $DirectoryWxiFilePath -gg -gl -sreg -scom -srd -platform x64 -template fragment dr INSTALLFOLDER -cg DependencyLibrariesGroup -var var.DependencyLibraryDir

# 2.2 python�ű�����Directory.wxi�ļ��е�ĳЩ����
python $UpdateWxiScriptPath

# 2.3 MSBuild���ɰ�װ����light.exe����
"$MSBuildExePath" -p:Configuration=Release CookPopularInstaller.sln

# �����겻�رմ��ڣ�����bat��pauseЧ��
read -n 1