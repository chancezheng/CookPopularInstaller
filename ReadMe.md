### Ϊ�˷����Զ��������ߣ����˿�����һ������Wix��������Ѵ�����ߣ�Ŀǰ�ǻ���VS2022(MSBuild)��Shell�ű����ɵİ�װ�������Զ�����.NetFramewrok�汾���Զ�������ȣ����ϻᷢ�����滯�������ɰ�װ���Ĺ��ߣ������ڴ�����������ɼ�QQȺ������658794308����ӭ��Ҳ��뿪����

### ��װ�����Ϊ������
- ��ӭ����
- �û����
- ��װ���
- ��װ����
- ��װ���

### 1.ԭ��Msi��װ��
![Image](Resources/Demos/OriginalUIMsi/original_msi1.png)
![Image](Resources/Demos/OriginalUIMsi/original_msi2.png)
![Image](Resources/Demos/OriginalUIMsi/original_msi3.png)
![Image](Resources/Demos/OriginalUIMsi/original_msi4.png)
![Image](Resources/Demos/OriginalUIMsi/original_msi5.png)

### 2.ԭ��Exe��װ��
![Image](Resources/Demos/OriginalUIExe/original_exe1.png)
![Image](Resources/Demos/OriginalUIExe/original_exe2.png)
![Image](Resources/Demos/OriginalUIExe/original_exe3.png)

### 3.�Զ���Exe��װ��
![Image](Resources/Demos/CustomUIExe/custom_exe1.png)
![Image](Resources/Demos/CustomUIExe/custom_exe2.png)
![Image](Resources/Demos/CustomUIExe/custom_exe3.png)
![Image](Resources/Demos/CustomUIExe/custom_exe4.png)
![Image](Resources/Demos/CustomUIExe/custom_exe5.png)


### �鿴���ճ���������־
    ���ɰ�װ����ɺ󣬽��밲װ��·����ʹ������"\CookPopularInstaller.exe -l "log.log"��������־�ļ�

### ��������ļ���wxs��
    "C:\Program Files (x86)\WiX Toolset v3.11\bin\heat.exe" dir . -o out.wxs -cg MyComponentGroup -sfrag -gg -g1

### ���ɰ�װ��
    "C:\Program Files (x86)\WiX Toolset v3.11\bin\light.exe" -ext WixUIExtension -cultures:en-us -dWixUILicenseRtf=Resources\license.rtf CookPopularInstaller.Msi.wixobj -out CookPopularInstaller.msi

### ע���ɾ��Ӧ��
    �����\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall  ���������й�
    �����\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products  ��װ������Ƿ�װ�й�
    �����\HKEY_CLASSES_ROOT\Installer\Dependencies\��װ���ľ���ID����guid
    �����\HKEY_CLASSES_ROOT\Installer\Products\��װ���ľ���ID�ĵ���