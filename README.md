说明：当选择二进制模式导出的图集方可使用此工具

# 使用步骤

1、把由FGUI导出的资源放到同级目录下，一般有一个xx_fui.bytes 和 若干张大图，例如：xx_atlas0.png、xx_atlas1.png

2、下载本项目源码编译出dll或直接在你的VS工程里引用本工程

3、开始代码：

~~~c#
using FGUIAtlasSpliter;
class Program{
    
	static void Main(string[] args)
    {
        var bytePath = @"D:\Atlas\bag_fui.bytes";
        var exportDir = @"D:\UISources";
		UIPackageTool.SplitAtlas(bytePath, exportDir);
    }
}
~~~

4、如果没有报错，一般会在指定的导出目录下生成散图

