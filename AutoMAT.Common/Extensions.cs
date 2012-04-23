using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace AutoMAT.Common
{
    public static class FileInfoExtensions
    {
        public static string BareName(this FileInfo fileInfo)
        {
            int index = fileInfo.Name.LastIndexOf('.');
            if (index > 0)
            {
                return fileInfo.Name.Substring(0, index);
            }
            return fileInfo.Name;
        }
    }

    public static class PixelFormatExtensions
    {
        public static bool HasAlpha(this PixelFormat format)
        {
            return
                format == PixelFormat.Format16bppArgb1555 ||
                format == PixelFormat.Format32bppArgb ||
                format == PixelFormat.Format64bppArgb;
        }
    }

    public static class StreamExtensions
    {
        public static byte[] ReadBytes(this Stream stream, int count)
        {
            var buffer = new byte[count];
            stream.Read(buffer, 0, count);
            return buffer;
        }

        public static void WriteBytes(this Stream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
    }

    public static class StringExtensions
    {
        public static string Format(this string str, IFormatProvider formatProvider, params object[] args)
        {
            return string.Format(formatProvider, str, args);
        }

        public static string FormatInvariant(this string str, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, str, args);
        }
    }

    public static class EnumExtensions
    {
        public static string GetName(this Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }
    }
}