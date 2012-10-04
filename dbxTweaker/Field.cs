using System;
using System.Collections.Generic;
namespace dbxTweaker
{
	public class Field
	{
		public FieldDescriptor descriptor;
		public long absPos;
		public object value;
		public DbxFile dbx;
		public string name
		{
			get;
			set;
		}
		public Field(FieldDescriptor fd, Dictionary<uint, string> unhash, long pos, DbxFile tmpdbx)
		{
			this.descriptor = fd;
			this.name = unhash[fd.hash];
			this.absPos = pos;
			this.dbx = tmpdbx;
		}
	}
}
