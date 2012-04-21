using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;

namespace AutoMAT
{
    static class PixelFormatExtensions
    {
        public static bool HasAlpha(this PixelFormat format)
        {
            return
                format == PixelFormat.Format16bppArgb1555 ||
                format == PixelFormat.Format32bppArgb ||
                format == PixelFormat.Format64bppArgb;
        }
    }
}
