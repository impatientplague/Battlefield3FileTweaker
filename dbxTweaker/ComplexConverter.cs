using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
namespace dbxTweaker
{
	public class ComplexConverter : ExpandableObjectConverter
	{
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			List<Field> fields = (value as Complex).fields;
			List<FieldTypeDescriptor> list = new List<FieldTypeDescriptor>();
			for (int i = 0; i < fields.Count; i++)
			{
				list.Add(new FieldTypeDescriptor(fields[i].name, fields[i]));
			}
			return new PropertyDescriptorCollection(list.ToArray());
		}
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return (value as Complex).name;
		}
	}
}
