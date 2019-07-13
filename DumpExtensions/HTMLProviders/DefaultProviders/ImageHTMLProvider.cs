﻿#if NETFRAMEWORK
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using VisualDump.HTMLProviderArgs;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class ImageHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<Image, ImageArgs>(Obj, CallStack, Args, (img, stack, a) =>
        {
                Bitmap[] bitmaps = new Bitmap[2];
                if (a.MaxSize != Size.Empty)
                {
                    if (img.Width > a.MaxSize.Width)
                        img = bitmaps[0] = new Bitmap(img, new Size(a.MaxSize.Width, (int)Math.Floor(img.Height * (a.MaxSize.Width / (double)img.Width))));
                    if (img.Height > a.MaxSize.Height)
                        img = bitmaps[1] = new Bitmap(img, new Size((int)Math.Floor(img.Width * (a.MaxSize.Height / (double)img.Height)), a.MaxSize.Height));
                }
            using (Bitmap bit = new Bitmap(img))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    bit.Save(m, ImageFormat.Png);
                    foreach (Bitmap x in bitmaps)
                        x?.Dispose();
                    return $"<img class='image' src='data:image/png;base64, {Convert.ToBase64String(m.ToArray())}' />";
                }
            }
        });
    }
}
#endif