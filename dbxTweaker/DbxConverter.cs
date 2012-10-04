using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
namespace dbxTweaker
{
	public class DbxConverter : ExpandableObjectConverter
	{
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			KeyValuePair<string, Complex>[] array = (value as DbxFile).instances.ToArray<KeyValuePair<string, Complex>>();
			List<DbxPropertyDescriptor> list = new List<DbxPropertyDescriptor>();
			KeyValuePair<string, Complex>[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				KeyValuePair<string, Complex> keyValuePair = array2[i];
				list.Add(new DbxPropertyDescriptor(keyValuePair.Key, keyValuePair.Value, value as DbxFile));
			}
			return new PropertyDescriptorCollection(list.ToArray());
		}
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return (value as DbxFile).filename;
		}
	}
}
