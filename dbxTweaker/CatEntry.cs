using System;
using System.IO;
namespace dbxTweaker
{
	public class CatEntry
	{
		public byte[] sha1;
		public uint offset;
		public uint size;
		public uint archiveNumber;
		public CatEntry(BinaryReader b)
		{
			this.sha1 = b.ReadBytes(20);
			this.offset = b.ReadUInt32();
			this.size = b.ReadUInt32();
			this.archiveNumber = b.ReadUInt32();
		}
	}
}
