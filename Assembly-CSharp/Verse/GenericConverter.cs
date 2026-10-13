using System;
using System.Linq.Expressions;

namespace Verse
{
	// Token: 0x0200013C RID: 316
	public static class GenericConverter
	{
		// Token: 0x060009A8 RID: 2472 RVA: 0x0002DABA File Offset: 0x0002BCBA
		public static OutType Convert<InType, OutType>(InType value)
		{
			return GenericConverter.StaticGenericCache<InType, OutType>.cachedFunc(value);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002DAC8 File Offset: 0x0002BCC8
		private static Func<InType, OutType> GenerateFunc<InType, OutType>()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(InType));
			return Expression.Lambda<Func<InType, OutType>>(Expression.Convert(parameterExpression, typeof(OutType)), new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x0200013D RID: 317
		private static class StaticGenericCache<InType, OutType>
		{
			// Token: 0x040004DC RID: 1244
			public static Func<InType, OutType> cachedFunc = GenericConverter.GenerateFunc<InType, OutType>();
		}
	}
}
