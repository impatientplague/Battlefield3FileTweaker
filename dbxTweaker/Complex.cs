using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace dbxTweaker
{
	[TypeConverter(typeof(ComplexConverter))]
	public class Complex
	{
		public ComplexDescriptor descriptor;
		public string name;
		public List<Field> fields;
		public Complex(ComplexDescriptor cd, Dictionary<uint, string> unhash)
		{
			this.descriptor = cd;
			this.name = unhash[this.descriptor.hash];
			this.fields = new List<Field>();
		}
	}
}
