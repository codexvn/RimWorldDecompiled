using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000118 RID: 280
	public static class GenString
	{
		// Token: 0x06000871 RID: 2161 RVA: 0x0002794C File Offset: 0x00025B4C
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0002798E File Offset: 0x00025B8E
		public static string ToStringCached(this int num)
		{
			if (num < -4999)
			{
				return num.ToString();
			}
			if (num > 4999)
			{
				return num.ToString();
			}
			return GenString.numberStrings[num + 5000];
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000279BD File Offset: 0x00025BBD
		public static IEnumerable<string> SplitBy(this string str, int chunkLength)
		{
			if (str.NullOrEmpty())
			{
				yield break;
			}
			if (chunkLength < 1)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < str.Length; i += chunkLength)
			{
				if (chunkLength > str.Length - i)
				{
					chunkLength = str.Length - i;
				}
				yield return str.Substring(i, chunkLength);
			}
			yield break;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000279D4 File Offset: 0x00025BD4
		public static StringBuilder AppendLineIfNotEmpty(this StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			return sb;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000279E7 File Offset: 0x00025BE7
		public static int GetHashCodeSafe(this string val)
		{
			if (val == null)
			{
				return 0;
			}
			return val.GetHashCode();
		}

		// Token: 0x0400044E RID: 1102
		private static string[] numberStrings = new string[10000];
	}
}
