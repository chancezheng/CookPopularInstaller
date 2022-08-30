import xml.etree.ElementTree as ET
import sys

wxi = "Directory.wxi"
app = "TestApp.exe"

def updateAppFileId():
    #读取文件
    tree = ET.parse(wxi)
    #获取根节点
    root = tree.getroot()
    root.tag = "Include"
    #使用XPath语法寻址
    fragments = root.findall("{http://schemas.microsoft.com/wix/2006/wi}Fragment")
    componentGroup = fragments[1].findall("{http://schemas.microsoft.com/wix/2006/wi}ComponentGroup")
    components = componentGroup[0].findall("{http://schemas.microsoft.com/wix/2006/wi}Component")
    for component in components:
        files = component.findall("{http://schemas.microsoft.com/wix/2006/wi}File")
        source = files[0].get("Source")
        if source == u'$(var.DependencyLibraryDir)\\' + f'{app}':
            print(files[0].tag)
            print(files[0].attrib)
            files[0].set("Id", "App.exe")
            break

    tree = ET.ElementTree(root)
    #注册命名控件前缀
    ET.register_namespace('',"http://schemas.microsoft.com/wix/2006/wi")
    tree.write(wxi, encoding="utf-8", xml_declaration=True)


if __name__=="__main__":
    updateAppFileId()
