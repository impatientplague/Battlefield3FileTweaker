using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace dbxTweaker
{
	public class EnumConverter : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			Enum @enum = ((FieldTypeDescriptor)context.PropertyDescriptor).field.value as Enum;
			List<string> list = new List<string>();
			foreach (Tuple<uint, string> current in @enum.choices)
			{
				list.Add(current.Item1 + " " + current.Item2);
			}
			return new TypeConverter.StandardValuesCollection(list.ToArray());
		}
	}
}
