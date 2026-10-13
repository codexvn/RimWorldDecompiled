using System;

namespace Verse
{
	// Token: 0x02000C72 RID: 3186
	public static class DataSerializeUtility
	{
		// Token: 0x06005162 RID: 20834 RVA: 0x001A7474 File Offset: 0x001A5674
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int i = 0; i < elements; i++)
			{
				array[i] = reader(i);
			}
			return array;
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x000FD030 File Offset: 0x000FB230
		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x000FD030 File Offset: 0x000FB230
		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001A74A0 File Offset: 0x001A56A0
		public static void LoadByte(byte[] arr, int elements, Action<int, byte> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, arr[i]);
			}
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x001A74CC File Offset: 0x001A56CC
		public static byte[] SerializeUshort(int elements, Func<int, ushort> reader)
		{
			byte[] array = new byte[elements * 2];
			for (int i = 0; i < elements; i++)
			{
				ushort num = reader(i);
				array[i * 2] = (byte)(num & 255);
				array[i * 2 + 1] = (byte)(num >> 8 & 255);
			}
			return array;
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x001A7518 File Offset: 0x001A5718
		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (int i) => data[i]);
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x001A754C File Offset: 0x001A574C
		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x001A7590 File Offset: 0x001A5790
		public static void LoadUshort(byte[] arr, int elements, Action<int, ushort> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (ushort)((int)arr[i * 2] | (int)arr[i * 2 + 1] << 8));
			}
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x001A75C8 File Offset: 0x001A57C8
		public static byte[] SerializeInt(int elements, Func<int, int> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int i = 0; i < elements; i++)
			{
				int num = reader(i);
				array[i * 4] = (byte)(num & 255);
				array[i * 4 + 1] = (byte)(num >> 8 & 255);
				array[i * 4 + 2] = (byte)(num >> 16 & 255);
				array[i * 4 + 3] = (byte)(num >> 24 & 255);
			}
			return array;
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x001A7638 File Offset: 0x001A5838
		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (int i) => data[i]);
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x001A766C File Offset: 0x001A586C
		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x001A76B0 File Offset: 0x001A58B0
		public static void LoadInt(byte[] arr, int elements, Action<int, int> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (int)arr[i * 4] | (int)arr[i * 4 + 1] << 8 | (int)arr[i * 4 + 2] << 16 | (int)arr[i * 4 + 3] << 24);
			}
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x001A7700 File Offset: 0x001A5900
		public static byte[] SerializeUint(int elements, Func<int, uint> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int i = 0; i < elements; i++)
			{
				uint num = reader(i);
				array[i * 4] = (byte)(num & 255U);
				array[i * 4 + 1] = (byte)(num >> 8 & 255U);
				array[i * 4 + 2] = (byte)(num >> 16 & 255U);
				array[i * 4 + 3] = (byte)(num >> 24 & 255U);
			}
			return array;
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x001A7770 File Offset: 0x001A5970
		public static void LoadUint(byte[] arr, int elements, Action<int, uint> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (uint)((int)arr[i * 4] | (int)arr[i * 4 + 1] << 8 | (int)arr[i * 4 + 2] << 16 | (int)arr[i * 4 + 3] << 24));
			}
		}
	}
}
