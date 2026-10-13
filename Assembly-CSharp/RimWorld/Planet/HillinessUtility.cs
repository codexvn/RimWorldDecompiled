using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CB2 RID: 15538
	public static class HillinessUtility
	{
		// Token: 0x060151F4 RID: 86516 RVA: 0x00652750 File Offset: 0x00650950
		public static string GetLabel(this Hilliness h)
		{
			switch (h)
			{
			case Hilliness.Flat:
				return "Hilliness_Flat".Translate();
			case Hilliness.SmallHills:
				return "Hilliness_SmallHills".Translate();
			case Hilliness.LargeHills:
				return "Hilliness_LargeHills".Translate();
			case Hilliness.Mountainous:
				return "Hilliness_Mountainous".Translate();
			case Hilliness.Impassable:
				return "Hilliness_Impassable".Translate();
			default:
				Log.ErrorOnce("Hilliness label unknown: " + h.ToString(), 694362);
				return h.ToString();
			}
		}

		// Token: 0x060151F5 RID: 86517 RVA: 0x006527F9 File Offset: 0x006509F9
		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
