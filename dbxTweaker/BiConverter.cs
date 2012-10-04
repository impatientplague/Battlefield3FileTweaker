using System;
namespace dbxTweaker
{
	public static class BiConverter
	{
		public static byte[] unhexlify(string str)
		{
			if (str.Length % 2 != 0)
			{
				throw new Exception("Cannot unhexlify. Wrong length.");
			}
			byte[] array = new byte[str.Length / 2];
			for (int i = 0; i < str.Length; i += 2)
			{
				char i2 = str[i];
				char i3 = str[i + 1];
				int num = (int)(BiConverter.UnHexValue(i2) * 16 + BiConverter.UnHexValue(i3));
				array[i / 2] = (byte)num;
			}
			return array;
		}
		private static byte UnHexValue(char i)
		{
			if (i >= '0' && i < ':')
			{
				return (byte)(i - '0');
			}
			if (i >= 'A' && i < 'G')
			{
				return (byte)(i - 'A' + '\n');
			}
			if (i >= 'a' && i < 'g')
			{
				return (byte)(i - 'a' + '\n');
			}
			throw new Exception("Cannot unhexlify. Invalid character.");
		}
		private static char GetHexValue(int i)
		{
			if (i < 10)
			{
				return (char)(i + 48);
			}
			return (char)(i - 10 + 65);
		}
		public static string ToString(byte[] value, int startIndex, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("byteArray");
			}
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length * 2;
			char[] array = new char[num];
			int num2 = startIndex;
			for (int i = 0; i < num; i += 2)
			{
				byte b = value[num2++];
				array[i] = BiConverter.GetHexValue((int)(b / 16));
				array[i + 1] = BiConverter.GetHexValue((int)(b % 16));
			}
			return new string(array);
		}
		public static string ToString(byte[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BiConverter.ToString(value, 0, value.Length);
		}
		public static string ToString(byte[] value, int startIndex)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BiConverter.ToString(value, startIndex, value.Length - startIndex);
		}
		public static bool ToBoolean(byte[] value, int startIndex)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "ArgumentOutOfRange_NeedNonNegNum");
			}
			if (startIndex > value.Length - 1)
			{
				throw new ArgumentOutOfRangeException("startIndex", "ArgumentOutOfRange_Index");
			}
			return value[startIndex] != 0;
		}
	}
}
