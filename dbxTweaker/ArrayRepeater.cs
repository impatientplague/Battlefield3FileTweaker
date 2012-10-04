using System;
using System.IO;
namespace dbxTweaker
{
	public struct ArrayRepeater
	{
		public uint offset;
		public uint repetitions;
		public uint complexIndex;
		public ArrayRepeater(BinaryReader b)
		{
			this.offset = b.ReadUInt32();
			this.repetitions = b.ReadUInt32();
			this.complexIndex = b.ReadUInt32();
		}
	}
}
