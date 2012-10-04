using System;
using System.IO;
namespace dbxTweaker
{
	public struct FieldDescriptor
	{
		public uint hash;
		public FieldType type;
		public ushort reference;
		public uint offset;
		public uint secondaryOffset;
		public FieldDescriptor(BinaryReader b)
		{
			this.hash = b.ReadUInt32();
			this.type = (FieldType)b.ReadUInt16();
			this.reference = b.ReadUInt16();
			this.offset = b.ReadUInt32();
			this.secondaryOffset = b.ReadUInt32();
		}
	}
}
