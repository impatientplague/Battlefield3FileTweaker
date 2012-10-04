using System;
using System.IO;
namespace dbxTweaker
{
	public struct ComplexDescriptor
	{
		public uint hash;
		public uint fieldStartIndex;
		public byte numField;
		public byte alignment;
		public ushort type;
		public ushort size;
		public ushort secondarySize;
		public ComplexDescriptor(BinaryReader b)
		{
			this.hash = b.ReadUInt32();
			this.fieldStartIndex = b.ReadUInt32();
			this.numField = b.ReadByte();
			this.alignment = b.ReadByte();
			this.type = b.ReadUInt16();
			this.size = b.ReadUInt16();
			this.secondarySize = b.ReadUInt16();
		}
	}
}
