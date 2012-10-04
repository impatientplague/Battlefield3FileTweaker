using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
namespace dbxTweaker
{
	[TypeConverter(typeof(DbxConverter))]
	public class DbxFile
	{
		public CasCat catHandle;
		public CatEntry catEntry;
		private BinaryReader binaryReader;
		public DbxHeader dbxHeader;
		public List<Tuple<byte[], byte[]>> externalGuids;
		public List<byte[]> internalGuids;
		private Dictionary<uint, string> unhash;
		private List<FieldDescriptor> fieldDescriptors;
		private List<ComplexDescriptor> complexDescriptors;
		private List<ArrayRepeater> arrayRepeaters;
		private List<InstanceRepeater> instanceRepeaters;
		private uint arraySectionStart;
		private uint stringSectionStart;
		private uint payloadSectionStart;
		public string filename;
		public Dictionary<string, Complex> instances;
		public DbxFile(BinaryReader fCas, CatEntry cat, CasCat ct)
		{
			this.catHandle = ct;
			this.catEntry = cat;
			this.InitializeMetaData(fCas);
			this.binaryReader = fCas;
		}
		public void InitializeMetaData(BinaryReader fCas)
		{
			this.dbxHeader = new DbxHeader(fCas);
			this.arraySectionStart = this.catEntry.offset + this.dbxHeader.stringOffset + this.dbxHeader.lenString + this.dbxHeader.lenPayload;
			this.stringSectionStart = this.catEntry.offset + this.dbxHeader.stringOffset;
			this.payloadSectionStart = this.catEntry.offset + this.dbxHeader.stringOffset + this.dbxHeader.lenString;
			this.externalGuids = new List<Tuple<byte[], byte[]>>((int)this.dbxHeader.numExternalGuid);
			int num = 0;
			while ((long)num < (long)((ulong)this.dbxHeader.numExternalGuid))
			{
				this.externalGuids.Add(new Tuple<byte[], byte[]>(fCas.ReadBytes(16), fCas.ReadBytes(16)));
				num++;
			}
			this.unhash = new Dictionary<uint, string>();
			byte[] array = new byte[this.dbxHeader.lenKeyword];
			fCas.BaseStream.Read(array, 0, (int)this.dbxHeader.lenKeyword);
			int num2 = 0;
			int num3 = 0;
			while ((long)num3 < (long)((ulong)this.dbxHeader.lenKeyword))
			{
				if (array[num3] == 0)
				{
					string @string = Encoding.ASCII.GetString(array, num2, num3 - num2);
					if (@string != "")
					{
						this.unhash.Add(DbxFile.hasher(@string), @string);
					}
					num2 = num3 + 1;
				}
				num3++;
			}
			this.fieldDescriptors = new List<FieldDescriptor>((int)this.dbxHeader.numField);
			int num4 = 0;
			while ((long)num4 < (long)((ulong)this.dbxHeader.numField))
			{
				FieldDescriptor item = new FieldDescriptor(fCas);
				this.fieldDescriptors.Add(item);
				num4++;
			}
			this.complexDescriptors = new List<ComplexDescriptor>((int)this.dbxHeader.numComplex);
			int num5 = 0;
			while ((long)num5 < (long)((ulong)this.dbxHeader.numComplex))
			{
				ComplexDescriptor item2 = new ComplexDescriptor(fCas);
				this.complexDescriptors.Add(item2);
				num5++;
			}
			this.instanceRepeaters = new List<InstanceRepeater>();
			int num6 = 0;
			while ((long)num6 < (long)((ulong)this.dbxHeader.numInstanceRepeater))
			{
				this.instanceRepeaters.Add(new InstanceRepeater(fCas));
				num6++;
			}
			long num7 = 16L - (fCas.BaseStream.Position - (long)((ulong)this.catEntry.offset)) % 16L;
			if (num7 != 16L)
			{
				fCas.ReadBytes((int)num7);
			}
			this.arrayRepeaters = new List<ArrayRepeater>();
			int num8 = 0;
			while ((long)num8 < (long)((ulong)this.dbxHeader.numArrayRepeater))
			{
				this.arrayRepeaters.Add(new ArrayRepeater(fCas));
				num8++;
			}
		}
		public void InitializeFilename(BinaryReader fCas)
		{
			fCas.BaseStream.Position = (long)((ulong)this.payloadSectionStart);
			foreach (InstanceRepeater current in this.instanceRepeaters)
			{
				int num = 0;
				while ((long)num < (long)((ulong)current.repetitions))
				{
					byte[] first = fCas.ReadBytes(16);
					if (first.SequenceEqual(this.dbxHeader.primaryInstanceGuid))
					{
						ComplexDescriptor complexDescriptor = this.complexDescriptors[(int)current.complexIndex];
						FieldDescriptor fieldDescriptor = this.fieldDescriptors[(int)complexDescriptor.fieldStartIndex];
						ComplexDescriptor complexDescriptor2 = this.complexDescriptors[(int)fieldDescriptor.reference];
						while (true)
						{
							if (complexDescriptor2.numField > 1)
							{
								for (int i = 0; i < (int)complexDescriptor2.numField; i++)
								{
									fieldDescriptor = this.fieldDescriptors[(int)(complexDescriptor2.fieldStartIndex + (uint)i)];
									if (!(this.unhash[fieldDescriptor.hash] != "Name"))
									{
										goto IL_199;
									}
								}
							}
							if (complexDescriptor2.numField == 0)
							{
								break;
							}
							complexDescriptor2 = this.complexDescriptors[(int)this.fieldDescriptors[(int)complexDescriptor2.fieldStartIndex].reference];
						}
						for (int j = 0; j < (int)complexDescriptor.numField; j++)
						{
							fieldDescriptor = this.fieldDescriptors[(int)(complexDescriptor.fieldStartIndex + (uint)j)];
							if (!(this.unhash[fieldDescriptor.hash] != "Name"))
							{
								goto IL_199;
							}
						}
						throw new Exception("No Name found.");
						IL_199:
						fCas.BaseStream.Position += (long)((ulong)fieldDescriptor.offset);
						fCas.BaseStream.Position = (long)((ulong)(this.stringSectionStart + fCas.ReadUInt32()));
						this.filename = DbxFile.readNullTerminatedString(fCas);
						return;
					}
					fCas.BaseStream.Position += (long)((ulong)this.complexDescriptors[(int)current.complexIndex].size);
					num++;
				}
			}
		}
		public void InitializePayload()
		{
			this.internalGuids = new List<byte[]>();
			this.binaryReader.BaseStream.Position = (long)((ulong)this.payloadSectionStart);
			this.instances = new Dictionary<string, Complex>();
			foreach (InstanceRepeater current in this.instanceRepeaters)
			{
				int num = 0;
				while ((long)num < (long)((ulong)current.repetitions))
				{
					byte[] array = this.binaryReader.ReadBytes(16);
					this.internalGuids.Add(array);
					Complex value = this.parseComplex((int)current.complexIndex, this.binaryReader);
					this.instances.Add(BiConverter.ToString(array), value);
					num++;
				}
			}
		}
		private Complex parseComplex(int reference, BinaryReader b)
		{
			ComplexDescriptor cd = this.complexDescriptors[reference];
			Complex complex = new Complex(cd, this.unhash);
			long position = b.BaseStream.Position;
			for (int i = 0; i < (int)cd.numField; i++)
			{
				FieldDescriptor fieldDesc = this.fieldDescriptors[i + (int)complex.descriptor.fieldStartIndex];
				b.BaseStream.Position = position + (long)((ulong)fieldDesc.offset);
				complex.fields.Add(this.parseField(fieldDesc, b));
			}
			b.BaseStream.Position = position + (long)((ulong)complex.descriptor.size);
			return complex;
		}
		private Field parseField(FieldDescriptor fieldDesc, BinaryReader b)
		{
			Field field = new Field(fieldDesc, this.unhash, b.BaseStream.Position - (long)((ulong)this.catEntry.offset), this);
			FieldType type = fieldDesc.type;
			if (type <= FieldType.Int8)
			{
				if (type <= FieldType.Array)
				{
					if (type <= FieldType.Complex)
					{
						if (type != FieldType.Inheritance && type != FieldType.Complex)
						{
							return field;
						}
					}
					else
					{
						if (type == FieldType.Guid)
						{
							field.value = b.ReadUInt32();
							return field;
						}
						if (type != FieldType.Array)
						{
							return field;
						}
						ArrayRepeater arrayRepeater = this.arrayRepeaters[b.ReadInt32()];
						ComplexDescriptor cd = this.complexDescriptors[(int)fieldDesc.reference];
						FieldDescriptor fieldDesc2 = this.fieldDescriptors[(int)cd.fieldStartIndex];
						if (arrayRepeater.repetitions == 0u)
						{
							field.value = "*nullArray*";
							return field;
						}
						b.BaseStream.Position = (long)((ulong)(this.arraySectionStart + arrayRepeater.offset));
						Complex complex = new Complex(cd, this.unhash);
						int num = 0;
						while ((long)num < (long)((ulong)arrayRepeater.repetitions))
						{
							complex.fields.Add(this.parseField(fieldDesc2, b));
							num++;
						}
						field.value = complex;
						return field;
					}
				}
				else
				{
					if (type <= FieldType.String)
					{
						if (type == FieldType.Enum)
						{
							Enum @enum = new Enum();
							@enum.selection = b.ReadUInt32();
							ComplexDescriptor complexDescriptor = this.complexDescriptors[(int)fieldDesc.reference];
							for (int i = 0; i < (int)complexDescriptor.numField; i++)
							{
								Field field2 = new Field(this.fieldDescriptors[(int)(complexDescriptor.fieldStartIndex + (uint)i)], this.unhash, b.BaseStream.Position, this);
								@enum.choices.Add(new Tuple<uint, string>(field2.descriptor.offset, field2.name));
							}
							field.value = @enum;
							return field;
						}
						if (type != FieldType.String)
						{
							return field;
						}
						long position = b.BaseStream.Position;
						b.BaseStream.Position = (long)((ulong)(this.stringSectionStart + b.ReadUInt32()));
						field.value = DbxFile.readNullTerminatedString(b);
						b.BaseStream.Position = position + 4L;
						return field;
					}
					else
					{
						if (type == FieldType.Bool)
						{
							field.value = (b.ReadByte() != 0);
							return field;
						}
						if (type != FieldType.Int8)
						{
							return field;
						}
						field.value = b.ReadByte();
						return field;
					}
				}
			}
			else
			{
				if (type <= FieldType.UInt32)
				{
					if (type <= FieldType.Int16)
					{
						if (type == FieldType.UInt16)
						{
							field.value = b.ReadUInt16();
							return field;
						}
						if (type != FieldType.Int16)
						{
							return field;
						}
						field.value = b.ReadInt16();
						return field;
					}
					else
					{
						if (type == FieldType.Int32)
						{
							field.value = b.ReadInt32();
							return field;
						}
						if (type != FieldType.UInt32)
						{
							return field;
						}
						field.value = b.ReadUInt32();
						return field;
					}
				}
				else
				{
					if (type <= FieldType.Double)
					{
						if (type == FieldType.Single)
						{
							field.value = b.ReadSingle();
							return field;
						}
						if (type != FieldType.Double)
						{
							return field;
						}
						field.value = b.ReadDouble();
						return field;
					}
					else
					{
						if (type == FieldType.RawGuid)
						{
							field.value = b.ReadBytes(16);
							return field;
						}
						if (type != FieldType.Complex2)
						{
							return field;
						}
					}
				}
			}
			field.value = this.parseComplex((int)fieldDesc.reference, b);
			return field;
		}
		public static string readNullTerminatedString(BinaryReader b)
		{
			StringBuilder stringBuilder = new StringBuilder(8);
			while (true)
			{
				byte b2 = b.ReadByte();
				if (b2 == 0)
				{
					break;
				}
				stringBuilder.Append((char)b2);
			}
			return stringBuilder.ToString();
		}
		private static uint hasher(string s)
		{
			uint num = 5381u;
			for (int i = 0; i < s.Length; i++)
			{
				byte b = (byte)s[i];
				num = ((uint)b ^ 33u * num);
			}
			return num;
		}
	}
}
