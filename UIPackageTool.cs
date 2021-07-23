using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UIPackageTool
{
    /// <summary>
    /// 切割图集
    /// </summary>
    /// <param name="byteFile">二进制路径</param>
    /// <param name="exportDir">导出的目录</param>
    /// <param name="isOverrideExists">是否覆盖已经存在的散图</param>
    public static void SplitAtlas(string byteFile, string exportDir,bool isOverrideExists = true)
    {
        // 创建导出文件夹
        string outPath = $"{exportDir}/{Path.GetFileNameWithoutExtension(byteFile)}/";
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        // bytes
        var source = File.ReadAllBytes(byteFile);
        var dir = Path.GetDirectoryName(byteFile);
        ByteBuffer buffer = new ByteBuffer(source);

        // UIPackage
        UIPackage pkg = new UIPackage();
        var mainAssetName = Path.GetFileNameWithoutExtension(byteFile);
        pkg.LoadPackage(buffer, mainAssetName);
        var sprites = pkg.sprites;

        // Atlas Map
        Dictionary<string, Bitmap> atlasMap = new Dictionary<string, Bitmap>();
        foreach (var item in pkg.GetItems())
        {
            if (item.type == PackageItemType.Atlas)
            {
                //Console.WriteLine($"加载图集：{item.file}");
                var bitmap = Image.FromFile($"{dir}/{item.file.Replace("_fui", string.Empty)}") as Bitmap;
                atlasMap.Add(item.file, bitmap);
            }
        }


        foreach (var item in sprites)
        {
            var key = item.Key;
            var atlasSprite = item.Value;

            // PackageItem
            var pItem = pkg.GetItem(key);
            if (pItem == null) continue;
            var name = pItem.name;
            var rect = atlasSprite.rect;
            var rotated = atlasSprite.rotated;
            var originalSize = atlasSprite.originalSize;
            var offset = atlasSprite.offset;
            var atlas = atlasMap[atlasSprite.atlas.file];

            var newTexPath = $"{outPath}/{name}.png";

            if (!isOverrideExists && File.Exists(newTexPath))
            {
                continue;
            }

            var width = (int)rect.width;
            var height = (int)rect.height;
            Bitmap tex = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var x = (int)rect.x;
            var y = (int)rect.y;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var _x = x + i;
                    var _y = y + j;
                    var col = atlas.GetPixel(_x, _y);
                    tex.SetPixel(i, j, col);
                }
            }

            if (rotated)
            {
                tex.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

           
            tex.Save(newTexPath);

        }
    }


}
