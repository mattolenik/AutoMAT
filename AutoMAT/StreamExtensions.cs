using System;
using System.IO;

namespace AutoMAT
{
	internal static class StreamExtensions
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
}