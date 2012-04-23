using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AutoMAT.Common
{
	static class RawSerializer
	{
		public static T Deserialize<T>(Stream stream)
		{
			return Deserialize<T>(stream.ReadBytes(Marshal.SizeOf(typeof(T))));
		}

		public static T Deserialize<T>(byte[] data)
		{
			return Deserialize<T>(data, 0);
		}

		public static T Deserialize<T>(byte[] data, int offset)
		{
			var size = Marshal.SizeOf(typeof(T));
			if (size > data.Length)
			{
				throw new Exception(String.Format(
					"Could not deserialize type {0}, size of type is larger than data given.",
					typeof(T).GetType().Name));
			}
			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			T result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();
			return result;
		}

		public static byte[] Serialize(Object obj)
		{
			var size = Marshal.SizeOf(obj);
			var data = new byte[size];
			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			Marshal.StructureToPtr(obj, handle.AddrOfPinnedObject(), false);
			handle.Free();
			return data;
		}
	}
}
