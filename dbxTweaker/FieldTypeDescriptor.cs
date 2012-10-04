using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
namespace dbxTweaker
{
	public class FieldTypeDescriptor : PropertyDescriptor
	{
		public Field field;
		public override Type ComponentType
		{
			get
			{
				return typeof(List<Field>);
			}
		}
		public override Type PropertyType
		{
			get
			{
				return this.field.value.GetType();
			}
		}
		public override bool IsReadOnly
		{
			get
			{
				FieldType type = this.field.descriptor.type;
				if (type <= FieldType.Guid)
				{
					if (type != FieldType.Inheritance && type != FieldType.Complex && type != FieldType.Guid)
					{
						return false;
					}
				}
				else
				{
					if (type != FieldType.Array && type != FieldType.String && type != FieldType.Complex2)
					{
						return false;
					}
				}
				return true;
			}
		}
		public FieldTypeDescriptor(string name, Field elem) : base(name, null)
		{
			this.field = elem;
		}
		public override object GetValue(object component)
		{
			FieldType type = this.field.descriptor.type;
			if (type != FieldType.Guid)
			{
				if (type == FieldType.RawGuid)
				{
					return BiConverter.ToString(this.field.value as byte[]);
				}
				return this.field.value;
			}
			else
			{
				uint num = (uint)this.field.value;
				if (num >> 31 == 1u)
				{
					Tuple<byte[], byte[]> tuple = this.field.dbx.externalGuids[(int)(num & 2147483647u)];
					if (this.field.dbx.catHandle.guidTable.ContainsKey(tuple.Item1))
					{
						return this.field.dbx.catHandle.guidTable[tuple.Item1] + "/" + BiConverter.ToString(tuple.Item2);
					}
					return BiConverter.ToString(tuple.Item1) + "-" + BiConverter.ToString(tuple.Item2);
				}
				else
				{
					if (num != 0u)
					{
						return BiConverter.ToString(this.field.dbx.internalGuids[(int)(num - 1u)]);
					}
					return "*nullGuid*";
				}
			}
		}
		public override void SetValue(object component, object value)
		{
			BinaryWriter writer = this.field.dbx.catHandle.getWriter(this.field.dbx);
			writer.Seek((int)(this.field.absPos + (long)((ulong)this.field.dbx.catEntry.offset)), SeekOrigin.Begin);
			if (this.field.descriptor.type != FieldType.Enum)
			{
				this.field.value = value;
			}
			FieldType type = this.field.descriptor.type;
			if (type <= FieldType.UInt16)
			{
				if (type <= FieldType.Bool)
				{
					if (type == FieldType.Enum)
					{
						Enum @enum = (Enum)this.field.value;
						uint num = uint.Parse(value.ToString().Substring(0, value.ToString().IndexOf(" ")));
						writer.Write(num);
						@enum.selection = num;
						return;
					}
					if (type != FieldType.Bool)
					{
						return;
					}
					writer.Write((bool)value);
					return;
				}
				else
				{
					if (type == FieldType.Int8)
					{
						writer.Write((sbyte)value);
						return;
					}
					if (type != FieldType.UInt16)
					{
						return;
					}
					writer.Write((ushort)value);
					return;
				}
			}
			else
			{
				if (type <= FieldType.Int32)
				{
					if (type == FieldType.Int16)
					{
						writer.Write((short)value);
						return;
					}
					if (type != FieldType.Int32)
					{
						return;
					}
					writer.Write((int)value);
					return;
				}
				else
				{
					if (type == FieldType.UInt32)
					{
						writer.Write((uint)value);
						return;
					}
					if (type == FieldType.Single)
					{
						writer.Write((float)value);
						return;
					}
					if (type != FieldType.Double)
					{
						return;
					}
					writer.Write((double)value);
					return;
				}
			}
		}
		public override void ResetValue(object component)
		{
		}
		public override bool ShouldSerializeValue(object component)
		{
			return !this.IsReadOnly;
		}
		public override bool CanResetValue(object component)
		{
			return false;
		}
	}
}
