### ��װ�����Ϊ�߲���
- ��ӭ����
- �û����
- ��װ���
- ��װĿ¼
- ��װ����
- ��װ���
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