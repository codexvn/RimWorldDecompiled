using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200010E RID: 270
	public static class GenDictionary
	{
		// Token: 0x060007FE RID: 2046 RVA: 0x00025AD0 File Offset: 0x00023CD0
		public static string ToStringFullContents<K, V>(this Dictionary<K, V> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<K, V> keyValuePair in dict)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				K key = keyValuePair.Key;
				string str = key.ToString();
				string str2 = ": ";
				V value = keyValuePair.Value;
				stringBuilder2.AppendLine(str + str2 + value.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00025B60 File Offset: 0x00023D60
		public static bool NullOrEmpty<K, V>(this Dictionary<K, V> dict)
		{
			return dict == null || dict.Count == 0;
		}
	}
}
