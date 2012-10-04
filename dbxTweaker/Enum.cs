using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace dbxTweaker
{
	[TypeConverter(typeof(EnumConverter))]
	public class Enum
	{
		public List<Tuple<uint, string>> choices;
		public uint selection;
		public Enum()
		{
			this.choices = new List<Tuple<uint, string>>();
		}
		public override string ToString()
		{
			foreach (Tuple<uint, string> current in this.choices)
			{
				if (current.Item1 == this.selection)
				{
					return current.Item2;
				}
			}
			return "*emptyEnum*";
		}
	}
}
