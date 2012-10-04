using System;
using System.IO;
namespace dbxTweaker
{
	public struct InstanceRepeater
	{
		public uint internalCount;
		public uint repetitions;
		public uint complexIndex;
		public InstanceRepeater(BinaryReader b)
		{
			this.internalCount = b.ReadUInt32();
			this.repetitions = b.ReadUInt32();
			this.complexIndex = b.ReadUInt32();
		}
	}
}
