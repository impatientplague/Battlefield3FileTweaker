using System;
using System.IO;
namespace dbxTweaker
{
	public struct DbxHeader
	{
		public uint stringOffset;
		public uint lenStringToEOF;
		public uint numExternalGuid;
		public uint _null;
		public uint numInstanceRepeater;
		public uint numComplex;
		public uint numField;
		public uint lenKeyword;
		public uint lenString;
		public uint numArrayRepeater;
		public uint lenPayload;
		public byte[] fileGuid;
		public byte[] primaryInstanceGuid;
		public DbxHeader(BinaryReader b)
		{
			this.stringOffset = b.ReadUInt32();
			this.lenStringToEOF = b.ReadUInt32();
			this.numExternalGuid = b.ReadUInt32();
			this._null = b.ReadUInt32();
			this.numInstanceRepeater = b.ReadUInt32();
			this.numComplex = b.ReadUInt32();
			this.numField = b.ReadUInt32();
			this.lenKeyword = b.ReadUInt32();
			this.lenString = b.ReadUInt32();
			this.numArrayRepeater = b.ReadUInt32();
			this.lenPayload = b.ReadUInt32();
			this.fileGuid = b.ReadBytes(16);
			this.primaryInstanceGuid = b.ReadBytes(16);
		}
	}
}
