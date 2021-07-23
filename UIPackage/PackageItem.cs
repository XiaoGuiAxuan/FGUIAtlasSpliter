using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class PixelHitTestData
{
    public int pixelWidth;
    public float scale;
    public byte[] pixels;
    public int pixelsLength;
    public int pixelsOffset;

    public void Load(ByteBuffer ba)
    {
        ba.ReadInt();
        pixelWidth = ba.ReadInt();
        scale = 1.0f / ba.ReadByte();
        pixels = ba.buffer;
        pixelsLength = ba.ReadInt();
        pixelsOffset = ba.position;
        ba.Skip(pixelsLength);
    }
}
/// <summary>
/// 
/// </summary>
public class PackageItem
{
    public UIPackage owner;

    public PackageItemType type;
    public ObjectType objectType;

    public string id;
    public string name;
    public int width;
    public int height;
    public string path;
    public string file;
    public bool exported;
    //public NTexture texture;
    public ByteBuffer rawData;
    public string[] branches;
    public string[] highResolution;

    //image
    public Rect? scale9Grid;
    public bool scaleByTile;
    public int tileGridIndice;
    public PixelHitTestData pixelHitTestData;


    public PackageItem getBranch()
    {
        if (branches != null && owner._branchIndex != -1)
        {
            string itemId = branches[owner._branchIndex];
            if (itemId != null)
                return owner.GetItem(itemId);
        }

        return this;
    }

    public PackageItem getHighResolution()
    {
        var contentScaleLevel = 0;
        if (highResolution != null && contentScaleLevel > 0)
        {
            int i = contentScaleLevel - 1;
            if (i >= highResolution.Length)
                i = highResolution.Length - 1;
            string itemId = highResolution[i];
            if (itemId != null)
                return owner.GetItem(itemId);
        }

        return this;
    }
}
