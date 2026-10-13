using System;
using System.Text;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000C71 RID: 3185
	public static class DataExposeUtility
	{
		// Token: 0x0600515A RID: 20826 RVA: 0x001A70EC File Offset: 0x001A52EC
		public static void LookByteArray(ref byte[] arr, string label)
		{
			if (Scribe.mode == LoadSaveMode.Saving && arr != null)
			{
				byte[] array = CompressUtility.Compress(arr);
				if (array.Length < arr.Length)
				{
					string text = Convert.ToBase64String(array).AddLineBreaksToLongString();
					Scribe_Values.Look<string>(ref text, label + "Deflate", null, false);
				}
				else
				{
					string text2 = Convert.ToBase64String(arr).AddLineBreaksToLongString();
					Scribe_Values.Look<string>(ref text2, label, null, false);
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				string text3 = null;
				Scribe_Values.Look<string>(ref text3, label + "Deflate", null, false);
				if (text3 != null)
				{
					arr = CompressUtility.Decompress(Convert.FromBase64String(text3.RemoveLineBreaks()));
					return;
				}
				Scribe_Values.Look<string>(ref text3, label, null, false);
				if (text3 != null)
				{
					arr = Convert.FromBase64String(text3.RemoveLineBreaks());
					return;
				}
				arr = null;
			}
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x001A71A4 File Offset: 0x001A53A4
		public static void LookBoolArray(ref bool[] arr, int elements, string label)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (arr.Length != elements)
				{
					Log.ErrorOnce("Bool array length mismatch for " + label, 74135877);
				}
				elements = arr.Length;
			}
			int numBytes = (elements + 7) / 8;
			byte[] arrBytes = DataExposeUtility.BytesToBits(arr, elements, numBytes);
			DataExposeUtility.LookByteArray(ref arrBytes, label);
			DataExposeUtility.BitsToBytes(ref arr, elements, arrBytes, numBytes);
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x001A71FC File Offset: 0x001A53FC
		public static void LookBoolArray(ref NativeArray<bool> arr, int elements, string label)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (arr.Length != elements)
				{
					Log.ErrorOnce("Native bool array length mismatch for " + label, 74135822);
				}
				elements = arr.Length;
			}
			bool[] array = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				array = arr.ToArray();
			}
			DataExposeUtility.LookBoolArray(ref array, elements, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (!arr.IsCreated)
				{
					arr = new NativeArray<bool>(elements, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}
				arr.CopyFrom(array);
			}
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x001A7278 File Offset: 0x001A5478
		public static void LookBitArray(ref NativeBitArray arr, int elements, string label)
		{
			bool[] array = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				array = new bool[elements];
				for (int i = 0; i < arr.Length; i++)
				{
					array[i] = arr.IsSet(i);
				}
			}
			DataExposeUtility.LookBoolArray(ref array, elements, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (!arr.IsCreated)
				{
					arr = new NativeBitArray(elements, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}
				for (int j = 0; j < array.Length; j++)
				{
					arr.Set(j, array[j]);
				}
			}
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x001A72F8 File Offset: 0x001A54F8
		private static byte[] BytesToBits(bool[] arr, int elements, int numBytes)
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				return null;
			}
			byte[] array = new byte[numBytes];
			int num = 0;
			byte b = 1;
			for (int i = 0; i < elements; i++)
			{
				if (arr[i])
				{
					byte[] array2 = array;
					int num2 = num;
					array2[num2] |= b;
				}
				b *= 2;
				if (b == 0)
				{
					b = 1;
					num++;
				}
			}
			return array;
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x001A7348 File Offset: 0x001A5548
		private static void BitsToBytes(ref bool[] arr, int elements, byte[] arrBytes, int numBytes)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				return;
			}
			if (arr == null)
			{
				arr = new bool[elements];
			}
			if (arrBytes != null && arrBytes.Length != 0)
			{
				if (arrBytes.Length != numBytes)
				{
					int num = 0;
					byte b = 1;
					for (int i = 0; i < elements; i++)
					{
						arr[i] = ((arrBytes[num] & b) > 0);
						b *= 2;
						if (b > 32)
						{
							b = 1;
							num++;
						}
					}
					return;
				}
				int num2 = 0;
				byte b2 = 1;
				for (int j = 0; j < elements; j++)
				{
					arr[j] = ((arrBytes[num2] & b2) > 0);
					b2 *= 2;
					if (b2 == 0)
					{
						b2 = 1;
						num2++;
					}
				}
			}
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x001A73DC File Offset: 0x001A55DC
		public static string AddLineBreaksToLongString(this string str)
		{
			StringBuilder stringBuilder = new StringBuilder(str.Length + (str.Length / 100 + 3) * 2 + 1);
			stringBuilder.AppendLine();
			for (int i = 0; i < str.Length; i++)
			{
				if (i % 100 == 0 && i != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(str[i]);
			}
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x001A7448 File Offset: 0x001A5648
		public static string RemoveLineBreaks(this string str)
		{
			return new StringBuilder(str).Replace("\n", "").Replace("\r", "").ToString();
		}

		// Token: 0x04003AA8 RID: 15016
		private const int NewlineInterval = 100;
	}
}
