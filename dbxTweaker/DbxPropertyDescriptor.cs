using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace dbxTweaker
{
	public class DbxPropertyDescriptor : PropertyDescriptor
	{
		private Complex complex;
		public override Type ComponentType
		{
			get
			{
				return typeof(List<Complex>);
			}
		}
		public override Type PropertyType
		{
			get
			{
				return this.complex.GetType();
			}
		}
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}
		public DbxPropertyDescriptor(string name, Complex elem, DbxFile dbxa) : base(name, null)
		{
			this.complex = elem;
		}
		public override object GetValue(object component)
		{
			return this.complex;
		}
		public override void ResetValue(object component)
		{
		}
		public override void SetValue(object component, object value)
		{
		}
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
		public override bool CanResetValue(object component)
		{
			return false;
		}
	}
}
