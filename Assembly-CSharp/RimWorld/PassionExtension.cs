using System;

namespace RimWorld
{
	// Token: 0x02002514 RID: 9492
	public static class PassionExtension
	{
		// Token: 0x0600CF60 RID: 53088 RVA: 0x003CDAF7 File Offset: 0x003CBCF7
		public static Passion IncrementPassion(this Passion passion)
		{
			if (passion == Passion.None)
			{
				return Passion.Minor;
			}
			if (passion == Passion.Minor)
			{
				return Passion.Major;
			}
			return passion;
		}
	}
}
