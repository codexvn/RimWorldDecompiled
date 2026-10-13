using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace Verse.Utility
{
	// Token: 0x02000DD2 RID: 3538
	public static class EnumUtility
	{
		// Token: 0x06005A1F RID: 23071 RVA: 0x001CDCE8 File Offset: 0x001CBEE8
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x001CDCFE File Offset: 0x001CBEFE
		public static IEnumerable<T> GetValuesReverse<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>().Reverse<T>();
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x001CDD19 File Offset: 0x001CBF19
		public static IEnumerable<Enum> GetBitFlags(this Enum input)
		{
			Array values = Enum.GetValues(input.GetType());
			int num;
			for (int i = 0; i < values.Length; i = num + 1)
			{
				if (math.countbits(Convert.ToUInt64(values.GetValue(i))) == 1 && input.HasFlag((Enum)values.GetValue(i)))
				{
					yield return (Enum)values.GetValue(i);
				}
				num = i;
			}
			yield break;
		}
	}
}
