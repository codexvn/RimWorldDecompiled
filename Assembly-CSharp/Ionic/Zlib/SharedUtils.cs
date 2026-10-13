using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02003EC0 RID: 16064
	internal class SharedUtils
	{
		// Token: 0x06016086 RID: 90246 RVA: 0x00698B26 File Offset: 0x00696D26
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x06016087 RID: 90247 RVA: 0x00698B30 File Offset: 0x00696D30
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x06016088 RID: 90248 RVA: 0x00698B71 File Offset: 0x00696D71
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x06016089 RID: 90249 RVA: 0x00698B7E File Offset: 0x00696D7E
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
