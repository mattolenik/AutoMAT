using System;
using System.Runtime.InteropServices;

namespace AutoMAT.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MatHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
        public byte[] Magic;				// Must/should be 'MAT '
        public Int32 Version;				// Should be 0x32

        public Int32 Type;					// 0 = color, 1 = ?, 2 = texture
        public Int32 MatRecordCount;		// Number of MAT records
        public Int32 TextureCount;			// Number of textures

        // Surface format should be built from the below values:
        public Int32 Transparency;			// Unknown (perhaps transparency?)

        public Int32 Bitdepth;				// Number of bits per pixel, valid formats: 8 (converted), 16, 32

        public Int32 BlueBits;				// 0, 5, 8
        public Int32 GreenBits;				// 0, 6 (16-bit 565), 5 (16-bit 1555), 8
        public Int32 RedBits;				// 0, 5, 8

        public Int32 RedShl;
        public Int32 GreenShl;
        public Int32 BlueShl;

        public Int32 RedShr;
        public Int32 GreenShr;
        public Int32 BlueShr;

        public Int32 Unknown1;
        public Int32 Unknown2;
        public Int32 Unknown3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MatRecordHeader
    {
        public Int32 RecordType;			// 0 = color, 8 = texture
        public Int32 TransparentColor;		// if 16-bit or 32-bit, RGB. If 8-bit, palette offset
        public Int32 Unknown1;				// Padding, most likely
        public Int32 Unknown2;				// More padding
        public Int32 Unknown3;				// Yet more padding
        public Int32 Unknown4;				// Further padding
        public Int32 Unknown5;
        public Int32 Unknown6;
        public Int32 Unknown7;
        public Int32 Unknown8;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TextureDataHeader
    {
        public Int32 SizeX;
        public Int32 SizeY;
        public Int32 TransparentBool;		// Use transparency? std bool, 0 = false, !0 = true

        public Int32 Unknown1;
        public Int32 Unknown2;

        public Int32 MipmapCount;			// Number of mipmaps
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MatColorRecordHeader
    {
        public Int32 RecordType;			// 0
        public Int32 ColorNum;				// index into palette
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
    }
}