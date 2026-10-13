using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020028BF RID: 10431
	public static class MonolithGenerationMethodExtension
	{
		// Token: 0x0600E430 RID: 58416 RVA: 0x0042A873 File Offset: 0x00428A73
		public static string ToStringHuman(this MonolithGenerationMethod method)
		{
			if (method == MonolithGenerationMethod.Disabled)
			{
				return "MonolithGenerationMethod_Disabled".Translate();
			}
			if (method != MonolithGenerationMethod.NearColonists)
			{
				throw new NotImplementedException();
			}
			return "MonolithGenerationMethod_NearColonists".Translate();
		}
	}
}
