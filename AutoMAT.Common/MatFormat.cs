using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoMAT.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MatHeader
    {
        public enum MatType
        {
            Color = 0,
            Unknown = 1,
            Texture = 2
        }

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
        public byte[] Magic;				// Must/should be 'MAT '
        public UInt32 Version;				// Should be 0x32

        [MarshalAs(UnmanagedType.U4)]
        public MatType Type;					// 0 = color, 1 = ?, 2 = texture
        public UInt32 MatRecordCount;		// Number of MAT records
        public UInt32 TextureCount;			// Number of textures

        // Surface format should be built from the below values:
        public UInt32 Transparency;			// Unknown (perhaps transparency?)

        public UInt32 Bitdepth;				// Number of bits per pixel, valid formats: 8 (converted), 16, 32

        public UInt32 BlueBits;				// 0, 5, 8
        public UInt32 GreenBits;	    	// 0, 6 (16-bit 565), 5 (16-bit 1555), 8
        public UInt32 RedBits;				// 0, 5, 8

        public UInt32 RedShl;
        public UInt32 GreenShl;
        public UInt32 BlueShl;

        public UInt32 RedShr;
        public UInt32 GreenShr;
        public UInt32 BlueShr;

        public UInt32 Unknown1;
        public UInt32 Unknown2;
        public UInt32 Unknown3;

        public override string ToString()
        {
            return
@"Magic:			""{0}""
Version:		{1}
Type:			{2}
MAT record count:	{3}
Texture count:		{4}
Transparency (?):	{5}
Bit depth:		{6}
Blue bits:		{7}
Green bits:		{8}
Red bits:		{9}
Red shift left:		{10}
Green shift left:	{11}
Blue shift left:	{12}
Red shift right:	{13}
Green shift right:	{14}
Blue shift right:	{15}
Unknown 1:		{16}
Unknown 2:		{17}
Unknown 3:		{18}".FormatInvariant(
                         Encoding.ASCII.GetString(Magic),
                         Version,
                         (int)Type + " / " + Type,
                         MatRecordCount,
                         TextureCount,
                         Transparency,
                         Bitdepth,
                         BlueBits,
                         GreenBits,
                         RedBits,
                         RedShl,
                         GreenShl,
                         BlueShl,
                         RedShr,
                         GreenShr,
                         BlueShr,
                         Unknown1,
                         Unknown2,
                         Unknown3);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MatRecordHeader
    {
        public enum RecordType
        {
            Color = 0,
            Texture = 8
        }

        [MarshalAs(UnmanagedType.U4)]
        public RecordType Type;			    // 0 = color, 8 = texture
        public UInt32 TransparentColor;		// if 16-bit or 32-bit, RGB. If 8-bit, palette offset
        public UInt32 Unknown1;				// Padding, most likely
        public UInt32 Unknown2;				// More padding
        public UInt32 Unknown3;				// Yet more padding
        public UInt32 Unknown4;				// Further padding
        public UInt32 Unknown5;
        public UInt32 Unknown6;
        public UInt32 Unknown7;
        public UInt32 Unknown8;

        public override string ToString()
        {
            return
@"Record type:		{0}
Transparent color:	{1}
Unknown 1:		{2}
Unknown 2:		{3}
Unknown 3:		{4}
Unknown 4:		{5}
Unknown 5:		{6}
Unknown 6:		{7}
Unknown 7:		{8}
Unknown 8:		{9}".FormatInvariant(
                    (int)Type + " / " + Type,
                    TransparentColor,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureDataHeader
    {
        public UInt32 SizeX;
        public UInt32 SizeY;
        public UInt32 TransparentBool;		// Use transparency? std bool, 0 = false, !0 = true

        public UInt32 Unknown1;
        public UInt32 Unknown2;

        public UInt32 MipmapCount;			// Number of mipmaps

        public override string ToString()
        {
            return
@"Width:	        	{0}
Height:		        {1}
Transparent:		{2}
Unknown 1:		{3}
Unknown 2:		{4}
Mipmap count:   	{5}".FormatInvariant(SizeX, SizeY, TransparentBool, Unknown1, Unknown2, MipmapCount);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MatColorRecordHeader
    {
        public UInt32 RecordType;			// 0
        public UInt32 ColorNum;				// index into palette
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;

        public override string ToString()
        {
            return
@"Record type:	{0}
Color index:	{1}
Unknown 1:		{2}
Unknown 2:		{3}
Unknown 3:		{4}
Unknown 4:		{5}".FormatInvariant(RecordType, ColorNum, Unknown1, Unknown2, Unknown3, Unknown4);
        }
    }
}