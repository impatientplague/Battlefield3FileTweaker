using System;
using System.IO;
namespace dbxTweaker
{
	public class CasHeader
	{
		public uint magic;
		public byte[] sha1;
		public uint size;
		public uint _null;
		public CasHeader(BinaryReader b)
		{
			this.magic = b.ReadUInt32();
			this.sha1 = b.ReadBytes(20);
			this.size = b.ReadUInt32();
			this._null = b.ReadUInt32();
		}
	}
}
