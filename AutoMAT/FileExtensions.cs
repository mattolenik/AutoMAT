using System;
using System.IO;

namespace AutoMAT
{
	internal static class FileExtensions
	{
		public static String BareName(this FileInfo fileInfo)
		{
			return fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf('.'));
		}
	}
}
